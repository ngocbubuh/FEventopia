using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class FeedBackModel
    {
        public string? Id { get; set; }
        public required Guid EventDetailID { get; set; }
        public required string AccountID { get; set; }
        [Range(1,5,ErrorMessage ="Rate must be in 1 to 5")]
        public int Rate { get; set; }
        [Required(AllowEmptyStrings = true)]
        public required string Description { get; set; }
        
    }
}
