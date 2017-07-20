using System.Collections.Generic;
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
        public Client Get(int id)
        {
            Clients clients = new Clients();
            return clients.GetClient(id);
        }

        // POST: api/client
        // создание клиента в chat2desc
        public void Post([FromBody]string name, [FromBody]string company, [FromBody]string post,[FromBody]string phone,[FromBody]string telegramID)
        {
            
        }

        // PUT: api/client/5
        public void Put(string id, [FromBody]string json)
        {
            Clients clients = new Clients();
            dynamic p = JsonConvert.DeserializeObject(json);
            string clientId = p.id;
            string post="";
            string company = "";
            string name = "";
            if (p.post != null) post = p.post;
            if (p.name != null) name = p.name;
            if (p.company != null) company = p.company;
            clients.ChangeClient(int.Parse(id), name, company, post);
        }

        // DELETE: api/client/5
        public void Delete(int id)
        {
        }
    }
}
