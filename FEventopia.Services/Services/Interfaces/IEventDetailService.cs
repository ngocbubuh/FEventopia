using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IEventDetailService
    {
        public Task<PageModel<EventDetailModel>> GetAllEventDetailByEventIdAsync(string eventId, PageParaModel pagePara);
        public Task<List<EventDetailModel>> GetAllEventDetailByStartDate(DateTime startDate);
        public Task<PageModel<EventDetailModel>> GetAllEventDetailAtLocation(string locationId, DateTime startDate, DateTime endDate, PageParaModel pagePara);
        public Task<EventDetailModel> GetEventDetailByIdAsync(string id);
        public Task<EventDetailOperatorModel> GetEventDetailOperatorByIdAsync(string id);
        public Task<EventDetailOperatorModel> AddEventDetailAsync(EventDetailProcessModel eventDetailModel);
        public Task<EventDetailOperatorModel> UpdateEventDetailAsync(string id, EventDetailProcessModel eventDetailModel);
        public Task<bool> DeleteEventDetailAsync(string id);
    }
}
