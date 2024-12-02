using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl;
using robert_brands_com.Models;
using Azure.Storage;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;

namespace robert_brands_com.Repositories
{
    public class FunctionSiteTools : IFunctionSiteTools
    {
        private FunctionSiteToolsConfig _functionsConfig;

        public FunctionSiteTools(FunctionSiteToolsConfig functionsConfig)
        {
            _functionsConfig = functionsConfig;
        }
        public async Task LogToDb(ActivityLogItem logItem)
        {
            // Create the queue client and queue.
            QueueClient queue = new QueueClient(_functionsConfig.StorageConnectionString, "logging");
            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();
            // Send message to the queue.
            string serializedLogActivity = JsonConvert.SerializeObject(logItem);
            await queue.SendMessageAsync(serializedLogActivity);
            return;
        }
        public async Task SendMail(string email, string subject, string htmlText)
        {
            EMail mailMessage = new EMail();
            mailMessage.Email = email;
            mailMessage.Subject = subject;
            mailMessage.HtmlText = htmlText;
            mailMessage.From = _functionsConfig.From;
            mailMessage.FromName = _functionsConfig.FromName;

            // Create the queue client and queue.
            QueueClient queue = new QueueClient(_functionsConfig.StorageConnectionString, "sendmail");
            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();
            // Send message to the queue.
            string serializedEmail = JsonConvert.SerializeObject(mailMessage);
            await queue.SendMessageAsync(serializedEmail);

            return;
        }
        public async Task<string> Translate(string language, string text)
        {
            object body = new { text = text };

            string[] supportedLanguages = { "en", "fr", "es", "it", "pt" };
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(language) || !supportedLanguages.Contains(language))
            {
                return text;
            }

            var response = await $"https://{_functionsConfig.FunctionAppName}.azurewebsites.net/api/TranslateText"
                            .WithHeader("x-functions-key", _functionsConfig.TranslateFunctionKey)
                           .SetQueryParam("to", language)
                           .PostJsonAsync(body)
                           .ReceiveJson<List<dynamic>>();
            dynamic translated = response[0];
            return translated.translations[0].text;
        }
        public async Task<dynamic> AnalyzeImage(string imageUrl)
        {
            object body = new { url = imageUrl };

            dynamic response = await $"https://{_functionsConfig.FunctionAppName}.azurewebsites.net/api/AnalyzeImage"
                            .WithHeader("x-functions-key", _functionsConfig.AnalyzeImageFunctionKey)
                           .PostJsonAsync(body)
                           .ReceiveJson<dynamic>();
            return response;

        }

    }

    public class FunctionSiteToolsConfig
    {
        public string StorageConnectionString { get; set; }
        public string FunctionAppName { get; set; }
        public string TranslateFunctionKey { get; set; }
        public string AnalyzeImageFunctionKey { get; set; }
        public string DbTenant { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
    }

}
