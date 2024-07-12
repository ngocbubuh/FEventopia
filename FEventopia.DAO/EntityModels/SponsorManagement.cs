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
        public double PledgeAmount { get; set; }
        public double ActualAmount { get; set; }
        public string Rank { get; private set; }
        public required string Status { get; set; }

        public virtual Account? Account { get; }
        public virtual Event? Event { get; }

        public SponsorManagement(Guid eventId, string sponsorId, double pledgeAmount, string status = "")
        {
            EventId = eventId;
            SponsorId = sponsorId;
            PledgeAmount = pledgeAmount;
            Status = status;
            Rank = SetRank(pledgeAmount); // Call set_rank during initialization
        }

        private string SetRank(double amount)
        {
            if (amount >= 50000000)
            {
                return SponsorRank.PLATINUM.ToString();
            }
            else if (amount >= 30000000)
            {
                return SponsorRank.GOLD.ToString();
            }
            else if (amount >= 15000000)
            {
                return SponsorRank.SILVER.ToString();
            }
            else if (amount > 0)
            {
                return SponsorRank.BRONZE.ToString();
            }
            else
            {
                return SponsorRank.FAILEDCREDIT.ToString(); // Default rank for amounts below thresholds
            }
        }

        public void SetAmount(double newAmount)
        {
            PledgeAmount = newAmount;
            Rank = SetRank(PledgeAmount);
        }
    }

    public enum SponsorRank
    {
        PLATINUM,
        GOLD,
        SILVER,
        BRONZE,
        FAILEDCREDIT
    }
}
