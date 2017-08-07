using Newtonsoft.Json;
using SupportAgentCRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SupportAgentCRM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpPost]
        public ActionResult GetChat(string Token)
        {
            Configer.SetToken(Token);
            ViewBag.chat = "Ваш Токен Зарегистрирован, Первичная настройка для Chat2Deck завершена.";
            return View("Index");
        }
        [HttpPost]
        public ActionResult GetSecret()
        {
            ViewBag.text = "Ваш Токен Зарегистрирован, Первичная настройка для Gmail завершена.";
            return View("Index");
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/" + fileName));
                ViewBag.gmail = "Секрет успешно загружен.";
            }
            return View("Index");
        }
        [HttpPost]
        public bool SetTokens(string ChatHelpDesk, string Gmail)
        {
            if (ChatHelpDesk != null && ChatHelpDesk != "")
            { 
                string name = "ChHToken.json";
                string path = HostingEnvironment.ApplicationPhysicalPath;
                try
                {
                    using (FileStream fstream = new FileStream(path + name, FileMode.Create))
                    {
                        ChatHelpDeskToken data = new ChatHelpDeskToken
                        {
                            Token = ChatHelpDesk
                        };
                        string json = JsonConvert.SerializeObject(data);
                        // преобразуем строку в байты
                        byte[] array = System.Text.Encoding.Default.GetBytes(json);
                        // запись массива байтов в файл
                        fstream.Write(array, 0, array.Length);
                    }
                }
                catch (Exception ex)
                {
                    string ModelError = ex.Message;
                }
            }
            if (Gmail != null && Gmail != "")
            {
                string name = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user";
                string path = HostingEnvironment.ApplicationPhysicalPath;
                try
                {
                    using (FileStream fstream = new FileStream(path + name, FileMode.Create))
                    {
                        //string json = JsonConvert.SerializeObject(Gmail);
                        // преобразуем строку в байты
                        byte[] array = System.Text.Encoding.Default.GetBytes(Gmail);
                        // запись массива байтов в файл
                        fstream.Write(array, 0, array.Length);
                    }
                }
                catch (Exception ex)
                {
                    string ModelError = ex.Message;
                }
            }
            return true;
        }
    }
}
