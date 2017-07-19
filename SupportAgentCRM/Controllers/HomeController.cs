using System;
using System.Collections.Generic;
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
    }
}
