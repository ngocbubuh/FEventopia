using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventDetailProcessModel
    {
        public required Guid EventID { get; set; }
        public required Guid LocationID { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required int TicketForSaleInventory { get; set; }
        public int? StallForSaleInventory { get; set; } = 0;
        public double? TicketPrice { get; set; } = 0;
    }
}
