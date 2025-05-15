using System;
using System.IO;
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
            string action = context.Request["action"];

            switch (action)
            {
                case "getFileLength":
                    GetFileLength(context);
                    break;
                case "upload":
                    Upload(context);
                    break;
                default:
                    context.Response.StatusCode = 400;
                    context.Response.Write($"Unknown action: {action}");
                    break;
            }
        }


        private void GetFileLength(HttpContext context)
        {
            string uploadguid = context.Request["uploadguid"];
            
            if (string.IsNullOrWhiteSpace(uploadguid))
            {
                context.Response.StatusCode = 400;
                context.Response.Write($"error: 'uploadguid is required'");
                return;
            }

            string fileName = Path.Combine(@"C:\temp\upload", Path.GetFileName(uploadguid));
            long length = -1; // -1 in case file does not exists on server

            if (File.Exists(fileName))
            {
                length = new FileInfo(fileName).Length;
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(length);
        }

        private void Upload(HttpContext context)
        {
            string uploadguid = context.Request["uploadguid"];

            if (string.IsNullOrWhiteSpace(uploadguid))
            {
                context.Response.StatusCode = 400;
                context.Response.Write($"error: 'uploadguid is required'");
                return;
            }

            long position = long.Parse(context.Request["position"]);

            if (position < 0)
            {
                context.Response.StatusCode = 400;
                context.Response.Write($"error: 'position must be >= 0'");
                return;
            }

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