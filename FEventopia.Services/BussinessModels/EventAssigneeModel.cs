using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventAssigneeModel
    {
        public Guid Id { get; set; }
        public required string AccountId { get; set; }
        public required Guid EventDetailId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string Role {get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
