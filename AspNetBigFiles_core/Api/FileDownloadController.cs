using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace WebRazor.Api
{

    [Route("api/[controller]")]
    [ApiController]
    public class FileDownloadController : ControllerBase
    {

        [HttpGet]
        public async Task Download()
        {
            // required ???  does it work???
            //HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpResponseBodyFeature>().DisableBuffering();

            string fileName = "file.dat";

            HttpContext.Response.ContentType = "application/octet-stream";
            HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename=\"{fileName}\"");

            var buffer = new byte[1 * 1024 * 1024];
            new Random().NextBytes(buffer);

            for (int i = 0; i < 1024; i++)
            {
                //await Response.WriteAsync(new String('x', 1 * 1024 * 1024));
                await Response.Body.WriteAsync(buffer, 0, buffer.Length);
                //Thread.Sleep(1000);   // just for slowing down. Only for testing purpose!
            }
        }

    }

}
