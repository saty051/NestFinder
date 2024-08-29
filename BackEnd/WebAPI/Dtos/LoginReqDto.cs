using Microsoft.AspNetCore.Server.HttpSys;

namespace WebAPI.Dtos
{
    public class LoginReqDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
