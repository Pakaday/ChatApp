using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
	[ApiController]
	[Route("api/users")]
	public class UsersController : ControllerBase
	{
		private readonly ChatDbContext _context;

		public UsersController(ChatDbContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser([FromBody] User user)
		{
			Console.WriteLine($"[UsersController] POST received: {user.Id}, {user.Username}");

			var exists = await _context.Users.AnyAsync(u => u.Id == user.Id);
			if (exists)
			{
				Console.WriteLine($"[UsersController] User already exists: {user.Id}");
				return Ok();
			}

			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			Console.WriteLine($"[UsersController] New user created: {user.Username}");
			return Ok();
		}
	}
}
