﻿using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Enum;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class EventAssigneeService : IEventAssigneeService
    {
        private readonly IEventAssigneeRepository _eventAssigneeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventAssigneeService(IEventAssigneeRepository eventAssigneeRepository, 
            IUserRepository userRepository, IEventDetailRepository eventDetailRepository,IMapper mapper, IEventRepository eventRepository)
        {
            _eventAssigneeRepository = eventAssigneeRepository;
            _userRepository = userRepository;
            _eventDetailRepository = eventDetailRepository;
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<bool> AddEventAssignee(string accountId, string eventDetailId)
        {
            //lay accountid, kiem tra xem account có ton tai khum
            var account = await _userRepository.GetAccountByIdAsync(accountId);
            if (account == null) { return false; } //chinh lai null

            //lay eventdetail, kiem tra eventdetail có ton tai khum
            var eventdetail = await _eventDetailRepository.GetByIdAsync(eventDetailId);
            if (eventdetail == null) { return false; } //chinh lai null

            //Lay event - Nếu sự kiện đang ở giai đoạn EXECUTION trở đi, ko được add
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (@event.Status.Equals(EventStatus.EXECUTE.ToString()) || @event.Status.Equals(EventStatus.POST.ToString())) return false;

            //tao event assignee - luu db
            var eventAssignee = new EventAssignee
            {
                Id = Guid.NewGuid(),                
                AccountId = account.Id,
                EventDetailId = eventdetail.Id,
                Role = account.Role,
            };
            await _eventAssigneeRepository.AddAsync(eventAssignee);

            return true;
            //var result = await _eventAssigneeRepository.GetByIdAsync(eventAssignee.Id.ToString());
            //if (result != null)
            //{
            //    return true;
            //}
            //else return false;
            //return _mapper.Map<EventAssigneeModel>(result);
        }

        public async Task<bool> DeleteEventAssignee(string eventDetailId, string accountId)
        {
            //lay eventdetail, kiem tra eventdetail có ton tai khum
            var eventdetail = await _eventDetailRepository.GetByIdAsync(eventDetailId);
            if (eventdetail == null) { return false; } //chinh lai null

            //Lay event - Nếu sự kiện đang ở giai đoạn EXECUTION trở đi, ko được xóa
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (@event.Status.Equals(EventStatus.EXECUTE.ToString()) || @event.Status.Equals(EventStatus.POST.ToString())) return false;

            var eventassignee = await _eventAssigneeRepository.GetByED_AC(eventDetailId, accountId);
            return await _eventAssigneeRepository.DeleteAsync(eventassignee);
        }

        public async Task<List<EventAssigneeModel>> GetAllByEventDetailId(string eventdetailid,PageParaModel pageParaModel)
        {
            var eventAssignees = await _eventAssigneeRepository.GetAllByEventDetail(eventdetailid);
            var result = _mapper.Map<List<EventAssigneeModel>>(eventAssignees);
            return PageModel<EventAssigneeModel>.ToPagedList(result, pageParaModel.PageNumber, pageParaModel.PageSize);
        }

        public async Task<EventAssigneeModel> GetById(string id, PageParaModel pageParaModel)
        {
            var eventAssignee = await _eventAssigneeRepository.GetByIdAsync(id);
            return _mapper.Map<EventAssigneeModel>(eventAssignee);
        }
    }
}
