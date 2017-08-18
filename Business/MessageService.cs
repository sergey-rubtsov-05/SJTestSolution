using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataModel.Dto;
using DataModel.Enities;

namespace Business
{
    public class MessageService : IMessageService
    {
        private readonly SjContext _context;

        public MessageService(SjContext context)
        {
            _context = context;
        }

        public IList<Message> GetMessages()
        {
            var queryable = _context.Messages.AsQueryable();
            var messages = queryable
                .OrderByDescending(m => m.CreationDateTime);
            return messages.ToList();
        }

        public Message AddMessage(MessageDto message)
        {
            var user = _context.Users.SingleOrDefault(u => u.Name == message.Username) ??
                       new User { Name = message.Username };

            var newMessage = new Message
            {
                Text = message.Text,
                User = user,
                CreationDateTime = DateTime.Now
            };
            _context.Messages.Add(newMessage);
            _context.SaveChanges();
            return newMessage;
        }
    }
}