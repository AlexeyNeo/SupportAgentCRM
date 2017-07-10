using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using GMailAPILibrary;
using Message = Google.Apis.Gmail.v1.Data.Message;
using Google.Apis.Util.Store;

public static class GmailApi
{
    static readonly string[] Scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailModify };// не только чтение но и изменение
    private const string ApplicationName = "GmailAPI";
    private const string Status = "UNREAD"; //статус сообщения
    private const string UserId = "me";
    //AIzaSyBoM2fANLjjx6gIwRECf7Wz5vL0-2ihInU

    private static GmailService Connection() //возвращет сервис.
    {
        var clientSecretData = Encoding.ASCII.GetBytes(ApiHelper.ClientSecret);
        var stream = new MemoryStream(clientSecretData);
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(@"E:\service", true)).Result;

        var service = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
        return service;
    }

    public static List<string> GetMsg()//основной метод который будет читать и возвращать смс
    {
        var parseMessages = new List<string>();
        var service = Connection();//метод возвращает созданный сервис
        var listmsg = new List<Message>();//  raw
        var messages = ListMessages(service, UserId, Status);
        if (messages != null)
        {
            foreach (var m in messages)
            {
                //Listmsg.Add(GmailAPI.GetMessage(service, UserId, m.Id));//вытаскиваю raw каждого нового сообщения
                parseMessages.Add(GetMessage(service, UserId, m.Id).Raw);
                ModifyMessage(service, UserId, m.Id); // переносим из UNREAD
            }
        }

        return parseMessages;
    }

    public static Message ModifyMessage(GmailService service, string userId, string messageId)
    {
        var mods = new ModifyMessageRequest { RemoveLabelIds = new[] { Status } }; // заголовок, который нужно удалить
        try
        {
            return service.Users.Messages.Modify(mods, userId, messageId).Execute();//удаляем пометку не прочитано
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static List<Message> ListMessages(GmailService service, string userId, string query)
    {
        var result = new List<Message>();
        var request = service.Users.Messages.List(userId);
        request.LabelIds = query;
        do
        {
            try
            {
                var response = request.Execute();
                if (response.Messages != null)// если нет новых писем то нулл
                    result.AddRange(response.Messages);
                else return null;
                request.PageToken = response.NextPageToken;
            }
            catch (Exception)
            {
                return null;
            }
        } while (!string.IsNullOrEmpty(request.PageToken));

        return result;
    }

    private static Message GetMessage(GmailService service, string userId, string messageId)
    {
        try
        {
            var request = service.Users.Messages.Get(userId, messageId);
            request.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Raw;
            return request.Execute();
        }
        catch (Exception)
        {
            return null;
        }
    }

}
