using ChatHelpdescAgent;
using SupportAgentCRM.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
namespace SupportAgentCRM.Controllers
{
    public class messagesController : ApiController
    {
        // GET: api/messages
        public List<Msg> Get()
        {
            List<GMailAPILibrary.Message> messagesGmail = GMailAPILibrary.Message.GetMessages();
            MessagesResponse messagesInCh2D = Messages.GetMessages("from_client", false, 100, false);
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
                    Transport = "gmail"
                };
                messages.Add(message);
            }

            foreach (var msg in messagesInCh2D.messages)
            {
                Client client = new Clients().GetClient(Int32.Parse(msg.clientID));
                Msg message = new Msg()
                {
                    Name = client.name,
                    Post = client.extra_comment_2,
                    Company = client.extra_comment_1,
                    text = msg.text,
                    Transport = msg.transport,
                    Phone = client.phone
                };
                messages.Add(message);
            }

            return messages;
        }

        // GET: api/messages/5
        public string Get(int id)
        {
            return "value";
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
    }
}
