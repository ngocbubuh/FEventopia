﻿using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface IEventAssigneeDAO : IGenericDAO<EventAssignee>
    {
        public Task<List<EventAssignee>> GetAllEventAssigneeDetailByAccountId(string accountId);
    }
}
