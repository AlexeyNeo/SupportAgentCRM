using ChatHelpdescAgent;
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
        public List<Msg> Get()
        {
            List<Msg> messages = new List<Msg>();
            messages.AddRange(GetGmailMessages());
            messages.AddRange(GetChat2DescMessages());
            return messages;
        }

        // GET: api/messages/5
        public Msg Get(int id)
        {
            return null;
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
        List<Msg> GetChat2DescMessages()
        {
            MessagesResponse messagesInCh2D = Messages.GetMessages("from_client", false, 100, read());
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
                };
                messages.Add(message);
            }
            return messages;
        }
    }
}
