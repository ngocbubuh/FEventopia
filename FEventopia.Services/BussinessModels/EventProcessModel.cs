﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventProcessModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Location name must between 1-50 characters!")]
        public required string EventName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string EventDescription { get; set; }

        [JsonIgnore]
        public string? Category { get; set; }
        public string? Banner { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Initial price must be > 0!")]
        public required double InitialCapital { get; set; }
    }
}
