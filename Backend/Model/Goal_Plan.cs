using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using retplann.Services.JsonConverters;


namespace retplann.Model
{
    public class Goal_plan
    {
        [Key]
        public int? GId{get; set;}
        [JsonConverter(typeof(StrictIntJsonConverter))]
        public int? PId{get; set;}
        [JsonConverter(typeof(StrictIntJsonConverter))]
        [Range(18, 100, ErrorMessage = "Value must be greater than zero.")]
        public int? CurrentAge{get; set;}
        [JsonConverter(typeof(StrictIntJsonConverter))]
        [Range(19, 101, ErrorMessage = "Value must be greater than zero.")]
        public int? RetirementAge{get; set;}
        public decimal? CurrentSavings{get; set;}
        public decimal? TargetSavings{get; set;}
        public decimal? MonthlyContribution{get; set;}
    }
}
