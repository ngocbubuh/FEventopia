﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class TaskProcessModel
    {
        public string StaffID { get; set; }
        public Guid EventDetailId { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        [Range(0, float.MaxValue)]
        public float PlanCost { get; set; }
        [Range(0, float.MaxValue)]
        public float ActualCost { get; set; }
        public string? Status { get; set; }
    }
}
