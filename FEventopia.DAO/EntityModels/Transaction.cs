using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class Transaction : EntityBase
    {
        public required string AccountID { get; set; }
        public required string TransactionType { get; set; } //In, OUT
        public required double Amount { get; set; }
        public required string Description { get; set; }
        public required DateTime TransactionDate { get; set; }
        public required bool Status { get; set; }
        public required virtual Account Account { get; set; } //FK AccountID
        public virtual Ticket? Ticket { get; set; } //FK 1 to 1
        public virtual EventStall? EventStall { get; set; }
        public virtual SponsorEvent? SponsorEvent { get; set; }
    }
}
