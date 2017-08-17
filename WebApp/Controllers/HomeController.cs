using System.Web.Mvc;
using Business;
using DataModel.Common;

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
            var messages = _messageService.GetMessages(new PageSettings(1, 5));
            return View(messages);
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