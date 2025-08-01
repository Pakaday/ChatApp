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

		[HttpGet]
		public async Task<IActionResult> GetMessages()
		{
			var messages = await _context.Messages
				.Include(m => m.User)
				.OrderBy(m => m.CreatedAt)
				.ToListAsync();

			var result = messages.Select(m => new
			{
				user = m.User.DisplayName ?? m.User.Email,
				message = m.Content,
				createdAt = m.CreatedAt
			});

			return Ok(result);
		}
	}
}
