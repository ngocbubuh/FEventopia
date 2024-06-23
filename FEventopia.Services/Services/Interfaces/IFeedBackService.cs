using FEventopia.DAO.EntityModels;
using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IFeedBackService
    {
        public Task<PageModel<FeedBackModel>> GetAllByEventDetail(string eventDetailId, PageParaModel pageparamodel);
        public Task<FeedBackModel> GetFeedBackByID(string id);
        public Task<FeedBackModel> CreateFeedBack(FeedBackModel feedBackModel);
        public Task<bool> UpdateFeedBack(string Id,FeedBackModel feedBackModel);
        public Task<bool> DeleteFeedBack(string Id);
    }
}
