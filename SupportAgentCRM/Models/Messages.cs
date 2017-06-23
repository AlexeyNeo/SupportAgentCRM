using System;

namespace SupportAgentCRM.Models
{
    public class Msg
    {
        public string Name { get; set; }
        public string Post { get; set; }
        public string Company { get; set; }
        public string text { get; set; }
        public string TextHtml { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset date { get; set; }
        public string transport { get; set; }
    }

    // GET: api/messages
   
}