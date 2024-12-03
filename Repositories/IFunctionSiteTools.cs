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
        Task<dynamic> AnalyzeImage(string imageUrl);

    }


}
