using System.Web.Mvc;
using Business;
using DataModel.Dto;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageService _messageService;

        public HomeController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public ActionResult Index()
        {
            var messages = _messageService.GetMessages();
            return View(messages);
        }

        public void AddMessage(MessageDto message)
        {
            _messageService.AddMessage(message);
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