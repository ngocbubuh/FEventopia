using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IEventStallService
    {
        public Task<PageModel<EventStallModel>> GetAllEventStall(PageParaModel pageParaModel);
        public Task<List<EventStallModel>> GetAllEventStallCurrentEvent(string eventDetailId);
        public Task<PageModel<EventStallModel>> GetEventStallBySponsorID(string  sponsorID, PageParaModel pageParaModel);
        public Task<EventStallModel> CreateEventStall(string eventDetailId, string username, string stallnumber);
        public Task<PageModel<EventStallModel>> GetAllByStallNumber(string stallnumber, PageParaModel pageParaModel);
        public Task<EventStallModel> GetEventStallById(string stallid);
    }
}
