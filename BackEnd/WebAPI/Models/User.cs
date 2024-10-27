using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public byte[] Password { get; set; }

        public byte[] PasswordKey { get; set; }

        public string Email { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Phone number must be 10 digits")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits and contain only numbers")]
        public string PhoneNumber { get; set; }

        public ICollection<Like> Likes { get; set; }

        [NotMapped]
        public int LatestUpdatedBy { get; set; }
    }
}
