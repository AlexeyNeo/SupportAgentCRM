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
            //возвращает лист диалогов
            List<Dialog> dialogs = new List<Dialog>();
            var client = new RestClient(Rest);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", token);
            IRestResponse response = client.Execute(request);

            dynamic dynObj = JsonConvert.DeserializeObject(response.Content);

            if (dialog == null || dialog.status == "error")
                return null;

            foreach (dynamic dialog in dynObj.data)
            {
                Dialog dlg = new Dialog();
                dlg.ID = dialog.id;
                dlg.state = dialog.state;
                dlg.begin = DateTimeOffset.ParseExact(dialog.begin.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null);
                dlg.end = DateTimeOffset.ParseExact(dialog.end.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null);
                dialogs.Add(dlg);
            }
            return dialogs;
        }
        public static Dialog Get(int id)
        {//возвращает диалог
            Dialog dlg = new  Dialog();
            var client = new RestClient(Rest + id.ToString());
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", token);
            IRestResponse response = client.Execute(request);

            dynamic DynObj = JsonConvert.DeserializeObject(response.Content);
            dynamic dialog = DynObj.data;

            if (dialog == null || dialog.status =="error")
                return null;

            dlg.ID = dialog.id;
            dlg.state = dialog.state;
            dlg.begin = DateTimeOffset.ParseExact(dialog.begin.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null);
            dlg.end = DateTimeOffset.ParseExact(dialog.end.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null);
            return dlg;
        }
    }
}