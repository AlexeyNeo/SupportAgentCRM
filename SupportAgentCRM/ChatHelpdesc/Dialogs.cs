using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupportAgentCRM.Models;
using System.Web.Configuration;
using RestSharp;
using Newtonsoft.Json;

namespace ChatHelpdescAgent
{
    public static class Dialogs
    {
          public static string Rest = "https://api.chat2desk.com/v1/dialogs/";
          public static string token = WebConfigurationManager.AppSettings["Chat2DescToken"];

        public static List<Dialog> Get()
        {
           // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;// если не сможет подключиться к серверу, то удалить строку.
            Dialog dlg = new Dialog();
            List<Dialog> dialogs = new List<Dialog>();
            var client = new RestClient(Rest);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", token);
            IRestResponse response = client.Execute(request);

            dynamic dynObj = JsonConvert.DeserializeObject(response.Content);

            foreach (dynamic dialog in dynObj.data)
            {
                dlg.ID = dialog.id;
                dlg.state = dialog.state;
                dlg.begin = DateTimeOffset.ParseExact(dialog.begin.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null);
                dlg.end = DateTimeOffset.ParseExact(dialog.last_message.created.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null);
                dialogs.Add(dlg);
            }
            return dialogs;
        }
    }
}