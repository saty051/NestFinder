namespace WebAPI.Dtos
{
    public class LikeDto
    {
        public int UserId { get; set; }
        public string PropertyName { get; set; }
        public DateTime LikedOn { get; set; }
        public string UserName { get; set; }
    }
}
