using System.Collections.Generic;
using System.Web.Mvc;
using Business;
using DataModel.Dto;
using DataModel.Enities;
using WebApp.Engine.Security;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _userContext;
        private readonly IMessageService _messageService;

        public HomeController(UserContext userContext, IMessageService messageService)
        {
            _userContext = userContext;
            _messageService = messageService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetMessages()
        {
            var messages = _messageService.Get();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public void AddMessage(MessageDto message)
        {
            _messageService.AddMessage(message, _userContext.Username);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}