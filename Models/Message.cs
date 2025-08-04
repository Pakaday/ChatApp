using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
	public class Message
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public required string Id { get; set; }
		public  required string UserId { get; set; }
		public User Sender { get; set; } = null!;
		public required string RecipientId { get; set; }
		public User Recipient { get; set; } = null!;
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public bool IsDeleted { get; set; } = false;
	}
}
