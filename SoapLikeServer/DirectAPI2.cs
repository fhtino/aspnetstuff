using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace SoapLikeServer
{
    public class DirectAPI2 : HttpTaskAsyncHandler, IRouteHandler
    {

        public override bool IsReusable => false;

        public IHttpHandler GetHttpHandler(RequestContext requestContext) { return this; }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            await Task.CompletedTask;
            context.Response.ContentType = "text/plain";
            context.Response.Write($"DirectAPI_2: Hello world. {DateTime.UtcNow.ToString("O")}\n\n");
            context.Response.StatusCode = 200;
        }

    }
}