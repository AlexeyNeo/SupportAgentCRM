﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatHelpdescAgent;

namespace SupportAgentCRM.Controllers
{
    public class clientController : ApiController
    {
        // GET: api/client
        public List<Client> Get()
        {
            Clients clients = new Clients();
            return clients.GetList();
        }

        // GET: api/client/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/client
        // создание клиента в chat2desc
        public void Post([FromBody]string name, [FromBody]string company, [FromBody]string post,[FromBody]string phone,[FromBody]string telegramID)
        {
            
        }

        // PUT: api/client/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/client/5
        public void Delete(int id)
        {
        }
    }
}
