using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace SupportAgentCRM.Models
{
    public static class Configer
    {
        static string FileName = "ChHToken.json";
        public static string Error = null;
        public static string GetToken()
        {
            string path = HostingEnvironment.ApplicationPhysicalPath;
            string token = string.Empty;
            try
            {
                using (FileStream fstream = new FileStream(path + FileName, FileMode.Open))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string json = System.Text.Encoding.Default.GetString(array);
                    ChatHelpDeskToken data = JsonConvert.DeserializeObject<ChatHelpDeskToken>(json);
                    token = data.Token;
                }
            }
            catch(FileNotFoundException ex)
            {

            }
            return token;

        }
        public static void SetToken(string token)
        {
            string path = HostingEnvironment.ApplicationPhysicalPath;
            try
            {
                using (FileStream fstream = new FileStream(path + FileName, FileMode.OpenOrCreate))
                {
                    ChatHelpDeskToken data = new ChatHelpDeskToken
                    {
                        Token = token
                    };
                    string json = JsonConvert.SerializeObject(data);
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(json);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                }
            }
            catch(Exception ex)
            {
                Error = ex.Message;
            }
        }
    }

    class ChatHelpDeskToken
    {
        public string Token { get; set; }
    }
}