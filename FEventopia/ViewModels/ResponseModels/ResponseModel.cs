using System.Reflection;
using System.Web;

namespace FEventopia.Controllers.ViewModels.ResponseModels
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public required string Message { get; set; }
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
