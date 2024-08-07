using Newtonsoft.Json;

namespace Insurance.Api.Models
{
    public class ProductDto
    {
        [JsonProperty("id")]
        public int ProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("salesPrice")]
        public double SalesPrice { get; set; }

        [JsonProperty("productTypeId")]
        public int ProductTypeId { get; set; }

        [JsonIgnore]
        public ProductTypeDto ProductType { get; set; }
    }
}
