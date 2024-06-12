using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class SponsorManagement : EntityBase
    {
        public required Guid EventId { get; set; }
        public required string SponsorId { get; set; }
        public double Amount { get; private set; }
        public string Rank { get; private set; }
        public required string Status { get; set; }

        public virtual Account? Account { get; }
        public virtual Event? Event { get; }

        public SponsorManagement(Guid eventId, string sponsorId, double amount, string status = "")
        {
            EventId = eventId;
            SponsorId = sponsorId;
            Amount = amount;
            Status = status;
            Rank = SetRank(amount); // Call set_rank during initialization
        }

        private string SetRank(double amount)
        {
            if (amount >= 10000000)
            {
                return "Platinum";
            }
            else if (amount >= 5000000)
            {
                return "Gold";
            }
            else if (amount >= 2000000)
            {
                return "Silver";
            }
            else
            {
                return "Bronze"; // Default rank for amounts below thresholds
            }
        }

        public void SetAmount(double newAmount)
        {
            Amount = newAmount;
            Rank = SetRank(Amount);
        }
    }
}
