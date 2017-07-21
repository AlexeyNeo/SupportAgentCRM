using System;
using System.Collections.Generic;

namespace SupportAgentCRM.Models
{
    public class Msg
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Post { get; set; }
        public string Company { get; set; }
        public string text { get; set; }
        public string TextHtml { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Transport { get; set; }
        public string Phone { get; set; }
        public string dialog { get; set; }
        public string type { get; set; }
        public List<string> Files { get; set; }
        public string external_id { get; set; }
        public string assigned_name { get; set; }
    }

    public static class MessagesList
    {
        public static List<Msg> Messages = new List<Msg>();
    }
   
}