using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class SponsorEventProcessModel
    {
        public required Guid EventId { get; set; }
        [Range(10000, double.MaxValue, ErrorMessage = "Sponsor amount must >= 10000")]
        public double Amount { get; set; }
    }
}
