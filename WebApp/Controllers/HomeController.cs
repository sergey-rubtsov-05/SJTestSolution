using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Business;
using DataModel.Dto;
using WebApp.Engine.Security;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _userContext;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public HomeController(UserContext userContext, IMessageService messageService, IMapper mapper)
        {
            _userContext = userContext;
            _messageService = messageService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetMessages()
        {
            var messages = _messageService.Get();
            var dtos = _mapper.Map<IList<MessageDto>>(messages);
            return Json(dtos, JsonRequestBehavior.AllowGet);
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