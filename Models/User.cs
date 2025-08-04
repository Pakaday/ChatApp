using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public required string Id { get; set; }
		public required string Email { get; set; }
		public string? DisplayName { get; set; }

		public ICollection<Message> SentMessages { get; set; } = new List<Message>();
		public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
	}
}
