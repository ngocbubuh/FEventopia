﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class LocationProcessModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Location name must between 1-50 characters!")]
        public required string LocationName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string LocationDescription { get; set; }
        [Range(0, int.MaxValue)]
        public required int Capacity { get; set; }
    }
}
