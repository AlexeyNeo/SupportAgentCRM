using System;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;
using System.Web.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace SupportAgentCRM.Models
{
    public static class Configer
    {
        static string FileName = "ChHToken.json";
        static string MessageFileName = "Rtti";
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
                using (FileStream fstream = new FileStream(path + FileName, FileMode.Create))
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
        public static bool IsSetRead()
        {
            return bool.Parse(WebConfigurationManager.AppSettings["SetMessagesRead"]);
        }
        public static int MessagesLimit()
        {
            return Int32.Parse(WebConfigurationManager.AppSettings["MessagesLimit"]);
        }
        public static List<Msg> ReadPool()
        {
            string path = HostingEnvironment.ApplicationPhysicalPath;
            List<Msg> data;
            try
            {
                using (FileStream fstream = new FileStream(path + MessageFileName, FileMode.Open))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string json = Decrypt(array,(WebConfigurationManager.AppSettings["rtti"]));
                    data = JsonConvert.DeserializeObject<List<Msg>>(json);
                }
            }
            catch (Exception ex)
            {
                return new List<Msg>();
            }
            return data;
        }
        public static bool WritePool(List<Msg> messages)
        {
            string path = HostingEnvironment.ApplicationPhysicalPath;
            try
            {
                using (FileStream fstream = new FileStream(path + MessageFileName, FileMode.Create))
                {
                    
                    string json = JsonConvert.SerializeObject(messages);
                    // преобразуем строку в байты
                    byte[] array = Encrypt(json, (WebConfigurationManager.AppSettings["rtti"]));
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
            return true;
        }
        public static int GetOffset()
        {
            return Int32.Parse(WebConfigurationManager.AppSettings["offset"]);
        }

        private static byte[] Encrypt(string clearText, string EncryptionKey = "123")
        {

            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            byte[] encrypted;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }); // еще один плюс шарпа в наличие таких вот костылей.
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encrypted = ms.ToArray();
                }
            }
            return encrypted;
        }
        private static string Decrypt(byte[] cipherBytes, string EncryptionKey = "123")
        {
            string cipherText = "";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }

    class ChatHelpDeskToken
    {
        public string Token { get; set; }
    }
}