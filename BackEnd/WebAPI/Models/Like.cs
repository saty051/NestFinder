namespace WebAPI.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public User User { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public DateTime LikedOn { get; set; } = DateTime.UtcNow;
    }
}
