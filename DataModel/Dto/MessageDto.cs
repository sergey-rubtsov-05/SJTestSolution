using System;

namespace DataModel.Dto
{
    public class MessageDto
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}