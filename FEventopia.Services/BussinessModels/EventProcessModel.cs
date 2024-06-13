using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventProcessModel
    {
        public required string EventName { get; set; }
        public required string EventDescription { get; set; }

        [JsonIgnore]
        public string? Category { get; set; }
        public string? Banner { get; set; }
        public required double InitialCapital { get; set; }
    }
}
