using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventAssigneeDetailModel
    {
        public EventAssigneeEventModel Event { get; set; }
        public ICollection<EventAssigneeModel> EventAssigneeModel { get; set; }
    }
}
