using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ChatHelpdescAgent
{
    class IniFileJson
    {
        string path = Environment.CurrentDirectory + "/config.ini";
        public string token { get; }

        public bool enabled { get; }


        public IniFileJson()
        {
            string jsonText;

            if (File.Exists(path))
            {
                using (FileStream fstream = new FileStream(path, FileMode.Open))
                {
                    byte[] ar = new byte[fstream.Length];
                    fstream.Read(ar, 0, ar.Length);
                    jsonText = Encoding.Default.GetString(ar);
                    token = JsonConvert.DeserializeObject<string>(jsonText);
                }
            }
            else
            {
                using (FileStream fstream = new FileStream(path, FileMode.Create))
                {
                    token = "dddddddddddddddddddddddddddddddddddddddddd";
                    jsonText = JsonConvert.SerializeObject(token);
                    byte[] ar = Encoding.Default.GetBytes(jsonText);
                    fstream.Write(ar, 0, ar.Length);
                }
            }


        }
    }
}
