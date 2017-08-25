using System;
using System.Web.Http;
using SupportAgentCRM.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ChatHelpdescAgent;

namespace SupportAgentCRM.Controllers
{
    public class HookController : ApiController
    {
        public static int HookCount = 0;
        public static string Error = "";
        public static string value;

        class rt
        {
            public int HookCount { get; set; }
            public string Error { get; set; }
            public string value { get; set; }

        }

        // GET: api/Hook/5
        public dynamic Get()
        {
            MessagesList.Messages.Clear();
            return new rt
            {
                Error = Error,
                HookCount = HookCount,
                value = value
            };
        }

        // POST: api/Hook/value
        public async Task<dynamic> Post()
        {
            string result = await Request.Content.ReadAsStringAsync();
            //dynamic dynMessageString = JsonConvert.DeserializeObject(result);
            if (result != null)
            {
                try
                {
                    dynamic dynMessage = JsonConvert.DeserializeObject(result);
                    Message msg = Messages.GetMessage(dynMessage.message_id.ToString());
                    Clients clients = new Clients();
                    Client client = clients.GetClient(Int32.Parse(msg.clientID));
                    
                    Msg message = new Msg
                    {
                        text = msg.text,
                        ID = msg.ID,
                        Transport = msg.transport,
                        type = msg.type,
                        Name = client.name,
                        Phone = client.phone,
                        dialog = dynMessage.dialog_id,
                        Date = msg.created,
                        external_id = client.external_id,
                        assigned_name = client.assigned_name
                    };
                    MessagesList.Messages.Add(message);
                    Configer.WritePool(MessagesList.Messages);
                    Error = null;
                }
                catch(Exception ex)
                {
                    Error = ex.Message;
                }
            }
            HookCount++;
            value = result;
            return Ok();
        }

        // PUT: api/Hook/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hook
        public dynamic Delete()
        {
            MessagesList.Messages.Clear();
            return Ok();
        }
    }
}
