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
        public static string js = "";

        class rt
        {
            public int HookCount { get; set; }
            public string Error { get; set; }
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
        public void Post([FromBody]dynamic value)
        {
            try
            {
                Msg Message = new Msg
                {
                    text = value.data.text,
                    ID = value.data.id,
                    Transport = value.data.transport,
                    type = value.data.type,
                    Date = DateTimeOffset.ParseExact(value.data.created.ToString().Replace("UTC", "GMT"),
                                                                         "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null),
                    dialog = value.data.dialog_id
                };
                MessagesList.Messages.Add(Message);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
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
