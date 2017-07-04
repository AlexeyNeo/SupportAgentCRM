using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using System.Web;
using System.Web.Configuration;

namespace ChatHelpdescAgent
{
    /// <summary>
    /// Предоставляет методы для получения и обработки сообщений
    /// </summary>
    public static class Messages
    {
        //"d1bdb8e80fc2c4d3050d49a10a433d"
        //static IniFile ini = new IniFile(Environment.CurrentDirectory+@"\config.ini");
        static string token = WebConfigurationManager.AppSettings["Chat2DescToken"];

        static string url = "https://api.chat2desk.com/v1/";

    

        /// <summary>
        /// Возвращает список сообщений
        /// </summary>
        /// <param name="type">фильтр по типу</param>
        /// <param name="read">фильтр по прочитанным или непрочитанным</param>
        /// <param name="setRead">указывает необходимо ли пометить полученные сообщения как прочитанные</param>
        /// <returns></returns>
        public static MessagesResponse GetMessages(string type, bool? read, int? limit, bool setRead, string dialog_id)
        {
           // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string reqURL = url + "messages";
            
            //формирование URL строки с параметрами
            var uriBuilder = new UriBuilder(reqURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (type != null)
                query["type"] = "from_client";
            if(read != null)
                query["read"] = read.ToString();
            if (limit != null)
                query["limit"] = limit.ToString();
            if (dialog_id != null)
                query["dialog_id"] = dialog_id;
            uriBuilder.Query = query.ToString();
            reqURL = uriBuilder.ToString();
            
            //отправка запроса на сервер
            var client = new RestClient(reqURL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", token);
            IRestResponse response = client.Execute(request);

            //получение коллекции сообщений
            List<Message> messages = new List<Message>();
            dynamic dynObj = JsonConvert.DeserializeObject(response.Content);//парсим json
            if (dynObj.data != null)
            {
                foreach (dynamic dynMessage in dynObj.data)
                {
                    Message message = new Message
                    {
                        ID = dynMessage.id,
                        text = dynMessage.text,
                        type = dynMessage.type,
                        read = Boolean.Parse(dynMessage.read.ToString()),
                        created = DateTimeOffset.ParseExact(dynMessage.created.ToString().Replace("UTC", "GMT"),
                                                                     "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null),
                        clientID = dynMessage.client_id,
                        dialog_id = dynMessage.dialog_id
                    };

                    if (setRead) //если пометить полученные сообщения как прочитанные
                        SetRead(message.ID, true);
                    messages.Add(message);
                }
            }

            MessagesResponse messagesResponse = new MessagesResponse
            {
                messages = messages,
                info = new MessagesResponse.MetaInfo
                {
                    total = Int32.Parse(dynObj.meta.total.ToString()),
                    limit = Int32.Parse(dynObj.meta.limit.ToString())
                }
            };
            return messagesResponse;
        }

        /// <summary>
        /// Помечает указанное сообщение как прочитанное или непрочитанное
        /// </summary>
        /// <param name="messageId">id сообщения</param>
        /// <param name="read">статус</param>
        /// <returns></returns>
        public static bool SetRead(string messageId, bool read)
        {
            //формировании url запроса
            string reqURL = url + "messages/" + messageId + "/";// /v1/messages/<id сообщения>/
            string readState = string.Empty;
            if (read)
                readState = "read";// /v1/messages/<id сообщения>/read
            else
                readState = "unread";// /v1/messages/<id сообщения>/unread
            reqURL += readState;

            //отправка запроса на сервер
            var client = new RestClient(reqURL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", token);
            IRestResponse response = client.Execute(request);

            dynamic dynObj = JsonConvert.DeserializeObject(response.Content);//парсим json

            if (dynObj.status == "success")//если операция успешна
                return true;
            else
                return false;
        }

        /// <summary>
        /// Возвращает указанное сообщение
        /// </summary>
        /// <param name="messageId">id сообщения</param>
        /// <returns></returns>
        public static Message GetMessage(string messageId)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //формировании url запроса
            string reqURL = url + "messages/" + messageId;// /v1/messages/<id сообщения>/

            //отправка запроса на сервер
            var client = new RestClient(reqURL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", token);
            IRestResponse response = client.Execute(request);

            dynamic dynObj = JsonConvert.DeserializeObject(response.Content);//парсим json

            if (dynObj.status == "success")
            {
                Message message = new Message
                {
                    ID = dynObj.data.id,
                    text = dynObj.data.text,
                    type = dynObj.data.type,
                    read = Boolean.Parse(dynObj.data.read.ToString()),
                    created = DateTimeOffset.ParseExact(dynObj.data.created.ToString().Replace("UTC", "GMT"),
                                                                 "yyyy'-'MM'-'dd'T'HH':'mm':'ss GMT", null),
                    clientID = dynObj.data.client_id,
                    transport = dynObj.data.transport
                };
                return message;
            }
            else
                return null;
        }
        
    }

    /// <summary>
    /// Представляет все данные сообщения
    /// </summary>
    public class Message
    {
        public string ID { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public bool read { get; set; }
        public DateTimeOffset created { get; set; }
        public string clientID { get; set; }
        public string transport { get; set; }
        public string dialog_id { get; set; }
    }

    /// <summary>
    /// Представляет результат запроса на получение сообщений
    /// </summary>
    public class MessagesResponse
    {
        public List<Message> messages { get; set; }
        public MetaInfo info { get; set; }

        public class MetaInfo
        {
            public int total { get; set; }
            public int limit { get; set; }
        }
    }

    


}
