using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventStallModel 
    {
        public required Guid Id { get; set; }
        public required string SponsorID { get; set; }
        public required Guid EventDetailID { get; set; }
        public required string StallNumber { get; set; }

        public TransactionModel? Transaction { get; set; }
        public EventDetailModel? EventDetail { get; set; }
        public EventTicketModel? Event { get; set; }
    }
}
