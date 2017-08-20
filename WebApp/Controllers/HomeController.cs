using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Business;
using DataModel.Dto;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public HomeController(IMessageService messageService, IMapper mapper)
        {
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
            message.Text = Server.HtmlEncode(message.Text);
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