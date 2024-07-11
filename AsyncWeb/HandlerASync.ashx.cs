using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace AsyncWeb
{

    public class HandlerASync : HttpTaskAsyncHandler
    {


        public override async Task ProcessRequestAsync(HttpContext context)
        {
            var sleep = new Random().Next(MyConsts.SleepMin, MyConsts.SleepMax);
            await Task.Delay(sleep);
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World ASYNC " + sleep);
        }

    }

}