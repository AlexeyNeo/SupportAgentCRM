using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using SupportAgentCRM.Models;
using System.IO;

namespace SupportAgentCRM.Controllers
{
    public class HookController : ApiController
    {
        public static int HookCount = 0;
        public static string Error = "";
        public dynamic value = null;

        class rt
        {
            public int HookCount { get; set; }
            public string Error { get; set; }
            public dynamic value { get; set; }

        }

        // GET: api/Hook/5
        public dynamic Get([FromBody]string jsonMessage)
        {
            return new rt
            {
                Error = Error,
                HookCount = HookCount
            };
        }
        [HttpPost]
        // POST: api/Hook/value
        public dynamic Post([FromBody] dynamic json)
        {
            try
            {
                Msg Message = new Msg
                {
                    text = json.text,
                    ID = json.id,
                    Transport = json.transport,
                    type = json.type,
                    Date = DateTimeOffset.ParseExact(json.created.ToString().Replace("UTC", "GMT"),
                                                                         "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null),
                    dialog = json.dialog_id
                };
                MessagesList.Messages.Add(Message);

                return json;
            }
            catch (Exception ex)
            {
               
                Error = ex.Message;
                this.value = json;
                return Error;
            }
            HookCount++;
        }

        // PUT: api/Hook/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hook/5
        public void Delete()
        {
            MessagesList.Messages.Clear();
        }
    }
}
