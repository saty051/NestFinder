using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Dtos
{
    public class CityDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
