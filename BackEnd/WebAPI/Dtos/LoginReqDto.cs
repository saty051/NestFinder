namespace WebAPI.Dtos
{
    public class LoginReqDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }  // New Field for Email
        public string PhoneNumber { get; set; }  // New Field for Phone Number
        public string TelegramId { get; set; }  // New Field for Telegram ID
    }
}
