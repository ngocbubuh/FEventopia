using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class EventDetail : EntityBase
    {
        public required Guid EventID { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTime { get; set; }
        public required Guid LocationID { get; set; }
        public int TicketForSaleInventory { get; set; }
        public double TicketPrice { get; set; }
        public double EstimateCost { get; set; }

        public required virtual Location Location { get; set; }
        public required virtual Event Event { get; set; }
        public virtual Ticket? Ticket { get; }
        public virtual EventStall? EventStall { get; }
        public virtual ICollection<Task>? Task { get; set; }
        public virtual ICollection<Cost>? Cost { get; set; }
    }
}
