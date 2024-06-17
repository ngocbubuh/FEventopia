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
        public Guid EventID { get; set; }
        public Guid LocationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required int TicketForSaleInventory { get; set; }
        public int? StallForSaleInventory { get; set; } = 0;
        public double TicketPrice { get; set; } = 0;
        public double EstimateCost { get; set; }

        public virtual Location Location { get; set; }
        public virtual Event Event { get; }
        public virtual ICollection<Ticket>? Ticket { get; }
        public virtual ICollection<EventStall>? EventStall { get; }
        public virtual ICollection<Task>? Task { get; }
        public virtual ICollection<Feedback>? Feedbacks { get; }
        public virtual ICollection<EventAssignee>? EventAssignee  { get;}
    }
}
