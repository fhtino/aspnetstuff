using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2Simple.Models;

namespace WebApi2Simple.Controllers
{
    public class CarController : ApiController
    {
        // GET: api/Car
        public IEnumerable<Car> Get()
        {
            return new Car[]
            {
                new Car() { ID = -1, ModelName = "fake-1" },
                new Car() { ID = -2, ModelName = "fake-2" }
            };
        }

        // GET: api/Car/5
        public Car Get(int id)
        {
            return new Car() { ID = -1, ModelName = "fake" };
        }

        // POST: api/Car
        public void Post([FromBody]Car c)
        {
        }

        // PUT: api/Car/5
        public void Put(int id, [FromBody]Car c)
        {
        }

        // DELETE: api/Car/5
        public void Delete(int id)
        {
        }
    }
}
