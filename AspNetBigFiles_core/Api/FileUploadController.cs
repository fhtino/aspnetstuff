using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace WebRazor.Api
{

    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {

        [HttpGet]
        public string Index()
        {
            return "FileUpload-API : Hello, world!";
        }


        [HttpPost]
        public async Task<string> UploadChunk()
        {
            string uploadguid = Request.Query["uploadguid"];
            long position = long.Parse(Request.Query["position"]);

            // appent to file
            string fileName = Path.Combine(@"C:\temp\upload", Path.GetFileName(uploadguid));


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

            return $"Data_on_server:  pos={position} guid={uploadguid}";
        }

    }

}
