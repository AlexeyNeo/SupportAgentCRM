﻿using ChatHelpdescAgent;
using SupportAgentCRM.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Configuration;

namespace SupportAgentCRM.Controllers
{
    public class messagesController : ApiController
    {
        // GET: api/messages
        public List<Msg> Get([FromUri]int? dialog_id)
        {
            List<Msg> messages = new List<Msg>();
            if (dialog_id != null)
            {
                messages.AddRange(GetChat2DescMessages(dialog_id.ToString(), null));
            }
            else
            {
                messages.AddRange(GetGmailMessages());
                messages.AddRange(GetChat2DescMessages(null,Types.from_client));
            }
            return messages;
        }



        // GET: api/messages
        public List<Msg> Get()
        {
            List<Msg> messages = new List<Msg>();
            messages.AddRange(GetGmailMessages());
            messages.AddRange(GetChat2DescMessages(null, null));
            return messages;
        }

        // GET: api/messages/5
        public Msg Get(int id)
        {
            Message msgBefore = Messages.GetMessage(id.ToString());
            Clients clients = new Clients();
            int ClientID = Int32.Parse(msgBefore.clientID);
            Client client = clients.GetClient(ClientID);
            Msg msgAfter = new Msg
            {
                ID = msgBefore.ID,
                Name = client.assigned_name,
                Post = client.extra_comment_2,
                Company = client.extra_comment_1,
                text = msgBefore.text,
                Transport = msgBefore.transport,
                Phone = client.phone,
                dialog = msgBefore.dialog_id,
                Date = msgBefore.created
            };

            return msgAfter;
        }

        // POST: api/messages
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/messages/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/messages/5
        public void Delete(int id)
        {
        }

        public static class Types
        {
            public static string from_client = "from_client";
            public static string to_client = "to_client";
            public static string system = "system";
        }


        bool read()
        {
            return Boolean.Parse(WebConfigurationManager.AppSettings["SetMessagesRead"]);
        }
        List<Msg> GetGmailMessages()
        {
            List<GMailAPILibrary.Message> messagesGmail = GMailAPILibrary.Message.GetMessages();
            List<Msg> messages = new List<Msg>();
            foreach (var msg in messagesGmail)
            {
                Msg message = new Msg
                {
                    Name = msg.sender.Name,
                    text = msg.TextBody,
                    TextHtml = msg.HtmlBody,
                    Subject = msg.Subject,
                    Date = msg.ReceivedDate,
                    Transport = "gmail",
                    EmailAddress = msg.sender.Address
                };
                messages.Add(message);
            }
            return messages;
        }
        List<Msg> GetChat2DescMessages(string dialog, string type)
        {
            bool setRead = read();
            MessagesResponse messagesInCh2D = Messages.GetMessages(type, true, 100, setRead, dialog);
            List<Msg> messages = new List<Msg>();
            foreach (var msg in messagesInCh2D.messages)
            {
                Client client = new Clients().GetClient(Int32.Parse(msg.clientID));
                var transport = ChatHelpdescAgent.Messages.GetMessage(msg.ID).transport;
                Msg message = new Msg()
                {
                    ID = msg.ID,
                    Name = client.name,
                    Post = client.extra_comment_2,
                    Company = client.extra_comment_1,
                    text = msg.text,
                    Transport = transport,
                    Phone = client.phone,
                    dialog=msg.dialog_id,
                    Date=msg.created
                    
                };
                messages.Add(message);
            }
            return messages;
        }
    }
}
