using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class EventStall : EntityBase
    {
        public required string SponsorID { get; set; }
        public required Guid EventDetailID { get; set; }
        public required Guid TransactionID { get; set; }
        public required string StallNumber { get; set; }
        public virtual Account? Account { get; } //FK SponsorID
        public virtual Transaction? Transaction { get; } //FK TransactionID
        public virtual EventDetail? EventDetail { get; }
    }
}
