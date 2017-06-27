using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatHelpdescAgent;
using Newtonsoft.Json;

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

        public class Params
        {
            string value { get; set; }
        }

        // PUT: api/client/5
        public void Put(string id, [FromBody]string json)
        {
            dynamic p = JsonConvert.DeserializeObject(json);
            string clientID
            string post = p.post;
            string company = p.company;
            string name = p.name;
        }

        // DELETE: api/client/5
        public void Delete(int id)
        {
        }
    }
}
