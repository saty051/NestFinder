using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;

namespace WebAPI.Models
{
    public class Property: BaseEntity
    {
        public string Name { get; set; }

        public string PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
         
        public decimal Price { get; set; }
        public int BHK { get; set; }
        public string Address { get; set; }
        public string? Address2 { get; set; }

        public string FurnishingTypeId { get; set; }
        public FurnishingType FurnishingType { get; set; }

        public int? SellRent { get; set; }
        public int BuiltArea { get; set; }
        public int? CarpetArea { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int FloorNo { get; set; }
        public int FotalFloors { get; set; }
        public bool ReadyToMove { get; set; }
        public string MainEntrance { get; set; }
        public int Security { get; set; }
        public bool Gated { get; set; }
        public int Maintenance { get; set; }
        public DateTime EstPossessionOn { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }

        public ICollection<Photo> Photos { get; set; }
        public DateTime PostedOn { get; set; }

        public int PostedBy { get; set; }
        public User User { get; set; }
    }
}
