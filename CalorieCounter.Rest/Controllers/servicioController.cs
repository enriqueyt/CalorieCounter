using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CalorieCounter.Rest.Controllers
{
    public class servicioController : ApiController
    {
        // GET api/servicio
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // GET api/servicio/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/servicio
        public void Post([FromBody]string value)
        {
        }

        // PUT api/servicio/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/servicio/5
        public void Delete(int id)
        {
        }
    }
}
