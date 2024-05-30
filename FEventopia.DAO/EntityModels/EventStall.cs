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
        public required virtual Account Account { get; set; } //FK SponsorID
        public required virtual Transaction Transaction { get; set; } //FK TransactionID
        public virtual required EventDetail EventDetail { get; set; }
    }
}
