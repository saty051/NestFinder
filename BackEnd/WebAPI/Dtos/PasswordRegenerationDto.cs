using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class PasswordRegenerationDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string SecurityQuestion { get; set; }

        [Required]
        public string SecurityAnswer { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
