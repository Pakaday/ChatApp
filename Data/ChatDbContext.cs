using Microsoft.EntityFrameworkCore;
using ChatApp.Models;

namespace ChatApp.Data
{
	public class ChatDbContext : DbContext
	{
		public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Message> Messages { get; set; } = null!;
	}
}
