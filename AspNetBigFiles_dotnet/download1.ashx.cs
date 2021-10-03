using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Web1
{

    public class download1 : IHttpHandler
    {
        public bool IsReusable { get { return false; } }


        public void ProcessRequest(HttpContext context)
        {
            string fileName = "file.txt";

            context.Response.Buffer = false;
            context.Response.ContentType = "application/octet-stream";
            context.Response.Headers.Add("Content-Disposition", $"attachment;filename=\"{fileName}\"");

            for (int i = 0; i < 1024; i++)
            {
                context.Response.Write(new String('x', 1024 * 1024));
                Thread.Sleep(1000);   // just to slow down....
            }
        }
        
    }
}