using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Web.Http;

namespace WebApi2Simple.Controllers
{
    public class OperationsController : ApiController
    {

        public class Error
        {
            public int Code { get; set; }
            public string Info { get; set; }
            public DateTime DT { get; set; }
        }

        public class SumRequest
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class Sample
        {
            public DateTime DT { get; set; }
            public double Value { get; set; }
        }


        public class GetFileRequest
        {
            public int ID { get; set; }
            public string FileName { get; set; }
            public int Size { get; set; }
        }


        public class GetFileResponse
        {
            public string Name { get; set; }
            public byte[] Body { get; set; }
            public byte[] MD5 { get; set; }
            public DateTime DT { get; set; }
        }



        [HttpGet]
        public DateTime IsAlive()
        {
            return DateTime.UtcNow;
        }


        [HttpPost]
        public int Sum([FromBody] SumRequest req)
        {
            return req.X + req.Y;
        }         


        [HttpPost]
        public Sample[] CreateSamples([FromBody] int n)
        {
            var rnd = new Random();
            var resp = new Sample[n];
            for (int i = 0; i < n; i++)
            {
                resp[i] = new Sample { DT = DateTime.UtcNow, Value = rnd.NextDouble() };
                Thread.Sleep(rnd.Next(20));
            }
            return resp;
        }


        [HttpPost]
        public GetFileResponse GetFile([FromBody] GetFileRequest req)
        {
            if (req==null)
                throw CreateErrorResponse(1, "empty req");

            if (req.Size < 1 || req.Size > 1000)
                throw CreateErrorResponse(2, "invalid size");
                    
            var body = new byte[req.Size];
            new Random().NextBytes(body);
            return new GetFileResponse
            {
                Name = req.FileName,
                Body = body,
                MD5 = MD5.Create().ComputeHash(body),
                DT=DateTime.UtcNow
            };
        }


        // ------------------

        private HttpResponseException CreateErrorResponse(int errorCode, string errorInfo)
        {
            var msg = Request.CreateResponse<Error>(
                           HttpStatusCode.BadRequest,
                           new Error()
                           {
                               Code = errorCode,
                               Info = errorInfo,
                               DT = DateTime.UtcNow
                           });
            return new HttpResponseException(msg);
        }



    }
}
