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
		public async Task SendMessage(string senderEmail, string recipientEmail, string message)
		{
			if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(message))
			{
				Console.WriteLine("[ChatHub] One or both parameters are null or empty.");
				return;
			}

			var sender = await _context.Users.FirstOrDefaultAsync(u => u.Email == senderEmail);
			var recipient = await _context.Users.FirstOrDefaultAsync(u => u.Email == recipientEmail);

			if (sender == null || recipient == null)
			{
				Console.WriteLine($"[ChatHub] Sender or recipient not found: {senderEmail}, {recipientEmail}");
				return;
			}


			var newMessage = new Message
			{
				Id = Guid.NewGuid().ToString(),
				UserId = sender.Id,
				RecipientId = recipient.Id,
				Content = message,
				CreatedAt = DateTime.UtcNow,
				IsDeleted = false,
			};

			_context.Messages.Add(newMessage);
			await _context.SaveChangesAsync();

			Console.WriteLine($"[ChatHub] Stored & sent message from '{senderEmail}': {recipientEmail}");
			await Clients.All.SendAsync("ReceiveMessage", sender.DisplayName ?? sender.Email, message);
		}

		public override async Task OnConnectedAsync()
		{
			Console.WriteLine("[ChatHub] Client connected.");
			await base.OnConnectedAsync();
		}
	}

}
