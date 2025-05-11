using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebRazor.Api
{

    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {

        [HttpGet("Index")]
        public string Index()
        {
            return "FileUpload-API : Hello, world!";
        }


        [HttpGet]
        public string GetFileLength([FromQuery] string uploadGuid)
        {
            if (string.IsNullOrWhiteSpace(uploadGuid))
            {
                Response.StatusCode = 400;
                return $"error: 'uploadGuid is required'";
            }

            string fileName = Path.Combine(@"C:\temp\upload", Path.GetFileName(uploadGuid));
            long length = -1; // -1 in case file does not exists on server

            if (System.IO.File.Exists(fileName))
            {
                length = new FileInfo(fileName).Length;
            }

            return $"{length}";
        }


        [HttpPost]
        public async Task<string> UploadChunk([FromQuery] string uploadGuid, [FromQuery] long position)
        {
            if (string.IsNullOrWhiteSpace(uploadGuid))
            {
                Response.StatusCode = 400;
                return $"error: 'uploadGuid is required'";
            }

            if (position < 0)
            {
                Response.StatusCode = 400;
                return $"error: 'position must be >= 0'";
            }

            // appent to file
            string fileName = Path.Combine(@"C:\temp\upload", Path.GetFileName(uploadGuid));


            if (System.IO.File.Exists(fileName) && position == 0)
            {
                System.IO.File.Delete(fileName);
            }

            using (var fstream = System.IO.File.OpenWrite(fileName))
            {
                fstream.Position = position;
                await Request.Body.CopyToAsync(fstream);
            }

            // slow response - only for testing
            Thread.Sleep(500 + new Random().Next(0, 200));

            // random errors - only for testing
            //if (new Random().Next(10) > 7)
            //    throw new ApplicationException();

            //throw new ApplicationException();

            return $"Data_on_server:  pos={position} guid={uploadGuid}";
        }

    }

}
