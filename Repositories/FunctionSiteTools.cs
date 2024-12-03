using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl;
using robert_brands_com.Models;
using Azure.Storage.Queues;
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
                           .ReceiveJson<List<TranslationResponse>>();
            return response.FirstOrDefault()?.Translations.FirstOrDefault()?.Text ?? text;
        }
        public async Task<dynamic> AnalyzeImage(string imageUrl)
        {
            object body = new { url = imageUrl };

            dynamic response = await $"https://{_functionsConfig.FunctionAppName}.azurewebsites.net/api/AnalyzeImage"
                            .WithHeader("x-functions-key", _functionsConfig.AnalyzeImageFunctionKey)
                           .PostJsonAsync(body)
                           .ReceiveJson<ImageAnalysisResult>();
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
    public class ImageAnalysisResult
    {
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }

    public class TranslationResponse
    {
        public List<Translation> Translations { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
    }
}
