using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class TicketModel
    {
        public required string Id { get; set; }
        public required string EventDetailID { get; set; }
        public required string VisitorID { get; set; }
        public bool CheckInStatus { get; set; }

        public TransactionModel? Transaction { get; set; }
        public EventDetailModel? EventDetail { get; set; }
        public EventTicketModel? Event { get; set; }
    }
}
