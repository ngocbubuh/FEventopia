using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventOperatorModel
    {
        public string? Id { get; set; }
        public required string EventName { get; set; }
        public required string EventDescription { get; set; }
        public required string Category { get; set; }
        public string? Banner { get; set; }
        public required double InitialCapital { get; set; }
        public double? SponsorCapital { get; set; } = 0; //No manually update
        public double? TicketSaleIncome { get; set; } = 0; //No manually update
        public required string Status { get; set; }

        public ICollection<EventDetailOperatorModel>? EventDetail { get; set; }
    }
}
