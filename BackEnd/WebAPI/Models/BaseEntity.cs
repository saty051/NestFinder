using System;

namespace WebAPI.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime LastUpdatedOn { get; set; } = DateTime.Now;
        public int LatestUpdatedBy { get; set; }
    }
}
