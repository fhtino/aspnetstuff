using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AsyncWeb
{

    public class HandlerSync : IHttpHandler
    {

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            var sleep = new Random().Next(MyConsts.SleepMin, MyConsts.SleepMax);
            Thread.Sleep(sleep);
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World " + sleep);
        }

    }
}