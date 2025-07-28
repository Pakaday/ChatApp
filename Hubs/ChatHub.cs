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
			var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user);
			if (dbUser == null) return;

			var newMessage = new Message
			{
				UserId = dbUser.Id,
				Content = message,
				CreatedAt = DateTime.UtcNow,
				IsDeleted = false
			};

			_context.Messages.Add(newMessage);
			await _context.SaveChangesAsync();

			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}
	}
}
