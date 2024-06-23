using AutoMapper;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.DAO.EntityModels;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEventopia.Repositories.Repositories;

namespace FEventopia.Services.Services
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IMapper _mapper;

        public FeedBackService(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }

        public async Task<FeedBackModel> CreateFeedBack(FeedBackModel feedBackModel)
        {
            var feedback = _mapper.Map<Feedback>(feedBackModel);
            var result = await _feedBackRepository.AddAsync(feedback);
            return _mapper.Map<FeedBackModel>(result);
        }

        public async Task<bool> DeleteFeedBack(string Id)
        {
            var feedback = await _feedBackRepository.GetByIdAsync(Id);
            return await _feedBackRepository.DeleteAsync(feedback);
        }

        public async Task<PageModel<FeedBackModel>> GetAllByEventDetail(string eventDetailId, PageParaModel pageparamodel)
        {
            var feedbacks = await _feedBackRepository.GetAllByEventDetail(eventDetailId);
            var result = _mapper.Map<List<FeedBackModel>>(feedbacks);
            return PageModel<FeedBackModel>.ToPagedList(result, pageparamodel.PageNumber, pageparamodel.PageSize);
        }

        public async Task<FeedBackModel> GetFeedBackByID(string id)
        {
            var feedback = await _feedBackRepository.GetByIdAsync(id);
            return _mapper.Map<FeedBackModel>(feedback);
        }

        public async Task<bool> UpdateFeedBack(string Id, FeedBackModel feedBackModel)
        {
            var feedback = await _feedBackRepository.GetByIdAsync(Id);
            if(feedback == null)
            {
                return false;
            }
            var result = _mapper.Map(feedBackModel, feedback);
            return await _feedBackRepository.UpdateAsync(result);
        }
    }
}
