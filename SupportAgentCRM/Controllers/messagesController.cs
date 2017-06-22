using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GMailAPILibrary;


namespace SupportAgentCRM.Controllers
{
    public class messagesController : ApiController
    {
        // GET: api/messages
        public IEnumerable<Message> Get()
        {
            return Message.GetMessages();
        }

        // GET: api/messages/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/messages
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/messages/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/messages/5
        public void Delete(int id)
        {
        }
    }
}
