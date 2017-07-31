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
        public List<Msg> Get([FromUri]int? dialog_id)
        {
            List<Msg> messages = new List<Msg>();
            if (dialog_id != null)
            {
                messages.AddRange(GetChat2DescMessages(dialog_id.ToString(), null, null));
            }
            else
            {
               // messages.AddRange(GetGmailMessages());
                messages.AddRange(GetChat2DescMessages(null,Types.from_client, null));
            }
            return messages;
        }

        public dynamic Get([FromUri]string source)
        {
            
            List<Msg> messages = new List<Msg>();
            string ErrorResponse = "Wrong parameters. Try 'gmail' or 'chathelpdesk'";
            if (source.Equals("gmail"))
            {
                Msg msg = GetGmailMessages();
                return msg;
            }
            else if (source.Equals("chathelpdesk") || source.Equals("chat2desk"))
                messages.AddRange(GetChat2DescMessages(null, Types.from_client, false));
            else
                return ErrorResponse;
            return messages;
        }

        // GET: api/messages
        public List<Msg> Get()
        {
            List<Msg> messages = MessagesList.Messages;
            return MessagesList.Messages;
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
                Date = msgBefore.created,
                assigned_name = client.assigned_name
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

        // DELETE: api/messages
        public dynamic Delete()
        {
            return "123456778";
        }

        public static class Types
        {
            public static string from_client = "from_client";
            public static string to_client = "to_client";
            public static string system = "system";
        }


        Msg GetGmailMessages()
         {
            GMailAPILibrary.Message messagesGmail = GMailAPILibrary.Message.GetMessages(true);
            Msg message = null;
            if (messagesGmail != null)
            {
                message = new Msg
                {
                    Name = messagesGmail.sender.Name,
                    text = messagesGmail.TextBody,
                    TextHtml = messagesGmail.HtmlBody,
                    Subject = messagesGmail.Subject,
                    Date = messagesGmail.ReceivedDate,
                    Transport = "gmail",
                    EmailAddress = messagesGmail.sender.Address,
                    Files = messagesGmail.Files
                };
            }

            return message;
        }

        /// <summary>
        /// Сообщения из ChatHelpDesc
        /// </summary>
        /// <param name="dialog">id диалога</param>
        /// <param name="type">тип сообщения</param>
        /// <param name="readState">прочитанные/непрочитанные сообщения</param>
        /// <returns></returns>
        List<Msg> GetChat2DescMessages(string dialog, string type, bool? readState)
        {
            bool setRead = Configer.IsSetRead();
            int limit = Configer.MessagesLimit();
            MessagesResponse messagesInCh2D = Messages.GetMessages(type, readState, limit, setRead, dialog);
            List<Msg> messages = new List<Msg>();
            foreach (var msg in messagesInCh2D.messages)
            {
                Client client = new Clients().GetClient(Int32.Parse(msg.clientID));
                //var transport = ChatHelpdescAgent.Messages.GetMessage(msg.ID).transport;
                Msg message = new Msg()
                {
                    ID = msg.ID,
                    Name = client.name,
                    Post = client.extra_comment_2,
                    Company = client.extra_comment_1,
                    text = msg.text,
                    Transport = msg.transport,
                    Phone = client.phone,
                    dialog = msg.dialog_id,
                    Date = msg.created,
                    type = msg.type,
                    assigned_name = client.assigned_name
                };
                messages.Add(message);
            }
            return messages;
        }
    }
}
