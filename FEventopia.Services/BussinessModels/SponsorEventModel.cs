using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class SponsorEventModel
    {
        public required string Id { get; set; }
        public required string SponsorID { get; set; }
        public required string EventID { get; set; }
        public required string TransactionID { get; set; }

        public Transaction? Transaction { get; }
        public Event? Event { get; }
    }
}
