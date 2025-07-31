using Microsoft.AspNetCore.SignalR;
using ChatApp.Data;
using Microsoft.EntityFrameworkCore;
using ChatApp.Models;

namespace ChatApp.Hubs
{
	public class ChatHub : Hub
	{

		private readonly ChatDbContext _context;

		public ChatHub(ChatDbContext context)
		{
			_context = context;
		}
		public async Task SendMessage(string user, string message)
		{
			if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(message))
			{
				Console.WriteLine("[ChatHub] One or both parameters are null or empty.");
				return;
			}

			var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user);
			if (dbUser == null)
			{
				Console.WriteLine($"[ChatHub] No user found in DB with username: {user}");
				return;
			}

			var newMessage = new Message
			{
				Id = Guid.NewGuid().ToString(),
				UserId = dbUser.Id,
				Content = message,
				CreatedAt = DateTime.UtcNow,
				IsDeleted = false,
				User = dbUser
			};

			_context.Messages.Add(newMessage);
			await _context.SaveChangesAsync();

			Console.WriteLine($"[ChatHub] Stored & sent message from '{user}': {message}");
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

		public override async Task OnConnectedAsync()
		{
			Console.WriteLine("[ChatHub] Client connected.");
			await base.OnConnectedAsync();
		}
	}

}
