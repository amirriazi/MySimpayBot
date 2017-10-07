using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.WebService
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // log request body
            string requestBody = await request.Content.ReadAsStringAsync();


            var requestUri = Guid.NewGuid();
            Def.MyDbLogger = new MyDbLoggerModel();

            Def.MyDbLogger.logging = true;
            Def.MyDbLogger.action = "request";
            Def.MyDbLogger.requestUid = requestUri.ToString();
            Def.MyDbLogger.UserUID = "";
            Def.MyDbLogger.requestUri = request.RequestUri.LocalPath;
            Def.MyDbLogger.playLoad = requestBody;
            Def.MyDbLogger.reportLog();






            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);
            Def.MyDbLogger.action = "respond";

            if (result.Content != null)
            {
                // once response body is ready, log it
                var responseBody = await result.Content.ReadAsStringAsync();
                //Log.Trace($"responseBody({requestUri.ToString()})=" + responseBody, 0);
                Def.MyDbLogger.playLoad = responseBody;
                Def.MyDbLogger.reportLog();

            }
            else
            {
                //Log.Trace($"responseBody({requestUri.ToString()})= NULL", 0);
                Def.MyDbLogger.playLoad = "empty";
                Def.MyDbLogger.reportLog();

            }

            return result;
        }
    }

}
