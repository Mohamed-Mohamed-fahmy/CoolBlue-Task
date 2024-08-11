using System;

namespace Insurance.Api.DTOs
{
    public class SurchargeRateDto
    {
        public int SurchargeRate { get; set; }
        public int ProductTypeId { get; set; }
        public Guid Version { get; set; }
    }
}
