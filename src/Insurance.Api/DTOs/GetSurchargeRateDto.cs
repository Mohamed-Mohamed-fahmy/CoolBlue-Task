namespace Insurance.Api.DTOs
{
    public class GetSurchargeRateDto : BaseResponse
    {
        public int SurchargeRate { get; set; }
        public int ProductTypeId { get; set; }
    }
}
