using System;
using System.Collections.Generic;
using SupportAgentCRM.Models;
using RestSharp;
using Newtonsoft.Json;

namespace ChatHelpdescAgent
{
    public static class Dialogs
    {
        public static string Rest = "https://api.chat2desk.com/v1/dialogs/";
        public static string token = Configer.GetToken();

        public static List<Dialog> Get()
        {
            //возвращает лист диалогов
            List<Dialog> dialogs = new List<Dialog>();
            var client = new RestClient(Rest);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", Configer.GetToken());
            IRestResponse response = client.Execute(request);

            dynamic dynObj = JsonConvert.DeserializeObject(response.Content);

            if (dynObj == null || dynObj.status == "error")
            {
                Dialog dlg = new Dialog();
                dlg.error = "Диалогов нет или произошла ошибка в запросе.";
                dialogs.Add(dlg);
                return dialogs;
            }
            
            foreach (dynamic dialog in dynObj.data)
            {
                Dialog dlg = new Dialog();
                dlg.ID = dialog.id;
                dlg.state = dialog.state;
                dlg.begin = DateTimeOffset.ParseExact(dialog.begin.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null).UtcDateTime;
                if (dlg.end != null)
                    dlg.end = DateTimeOffset.ParseExact(dialog.end.ToString().Replace("UTC", "GMT"),
                                                                         "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null).UtcDateTime;
                dialogs.Add(dlg);
            }
            return dialogs;

        }

        public static Dialog Get(int id)
        {//возвращает диалог
            Dialog dlg = new  Dialog();
            var client = new RestClient(Rest + id.ToString());
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", Configer.GetToken());
            IRestResponse response = client.Execute(request);

            dynamic DynObj = JsonConvert.DeserializeObject(response.Content);
            dynamic dialog = DynObj.data;

            if (DynObj == null || DynObj.status == "error")
            {
                dlg.error = "Такого диалога не существует или произошла ошибка в запросе.";
                return dlg;
            }

            dlg.ID = dialog.id;
            dlg.state = dialog.state;
            dlg.begin = DateTimeOffset.ParseExact(dialog.begin.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null).UtcDateTime.AddHours(Configer.GetOffset());
            if(dlg.end!=null)
                dlg.end = DateTimeOffset.ParseExact(dialog.end.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null).UtcDateTime.AddHours(Configer.GetOffset());
            return dlg;
        }
    }
}