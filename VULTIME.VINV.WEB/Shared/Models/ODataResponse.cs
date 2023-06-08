﻿using System.Text.Json.Serialization;

namespace VULTIME.VINV.WEB.Shared.Models
{
    public class ODataResponse<T>
    {
        [JsonPropertyName("@odata.context")]
        public string? Context { get; set; }

        [JsonPropertyName("@odata.count")]
        public int? Count { get; set; }

        public string? NextLink { get; set; }
        public string? DeltaLink { get; set; }
        public List<T>? Value { get; set; }
    }
}