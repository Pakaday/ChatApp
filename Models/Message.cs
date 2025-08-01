using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
	public class Message
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public required string Id { get; set; }
		[ForeignKey("User")]
		public  required string UserId { get; set; }
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public bool IsDeleted { get; set; } = false;
		public required User User { get; set; }
	}
}
