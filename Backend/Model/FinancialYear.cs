using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using retplann.Services.JsonConverters;

namespace retplann.Model
{
    public class FinancialYear
    {
        [Key]
        public int? FId{get; set;}
        [JsonConverter(typeof(StrictIntJsonConverter))]
        public int? GId{get; set;}
        [JsonConverter(typeof(StrictIntJsonConverter))]
        public int? Year{get; set;}
        [JsonConverter(typeof(StrictIntJsonConverter))]
        [Range(1, 12, ErrorMessage = "MonthNumber must be between 1 and 12.")]
        public int? MonthNumber {get; set;}
        public decimal? MonthlyInvestment{get; set;}
    }
}
