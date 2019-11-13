using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi2Simple.Controllers
{
    public class DataController : ApiController
    {
        [HttpGet]
        public DateTime IsAlive()
        {
            return DateTime.UtcNow;
        }


        [HttpGet]
        public DateTime IsAliveLocal()
        {
            return DateTime.UtcNow;
        }
    }
}
