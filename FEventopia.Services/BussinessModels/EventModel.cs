using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventModel
    {
        public string? Id { get; set; }
        public required string EventName { get; set; }
        public required string EventDescription { get; set; }
        public required string Category { get; set; }
        public string? Banner { get; set; }
    }
}
