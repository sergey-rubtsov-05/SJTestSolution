using System.Web.Mvc;
using Business;
using DataModel.Dto;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SessionContext _sessionContext;
        private readonly IMessageService _messageService;

        public HomeController(SessionContext sessionContext, IMessageService messageService)
        {
            _sessionContext = sessionContext;
            _messageService = messageService;
        }

        public ActionResult Index()
        {
            var messages = _messageService.GetMessages();
            return View(messages);
        }

        [Authorize]
        public void AddMessage(MessageDto message)
        {
            message.Username = _sessionContext.Username;
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