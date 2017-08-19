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
        private readonly IUnitOfWork _uow;

        public MessageService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IList<Message> Get()
        {
            var messages = GetQuery()
                .OrderByDescending(m => m.CreationDateTime);
            return messages.ToList();
        }

        public Message AddMessage(MessageDto message)
        {
            var user = _uow.Query<User>().SingleOrDefault(u => u.Name == message.Username) ??
                       new User { Name = message.Username };

            var newMessage = new Message
            {
                Text = message.Text,
                User = user,
                CreationDateTime = DateTime.Now
            };
            _uow.Add(newMessage);
            _uow.SaveChanges();
            return newMessage;
        }

        private IQueryable<Message> GetQuery()
        {
            return _uow.Query<Message>();
        }
    }
}