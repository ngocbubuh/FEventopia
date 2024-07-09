using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IEventAssigneeService
    {
        public Task<EventAssigneeModel> GetById (string id, PageParaModel pageParaModel);
        public Task<PageModel<EventAssigneeModel>> GetAllByEventDetailId(string eventdetailid,PageParaModel pageParaModel);
        public Task<PageModel<EventAssigneeModel>> GetAllByAccountId(string username, PageParaModel pageParaModel);
        public Task<bool> AddEventAssignee(string accountId, string eventDetailId);
        public Task<bool> DeleteEventAssignee(string eventDetailId, string accountId);
    }
}
