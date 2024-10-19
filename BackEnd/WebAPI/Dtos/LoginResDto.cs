namespace WebAPI.Dtos
{
    public class LoginResDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }  // New Field for Email
        public string PhoneNumber { get; set; }  // New Field for Phone Number
        public string TelegramId { get; set; }  // New Field for Telegram ID
    }
}
