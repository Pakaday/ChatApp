using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using ChatApp.Models;

namespace ChatApp.Controllers
{
	[ApiController]
	[Route("api/messages")]
	public class MessagesController : ControllerBase
	{
		private readonly ChatDbContext _context;

		public MessagesController(ChatDbContext context)
		{
			_context = context;
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetMessages(string userId)
		{
			var messages = await _context.Messages
				.Include(m => m.Sender)
				.Include(m => m.Recipient)
				.Where(m => m.UserId == userId || m.RecipientId == userId)
				.OrderBy(m => m.CreatedAt)
				.ToListAsync();

			var result = messages.Select(m => new
			{
				from = m.Sender.DisplayName ?? m.Sender.Email,
				to = m.Recipient.DisplayName ?? m.Recipient.Email,
				message = m.Content,
				createdAt = m.CreatedAt
			});

			return Ok(result);
		}
	}
}
