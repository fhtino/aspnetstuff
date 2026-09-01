using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace SoapLikeServer
{
    public class DirectAPI1 : HttpTaskAsyncHandler
    {

        public override bool IsReusable => false;

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            await Task.CompletedTask;
            context.Response.ContentType = "text/plain";
            context.Response.Write($"DirectAPI_1: Hello world. {DateTime.UtcNow.ToString("O")}\n\n");
            context.Response.StatusCode = 200;
        }

    }
}