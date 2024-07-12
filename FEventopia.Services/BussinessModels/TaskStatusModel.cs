using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class TaskStatusModel
    {
        public Enum.TaskStatus Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Actual Cost must be >= 0")]
        public double? ActualCost { get; set; }
    }
}
