using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class SponsorManagementModel
    {
        public required string Id { get; set; }
        public required Guid EventId { get; set; }
        public required string SponsorId { get; set; }
        public double PledgeAmount { get; set; }
        public double ActualAmount { get; set; }
        public string? Rank { get; private set; }
        public required string Status { get; set; }

        public virtual Event? Event { get; }
    }
}
