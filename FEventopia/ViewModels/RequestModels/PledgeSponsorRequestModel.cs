using System.ComponentModel.DataAnnotations;

namespace FEventopia.Controllers.ViewModels.RequestModels
{
    public class PledgeSponsorRequestModel
    {
        public required Guid EventId { get; set; }
        [Range(10000000, double.MaxValue, ErrorMessage = "Amount must be at least 10.000.000!")]
        public required double Amount { get; set; }
    }
}
