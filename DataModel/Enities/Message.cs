using System;

namespace DataModel.Enities
{
    public class Message : Entity
    {
        public string Text { get; set; }

        public DateTime CreationDateTime { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}