using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FEventopia.Services.BussinessModels
{
    public class TransactionModel
    {
        public required Guid Id { get; set; }
        public required string AccountID { get; set; }
        public required string TransactionType { get; set; } //In, OUT
        public required double Amount { get; set; }
        public required string Description { get; set; }
        public required DateTime TransactionDate { get; set; }
        public bool Status { get; set; }

        public string ToUrlParameters()
        {
            var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var queryString = new List<string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                if (value != null)
                {
                    queryString.Add($"{property.Name}={HttpUtility.UrlEncode(value.ToString())}");
                }
            }

            return string.Join("&", queryString);
        }
    }
}
