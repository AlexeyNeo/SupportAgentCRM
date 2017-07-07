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
            GetAttachment(message);

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

        protected void GetAttachment(MimeMessage message)
        {
            var attachments = message.Attachments;
            foreach (var attachment in message.Attachments)
            {
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

                    using (var filestream = File.Create(fileName))
                        part.ContentObject.DecodeTo(filestream);

                }
            }
        }

        public static List<Message> GetMessages()
        {
            var rawMessages = GmailApi.GetMsg();//список raw сообщений

            var messages = new List<Message>();
            foreach (var rawMessage in rawMessages)
            {
                messages.Add(new Message(rawMessage));
            }

            return messages;
        }
    }
}
