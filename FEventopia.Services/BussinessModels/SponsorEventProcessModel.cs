using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class SponsorEventProcessModel
    {
        public required string EventId { get; set; }
        public double Amount { get; set; }
    }
}
