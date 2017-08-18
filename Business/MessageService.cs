using System;
using System.Linq;
using DataAccess;
using DataModel.Common;
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

        public Page<Message> GetMessages(PageSettings pageSettings)
        {
            var queryable = _context.Messages.AsQueryable();
            var total = queryable.Count();

            var messages = queryable
                .OrderBy(m => m.CreationDateTime)
                .Skip(pageSettings.Skip)
                .Take(pageSettings.Size);
            return new Page<Message> { Data = messages.ToList(), Total = total };
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