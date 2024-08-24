using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int LatestUpdatedBy { get; set; }
    }
}
