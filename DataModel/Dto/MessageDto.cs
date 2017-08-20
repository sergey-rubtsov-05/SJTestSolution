using System;
using System.Web.Mvc;

namespace DataModel.Dto
{
    public class MessageDto
    {
        public string Username { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        public DateTime CreationDateTime { get; set; }
    }
}