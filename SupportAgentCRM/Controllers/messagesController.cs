using System.Collections.Generic;
using System.Web.Http;

namespace SupportAgentCRM.Controllers
{
    public class Messages
    {
        public IEnumerable <GMailAPILibrary.Message> gmail { get; set; }
        public IEnumerable <ChatHelpdescAgent.Message> chat2desk { get; set; }
    }
    public class messagesController : ApiController
    {
        // GET: api/messages
        public Messages Get()
        {
            var clients = new ChatHelpdescAgent.Clients();
            var msg = new Messages();
            msg.gmail = GMailAPILibrary.Message.GetMessages();
            msg.chat2desk = ChatHelpdescAgent.Messages.GetMessages("From_client", true,10, false).messages;
            return msg;
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
