using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.Configuration;

namespace ChatHelpdescAgent
{
    public class Clients
    {
        /// <summary>
        /// <param name="Rest">Ссылка для  запроса к api</param>
        /// </summary>
        string Rest;// сслыка  на api
        string token;
        //private static IniFile ini = new IniFile("config.ini");
        public Clients()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;// если не сможет подключиться к серверу, то удалить строку.
            Rest = "https://api.chat2desk.com/v1/clients/";
            //token = new IniFileJson().token;
            token = WebConfigurationManager.AppSettings["Chat2DescToken"];

        }
        /// <summary>
        ////Метод возвращает лист клиентов
        /// </summary>
        /// <returns>List <Client></returns>
        public List<Client> GetList()
        {
            List<Client> Clients=GetId();//Получаю id каждого клиента 
            foreach (var C in Clients)//затем в цикле заполняю подробную информацию
            {
                string[] transport = new string[5];
                int Count = 0;
                var client = new RestClient(Rest + C.id.ToString());
                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", token);
                IRestResponse response = client.Execute(request);

                dynamic dynObj = JsonConvert.DeserializeObject(response.Content);
                dynamic d = dynObj.data;    
  
                C.name = d.name;
                C.phone = d.phone;
                C.assigned_name = d.assigned_name;
                C.avatar = d.avatar;
                C.region_id = d.region_id;
                C.country_id = d.country_id;
                C.external_id = d.external_id;
                C.extra_comment_1 = d.extra_comment_1;
                C.extra_comment_2 = d.extra_comment_2;
                C.extra_comment_3 = d.extra_comment_3;

                dynamic transports = dynObj.data.channels;
                foreach (dynamic t in transports)//так как канал это "массив", прогоняем и его в цикле
                {  
                   transport[Count]= t.transports.ToString();
                    Count++;
                }

                C.transports = new string[Count];//выделяю память для объекта Client
                for(int i=0; i < Count;i++ )
                C.transports[i]=transport[i];//записываю каналы связи
            }
            return Clients;
        }

        public Client GetClient(int Id)
        {
            int f = 0;
            f = GetIdClient(Id);
      
                Client Clients = new Client();
                if (f != -1)
                {
                string[] transport = new string[5];
                int Count = 0;
                var client = new RestClient(Rest + Id.ToString());
                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", token);
                IRestResponse response = client.Execute(request);

                dynamic dynObj = JsonConvert.DeserializeObject(response.Content);
                dynamic d = dynObj.data;
                Clients.id = Id.ToString();
                Clients.name = d.name;
                Clients.phone = d.phone;
                Clients.assigned_name = d.assigned_name;
                Clients.avatar = d.avatar;
                Clients.region_id = d.region_id;
                Clients.country_id = d.country_id;
                Clients.external_id = d.external_id;
                Clients.extra_comment_1 = d.extra_comment_1;
                Clients.extra_comment_2 = d.extra_comment_2;
                Clients.extra_comment_3 = d.extra_comment_3;

                dynamic transports = dynObj.data.channels;
                foreach (dynamic t in transports)//так как канал это "массив", прогоняем и его в цикле
                {
                    transport[Count] = t.transports.ToString();
                    Count++;
                }

                Clients.transports = new string[Count];//выделяю память для объекта Client
                for (int i = 0; i < Count; i++)
                    Clients.transports[i] = transport[i];//записываю каналы связи
            }

                return Clients;
            }
           
        /// <summary>
        /// Выбирает весь список клиентов 
        /// </summary>
        /// <returns></returns>
        private List<Client> GetId()
        {
            try
            {
                List<Client> Clients = new List<Client>();
                var client = new RestClient("https://api.chat2desk.com/v1/clients/");
                var request = new RestRequest(Method.GET);
                //     request.AddHeader("postman-token", "724df9c3-0411-af74-62f0-df48cc2aad9f");
                //   request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", token);
                IRestResponse response = client.Execute(request);
                dynamic dynObj = JsonConvert.DeserializeObject(response.Content);
                foreach (dynamic C in dynObj.data)
                {
                    Client c = new Client()
                    { id = C.id };
                    Clients.Add(c);
                }
                return Clients;
            }
            catch(JsonReaderException ex)
            {
                Console.WriteLine("Ошибка при получении списка клиентов: ", ex.Message);
                return null;
            }       
        }

        /// <summary>
        /// Метод SetClient Изменяет данные о клиенте.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="nickname"></param>
        /// <param name="comment"></param>
        /// <param name="extra_comment_1"></param>
        /// <param name="extra_comment_2"></param>
        /// <param name="extra_comment_3"></param>
        /// <returns></returns>
        public bool ChangeClient(int Id, string nickname, string extra_comment_1, string extra_comment_2)
        {
            int flag = 0;
            flag = GetIdClient(Id);// есть ли такой id 
            if (Id >= 0)
            {
                if (flag == 1)
                {
                    int f = 0;
                    string query="{";// для тела запроса
                    var client = new RestClient(Rest + Id.ToString());
                    var request = new RestRequest(Method.PUT);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("authorization", token);
                    // формирую строку запроса. если поле имеет пустые кавыки, то параметр игнорируем.
                    if (nickname != "")
                    {
                        query += "\n\n \"nickname\": \"" + nickname + "\"";
                        f = 1;
                    }
                    if (extra_comment_1 != "")
                    {
                        if (f != 0)
                            query += ",";
                            query += "\n \"extra_comment_1\": \"" + extra_comment_1 + "\"";
                        f = 1;
                    }
                    if (extra_comment_2 != "")
                    {
                        if (f !=0)
                            query += ",";
                        query += "\n \"extra_comment_2\": \"" + extra_comment_2 + "\"\n";
                    }
                    query += "}";// для тела запроса
                    request.AddParameter("application/json", query, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    if (response.ErrorException != null)//произошла ли ошибка
                    {
                        Console.WriteLine("Исключение в методе SetClient() {0}", response.ErrorMessage);
                        return false;
                    }
                }
                else if (flag == -1) return false;//если нет такого id то false 
            }
            return true;
        }

        public bool ChangeClient(int Id, string extra_comment_2)
        {
            int flag = 0;
            flag = GetIdClient(Id);// есть ли такой id 
            if (Id >= 0)
            {
                if (flag == 1)
                {
                    var client = new RestClient(Rest + Id.ToString());
                    var request = new RestRequest(Method.PUT);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("authorization", token);

                    request.AddParameter("application/json", "{\n \"extra_comment_2\": \"" + extra_comment_2 + "\"\n}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.ErrorException != null)//произошла ли ошибка
                    {
                        Console.WriteLine("Исключение в методе SetClient() {0}", response.ErrorMessage);
                        return false;
                    }
                }
                else if (flag == -1) return false;//если нет такого id то false 
            }
            return true;
        }
        /// <summary>
        /// GetIdClient проверяет на существование  клиента
        /// </summary>
        /// <param name="Id">Идентификатор клиента</param>
        /// <returns>Значение типа int. Если значение 1: Существует, -1: клиента с таким id нет.</returns>
        public int GetIdClient(int Id)
        {
            var client = new RestClient(Rest + Id.ToString());
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", token);
            IRestResponse response = client.Execute(request);
            if (response.ErrorException != null)
            {
                Console.WriteLine(" Исключение в методе GetIdClient(int Id) {0}", response.ErrorMessage);
                return -1;
            }
            dynamic dynObj = JsonConvert.DeserializeObject(response.Content);
            if (dynObj == null)
                return -1;
            return 1;
        }
             
    }
}
