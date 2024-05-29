using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class SponsorEvent : EntityBase
    {
        public required string SponsorID { get; set; }
        public required string EventID { get; set; }
        public required string TransactionID { get; set; }
        public required virtual Account Account { get; set; } //FK SponsorID
        public required virtual Transaction Transaction { get; set; } //FK TransactionID
        public required virtual Event Event { get; set; }
    }
}
