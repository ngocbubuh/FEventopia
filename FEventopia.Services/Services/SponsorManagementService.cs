﻿using AutoMapper;
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
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SponsorManagementService(ISponsorManagementRepository sponsorManagementRepository, IMapper mapper, IUserRepository userRepository, IEventRepository eventRepository) 
        {
            _sponsorManagementRepository = sponsorManagementRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public async Task<SponsorManagementModel> AddSponsorManagementAsync(Guid eventId, double amount, string username)
        {
            var account = await _userRepository.GetAccountByUsernameAsync(username);
            
            //Nếu sự kiện chưa mở tài trợ hoặc đã qua tài trợ => Ko cho
            var @event = await _eventRepository.GetByIdAsync(eventId.ToString());
            if (!@event.Status.Equals(EventStatus.FUNDRAISING.ToString())) { return null; }

            var sponsorManagement = new SponsorManagement(eventId, account.Id, amount, SponsorsManagementStatus.PENDING.ToString())
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                SponsorId = account.Id,
                Status = SponsorsManagementStatus.PENDING.ToString()
            };
            await _sponsorManagementRepository.AddAsync(sponsorManagement);
            var result = await _sponsorManagementRepository.GetSponsorManagementDetailById(sponsorManagement.Id.ToString());
            return _mapper.Map<SponsorManagementModel>(result);
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

        public async Task<PageModel<SponsorManagementModel>> GetAllSponsorManagementWithDetailCurrentUser(string username, PageParaModel pageParaModel)
        {
            var user = await _userRepository.GetAccountByUsernameAsync(username);
            var sponsorManagements = await _sponsorManagementRepository.GetAllSponsorManagementWithDetailCurrentUser(user.Id);
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
