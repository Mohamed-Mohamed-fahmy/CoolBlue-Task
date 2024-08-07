﻿using Newtonsoft.Json;

namespace Insurance.Api.Models
{
    public class ProductTypeDto
    {
        [JsonProperty("id")]
        public int ProductTypeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("CanBeInsured")]
        public bool CanBeInsured { get; set; }
    }
}
