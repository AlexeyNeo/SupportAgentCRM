using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SupportAgentCRM.Models;

namespace SupportAgentCRM.Controllers
{
    public class HookController : ApiController
    {
        // GET: api/Hook
        

        public static int HookCount = 0;
        public static string Error = "";
        public static string js = "";

        class rt
        {
            public int HookCount { get; set; }
            public string Error { get; set; }
            public string js { get; set; }
        }

        // GET: api/Hook/5
        public dynamic Get([FromBody]string jsonMessage)
        {
            return new rt
            {
                Error = Error,
                HookCount = HookCount,
                js = js
            };
        }

        // POST: api/Hook
        public void Post([FromBody]string value)
        {
            try
            {
                dynamic dynMessage = JsonConvert.DeserializeObject(value);
                Msg Message = new Msg
                {
                    text = dynMessage.text,
                    ID = dynMessage.id,
                    Transport = dynMessage.transport,
                    type = dynMessage.type,
                    Date = DateTimeOffset.ParseExact(dynMessage.created.ToString().Replace("UTC", "GMT"),
                                                                         "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null),
                    dialog = dynMessage.dialog_id
                };
                MessagesList.Messages.Add(Message);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                js = value;
            }
            HookCount++;
        }

        // PUT: api/Hook/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hook/5
        public void Delete(int id)
        {
        }
    }
}
