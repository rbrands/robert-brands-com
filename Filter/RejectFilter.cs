using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;



namespace robert_brands_com.Filter
{
    public class RejectFilter : IAsyncPageFilter
    {
        // List of rejected clients
        private string[] _rejectedClients =
        {
            "Ubuntu/10",
            "Mozilla/5.0 (X11; U; Linux amd64; rv:5.0) Gecko/20100101 Firefox/5.0 (Debian)",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.10 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/11.04 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
            "Mozilla/5.0 (X11; Datanyze; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36",
            "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.17 (KHTML, like Gecko) Chrome/10.0.649.0 Safari/534.17",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/534.30 (KHTML, like Gecko) Ubuntu/10.04 Chromium/12.0.742.112 Chrome/12.0.742.112 Safari/534.30",
            "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Mobile Safari/537.36",
            "Mozilla/5.0 (compatible; adscanner/)",
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2)"
        };
        public async Task OnPageHandlerSelectionAsync(
                                      PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(
                                           PageHandlerExecutingContext context,
                                           PageHandlerExecutionDelegate next)
        {
            // Check user agent
            string userAgent = context.HttpContext.Request.Headers["User-Agent"];
            foreach (string client in _rejectedClients)
            {
                if (userAgent.Contains(client))
                {
                    context.Result = new NotFoundResult();
                    await Task.CompletedTask;
                    break;
                }
            }
            await next.Invoke();

        }
    }
}
