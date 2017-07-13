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

        // POST: api/Hook/value
        public void Post([FromBody] dynamic value)
        {
            if (value = null)
            {
                try
                {
                    Msg Message = new Msg
                    {
                        text = value.text,
                        ID = value.id,
                        Transport = value.transport,
                        type = value.type,
                        Date = DateTimeOffset.ParseExact(value.created.ToString().Replace("UTC", "GMT"),
                                                                             "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null),
                        dialog = value.dialog_id
                    };
                    MessagesList.Messages.Add(Message);
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    this.value = value;
                }
            }
            else
            {
                Error = "value пуст";
                this.value = value;
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
