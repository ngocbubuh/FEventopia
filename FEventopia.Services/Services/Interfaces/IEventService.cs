using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IEventService
    {
        public Task<PageModel<EventModel>> GetAllEventAsync(PageParaModel pageParaModel); //Get all event for home page
        public Task<EventModel> GetEventByIdAsync(string id);
        public Task<EventOperatorModel> AddEventAsync(EventProcessModel eventProcessModel);
        public Task<EventOperatorModel> UpdateEventAsync(string id, EventProcessModel eventProcessModel);
        public Task<bool> CancelEventAsync(string id);
        public Task<EventOperatorModel> GetEventByIdOperatorAsync(string id);
        public Task<bool> UpdateEventNextPhaseAsync(string id);
        public Task<bool> DeleteEventAsync(string id);
    }
}
