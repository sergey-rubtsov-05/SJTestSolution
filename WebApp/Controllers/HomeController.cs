using System.Web.Mvc;
using Business;
using DataModel.Dto;
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
            var messages = _messageService.GetMessages();
            return View(messages);
        }

        [Authorize]
        public void AddMessage(MessageDto message)
        {
            message.Username = _userContext.Username;
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