namespace ChatApp.Models
{
	public class Message
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public User? User { get; set; } // Navigation property to the User who sent the message
		public bool IsDeleted { get; set; } = false;
	}
}
