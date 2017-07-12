﻿using System;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        class rt
        {
            public static int HookCount = 0;
            public static string Error = "";
            public static string js = "";
        }

        // GET: api/Hook/5
        public void Get([FromBody]string jsonMessage)
        {
            try
            {
                dynamic dynMessage = JsonConvert.DeserializeObject(jsonMessage);
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
                rt.Error = ex.Message;
                rt.js = jsonMessage;
            }
            rt.HookCount++;
        }

        // POST: api/Hook
        public dynamic Post([FromBody]string value)
        {
            return new rt();
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
