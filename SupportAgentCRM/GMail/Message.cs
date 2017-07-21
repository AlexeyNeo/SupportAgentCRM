using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GMailAPILibrary
{
    public class Message
    {
        public string Subject { get; }
        public DateTimeOffset ReceivedDate { get; }
        public Dictionary<string, string> From { get; }
        public Dictionary<string, string> To { get; }
        public string Snippet { get; }
        public string TextBody { get; }
        public string HtmlBody { get; }
        
        public struct Sender
        {
            public string Address;
            public string Name;

            public Sender(string address, string name)
            {
                this.Address = address;
                this.Name = name;
            }
        }
        public Sender sender { get; }
        public string Files {get; set;}

        public Message(string raw)
        {
            //декодировка raw сообщения из формата base64url
            string mimeMessage = Base64UrlDecode(raw);

            //формирование и парсинг сообщения с помощью MimeKit
            byte[] ar = Encoding.UTF8.GetBytes(mimeMessage);
            Stream stream = new MemoryStream(ar);
            var parser = new MimeParser(stream, MimeFormat.Default);
            var message = parser.ParseMessage();
           

            //отправитель sender
            if (message.Sender != null)
                sender = new Sender(message.Sender.Address, message.Sender.Name);//отправитель
            else
            {
                var address = message.From[0] as MailboxAddress;
                sender = new Sender(address.Address, address.Name);
            }

            //сохранение присоединенных файлов
            Files = GetAttachment(message);
            Subject = message.Subject;//тема
            ReceivedDate = message.Date;//дата
            TextBody = message.TextBody;//тело сообщения
            HtmlBody = message.HtmlBody;//тело сообщения в html
            

            //список заголовков
        /*    this.Headers = new List<string>();
            foreach (var header in message.Headers)
                this.Headers.Add(header.Value);
                */

            //адрес отправителя (from)
            this.From = new Dictionary<string, string>();
            var internetAddressList = message.From;
            foreach (MailboxAddress from in internetAddressList)
                this.From.Add(from.Address, from.Name);

            //адрес получателя
            this.To = new Dictionary<string, string>();
            internetAddressList = message.To;
            foreach (MailboxAddress to in internetAddressList)
                this.To.Add(to.Address, to.Name);
        }

        public static string Base64UrlDecode(string input)
        {
           
            string text = string.Empty;
            if (input.Length % 4 == 0)
            {
                byte[] buffer = Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/'));
                text = Encoding.UTF8.GetString(buffer); // Hello, World
            }
            return text;
        }

        protected string  GetAttachment(MimeMessage message)
        {
            string text = "";
            int count= 0;
            List<string> files= new List<string>();
            string file="";
            string path = @"E:\";
            var attachments = message.Attachments;
            foreach (var attachment in message.Attachments)
            {
                // files = new string[1];
                if (attachment is MessagePart)
                {

                    var rfc822 = (MessagePart)attachment;

                    FileStream readstream = new FileStream("1.txt", FileMode.Create);
                    rfc822.Message.WriteTo(readstream);

                    using (FileStream fstream = new FileStream("1.txt", FileMode.OpenOrCreate))
                    {
                        // получаем массив байтов из потока
                        byte[] array = new byte[readstream.Length];
                        readstream.Read(array, 0, array.Length);
                        // запись массива байтов в файл
                        fstream.Write(array, 0, array.Length);
                    }
                }
                else
                {

                    var part = (MimePart)attachment;
                    var fileName = part.FileName;
                    FileStream  filestream;
                    using (filestream = File.Create(path + @"/file.txt"))
                    {
                        part.ContentObject.DecodeTo(filestream);
                        
                        // считываем данные
                       
                    }
                   
                    using (var fileread = File.Open(path + @"/file.txt", FileMode.Open ))
                    {
                        byte[] b = new byte[fileread.Length];
                        fileread.Read(b, 0, b.Length);
                        text =  Encoding.Default.GetString(b);
                        
                    }
                    using (FileStream wrStream = File.Create(path + fileName))
                    {
                        byte[] ar = Encoding.Default.GetBytes(text);
                        wrStream.Write(ar, 0, ar.Length);
                    }
                }
            }

            return text;
        }
        /// <summary>
        ////Метод дергает Gmail
        /// </summary>
        /// <param name="type"> true- возврвщает 1 новое, false- список</param>
        /// <returns></returns>
        public static dynamic GetMessages(bool type=false)
        {
            var messages = new List<Message>();
            if (type == false)
            {
                var rawMessages = GmailApi.GetMsgs();//список raw сообщений
                foreach (var rawMessage in rawMessages)
                {
                    messages.Add(new Message(rawMessage));
                }
                return messages;
            }
            else
            {
                var rawMessage = GmailApi.GetMsg();
                Message Msg = new Message(rawMessage);
                return Msg;
            }      
        }
    }
}
