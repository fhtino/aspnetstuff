using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Web1
{
    /// <summary>
    /// Summary description for uploadHandler
    /// </summary>
    public class uploadHandler : IHttpHandler
    {

        public bool IsReusable { get { return false; } }



        public void ProcessRequest(HttpContext context)
        {
            string uploadguid = context.Request["uploadguid"];
            long position = long.Parse(context.Request["position"]);

            // appent to file
            string fileName = Path.Combine(@"C:\temp\upload", Path.GetFileName(uploadguid));

            if (File.Exists(fileName) && position == 0)
            {
                File.Delete(fileName);
            }

            using (var fstream = File.OpenWrite(fileName))
            {
                fstream.Position = position;
                context.Request.InputStream.CopyTo(fstream);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("Data_on_server : len=" + context.Request.InputStream.Length + " : " + position + " : " + uploadguid);

            // slow response - only for testing
            Thread.Sleep(500 + new Random().Next(0, 200));

            // random errors - only for testing
            //if (new Random().Next(10) > 7)
            //    throw new ApplicationException();

            //throw new ApplicationException();

        }


    }
}