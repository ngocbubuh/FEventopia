using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class SponsorManagementService : ISponsorManagementService
    {
        private readonly ISponsorManagementRepository _sponsorManagementRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SponsorManagementService(ISponsorManagementRepository sponsorManagementRepository, IMapper mapper, IUserRepository userRepository) 
        {
            _sponsorManagementRepository = sponsorManagementRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<SponsorManagementModel> AddSponsorManagementAsync(Guid eventId, double amount, string username)
        {
            var account = await _userRepository.GetAccountByUsernameAsync(username);
            var sponsorManagement = new SponsorManagement(eventId, account.Id, amount, SponsorsManagementStatus.PENDING.ToString())
            {
                EventId = eventId,
                SponsorId = account.Id,
                Status = SponsorsManagementStatus.PENDING.ToString()
            };
            await _sponsorManagementRepository.UpdateAsync(sponsorManagement);
            return _mapper.Map<SponsorManagementModel>(sponsorManagement);
        }

        public async Task<PageModel<SponsorManagementModel>> GetAllSponsorManagementWithDetailAsync(PageParaModel pageParaModel)
        {
            var sponsorManagements = await _sponsorManagementRepository.GetAllSponsorManagementDetail();
            var result = _mapper.Map<List<SponsorManagementModel>>(sponsorManagements);
            return PageModel<SponsorManagementModel>.ToPagedList(result, 
                pageParaModel.PageNumber, 
                pageParaModel.PageSize);
        }

        public async Task<PageModel<SponsorManagementModel>> GetAllSponsorManagementWithDetailCurrentEvent(string eventId, PageParaModel pageParaModel)
        {
            var sponsorManagements = await _sponsorManagementRepository.GetAllSponsorManagementWithDetailCurrentEvent(eventId);
            var result = _mapper.Map<List<SponsorManagementModel>>(sponsorManagements);
            return PageModel<SponsorManagementModel>.ToPagedList(result,
                pageParaModel.PageNumber,
                pageParaModel.PageSize);
        }

        public async Task<PageModel<SponsorManagementModel>> GetAllSponsorManagementWithDetailCurrentUser(string userId, PageParaModel pageParaModel)
        {
            var sponsorManagements = await _sponsorManagementRepository.GetAllSponsorManagementWithDetailCurrentUser(userId);
            var result = _mapper.Map<List<SponsorManagementModel>>(sponsorManagements);
            return PageModel<SponsorManagementModel>.ToPagedList(result,
                pageParaModel.PageNumber,
                pageParaModel.PageSize);
        }

        public async Task<SponsorManagementModel?> GetSponsorManagementDetailById(string sponsorManagementId)
        {
            var result = await _sponsorManagementRepository.GetSponsorManagementDetailById(sponsorManagementId);
            return _mapper.Map<SponsorManagementModel>(result);
        }

        public async Task<SponsorManagementModel> GetSponsorManagementDetailByPrimaryKey(string eventId, string userId)
        {
            var result = await _sponsorManagementRepository.GetSponsorManagementDetailByPrimaryKey(eventId, userId);
            return _mapper.Map<SponsorManagementModel>(result);
        }
    }
}
