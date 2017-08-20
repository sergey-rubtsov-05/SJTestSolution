using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataModel.Dto;
using DataModel.Enities;
using Security;

namespace Business
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _uow;
        private readonly ISecurityContext _securityContext;

        public MessageService(IUnitOfWork uow, ISecurityContext securityContext)
        {
            _uow = uow;
            _securityContext = securityContext;
        }

        public IList<Message> Get()
        {
            var messages = GetQuery()
                .OrderByDescending(m => m.CreationDateTime);
            return messages.ToList();
        }

        public Message Add(MessageDto message)
        {
            var newMessage = new Message
            {
                Text = message.Text,
                UserId = _securityContext.User.Id,
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