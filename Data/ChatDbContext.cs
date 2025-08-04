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
	

	protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Message>()
				.HasOne(m => m.Sender)
				.WithMany(u => u.SentMessages)
				.HasForeignKey(m => m.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Message>()
				.HasOne(m => m.Recipient)
				.WithMany(u => u.ReceivedMessages)
				.HasForeignKey(m => m.RecipientId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
