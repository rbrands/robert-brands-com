using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using robert_brands_com.Models;
using Newtonsoft.Json;

namespace robert_brands_com.Repositories
{
        /// <summary>
        /// Interface to use the functions of FunctionSiteTools from Azure Function App
        /// </summary>
    public interface IFunctionSiteTools
    {
        Task<string> Translate(string language, string text);
        Task LogToDb(ActivityLogItem logItem);
        Task SendMail(string email, string subject, string htmlText);
        Task<dynamic> AnalyzeImage(string imageUrl);

    }

    public class EMail
    {
        [JsonProperty(PropertyName = "from", NullValueHandling = NullValueHandling.Ignore)]
        public string From { get; set; }
        [JsonProperty(PropertyName = "fromName", NullValueHandling = NullValueHandling.Ignore)]
        public string FromName { get; set; }
        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "subject", NullValueHandling = NullValueHandling.Ignore)]
        public string Subject { get; set; }
        [JsonProperty(PropertyName = "htmlText", NullValueHandling = NullValueHandling.Ignore)]
        public string HtmlText { get; set; }
    }

}
