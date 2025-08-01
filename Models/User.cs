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
		public string Username { get; set; } = string.Empty;
		public string? DisplayName { get; set; }
		public string? PasswordHash { get; set; }

		public ICollection<Message> Messages { get; set; } = new List<Message>();
	}
}
