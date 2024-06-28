using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class AnalysisModel
    {
        public int NumTicketSold { get; set; }
        public int NumTicketCheckedIn { get; set; }
        public double TicketIncome { get; set; }
        public int? NumStallSold { get; set; }
        public double? StallIncome { get; set; }
        public double InitialCapital { get; set; }
        public double? SponsorCaptital { get; set; }
        public double ActualExpense { get; set; }
        public double AverageFeedback { get; set; }
    }
}
