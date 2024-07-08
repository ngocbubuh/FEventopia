using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly IEventDAO _eventDAO;
        public EventRepository(IGenericDAO<Event> genericDAO, IEventDAO eventDAO) : base(genericDAO)
        {
            _eventDAO = eventDAO;
        }
        
        private static string UnsignName(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = Regex.Replace(text, "[^a-zA-Z0-9 ]", " ");

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC)
                .ToLowerInvariant();
        }
        private static IEnumerable<Event> SearchData(IEnumerable<Event> data, string keyword)
        {
            string normalizedKeyword = UnsignName(keyword);

            return data.Where(item => UnsignName(item.EventName).Contains(normalizedKeyword));
        }


        public async Task<Event?> GetEventWithDetailByIdAsync(string id)
        {
            return await _eventDAO.GetEventWithDetailByIdAsync(id);
        }

        public async Task<List<Event>> SearchEventByName(string name)
        {
            var events = await _eventDAO.GetAllAsync();
            var resuls = SearchData(events, name);
            return resuls.ToList();
        }

        public async Task<Event> GetEventByName(string nameSAMPLE)
        {
            var events = await _eventDAO.GetAllAsync();
            string name = UnsignName(nameSAMPLE);
            return events.Where(e => e.EventName.Equals(name)).SingleOrDefault();
        }

        public async Task<List<Event>> GetAllEventForVisitorAsync()
        {
            var eventLists = await _eventDAO.GetAllAsync();
            return eventLists.Where(e => e.Status.ToUpper().Equals("EXECUTE") || e.Status.ToUpper().Equals("POST")).ToList();
        }
    }
}
