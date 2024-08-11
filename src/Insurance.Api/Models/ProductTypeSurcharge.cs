using System;

namespace Insurance.Api.Models
{
    public class ProductTypeSurcharge
    {
        public int Id { get; set; }
        public int ProductTypeId { get; set; }
        public decimal SurchargeRate { get; set; }
        public Guid Version { get; set; }
    }
}
