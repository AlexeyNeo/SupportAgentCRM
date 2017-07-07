using System;
using System.Collections.Generic;
using System.Web.Http;
using SupportAgentCRM.Models;
using ChatHelpdescAgent;

namespace SupportAgentCRM.Controllers
{
    public class dialogsController : ApiController
    {
        // GET api/<controller>
        public List<Dialog> Get()
        {    
            return Dialogs.Get();
        }

        // GET api/<controller>/5
        public Dialog Get(int id)
        {
            return Dialogs.Get(id);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}