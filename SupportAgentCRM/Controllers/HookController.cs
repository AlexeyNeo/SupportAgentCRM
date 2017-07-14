using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using SupportAgentCRM.Models;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
        public dynamic Get([FromBody]string jsonMessage)
        {
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
                    Msg message = new Msg
                    {
                        text = dynMessage.text.ToString(),
                        ID = dynMessage.message_id.ToString(),
                        Transport = dynMessage.transport.ToString(),
                        type = dynMessage.type.ToString(),
                        Name = dynMessage.client.name.ToString(),
                        Phone = dynMessage.client.phone.ToString(),
                        dialog = dynMessage.dialog_id.ToString(),
                        Date = DateTimeOffset.Now
                    };
                    MessagesList.Messages.Add(message);
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
