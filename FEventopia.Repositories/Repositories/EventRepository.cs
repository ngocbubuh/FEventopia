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
using FuzzySharp;
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

            var normalizedString = text.Normalize(NormalizationForm.FormC).ToLower();
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                stringBuilder.Append(VietNameseAccentUtils.GetUnaccentedCharacter(c));
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormKD)
                .ToLowerInvariant();
        }

        private static IEnumerable<Event> SearchData(IEnumerable<Event> data, string keyword)
        {
            return data.Where(e => Fuzz.PartialRatio(UnsignName(keyword), UnsignName(e.EventName)) >= 60)
                .OrderByDescending(e => Fuzz.PartialRatio(UnsignName(keyword), UnsignName(e.EventName)));
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

    public class VietNameseAccentUtils
    {
        private static readonly Dictionary<char, char> AccentMap = new Dictionary<char, char>()
        {
            {'à', 'a'}, {'á', 'a'}, {'ả', 'a'}, {'ã', 'a'}, {'ạ', 'a'},
            {'è', 'e'}, {'é', 'e'}, {'ẻ', 'e'}, {'ẽ', 'e'}, {'ẹ', 'e'},
            {'ì', 'i'}, {'í', 'i'}, {'ỉ', 'i'}, {'ĩ', 'i'}, {'ị', 'i'},
            {'ò', 'o'}, {'ó', 'o'}, {'ỏ', 'o'}, {'õ', 'o'}, {'ọ', 'o'},
            {'ù', 'u'}, {'ú', 'u'}, {'ủ', 'u'}, {'ũ', 'u'}, {'ụ', 'u'},
            {'đ', 'd'},

            {'â', 'a'}, {'ă', 'a'},
            {'ê', 'e'}, {'ô', 'o'},
            {'ư', 'u'}, {'ơ', 'o'},
            {'ỳ', 'y'}, {'ý', 'y'}, {'ỷ', 'y'}, {'ỹ', 'y'}, {'ỵ', 'y'},

            {'ấ', 'a'}, {'ầ', 'a'}, {'ẩ', 'a'}, {'ẫ', 'a'}, {'ậ', 'a'},
            {'ắ', 'a'}, {'ằ', 'a'}, {'ẳ', 'a'}, {'ẵ', 'a'}, {'ặ', 'a'},
            {'ế', 'e'}, {'ề', 'e'}, {'ể', 'e'}, {'ễ', 'e'}, {'ệ', 'e'},
            {'ố', 'o'}, {'ồ', 'o'}, {'ổ', 'o'}, {'ỗ', 'o'}, {'ộ', 'o'},
            {'ớ', 'o'}, {'ờ', 'o'}, {'ở', 'o'}, {'ỡ', 'o'}, {'ợ', 'o'},
            {'ứ', 'u'}, {'ừ', 'u'}, {'ử', 'u'}, {'ữ', 'u'}, {'ự', 'u'},
        };

        public static char GetUnaccentedCharacter(char @char)
        {
            char lowerChar = Char.ToLowerInvariant(@char);
            if (AccentMap.ContainsKey(lowerChar))
            {
                return AccentMap[lowerChar];
            }
            else
            {
                return @char;
            }
        }
    }
}
