using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataModel.Dto;
using DataModel.Enities;
using NSubstitute;
using Security;
using Xunit;

namespace Business.UnitTests
{
    public class MessageServiceTest
    {
        private List<Message> _messages = new List<Message>();
        private readonly IMessageService _messageService;
        private readonly ISecurityContext _securityContext;
        private readonly IUnitOfWork _uow;

        public MessageServiceTest()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _uow.Query<Message>().Returns(info => _messages.AsQueryable());
            _securityContext = Substitute.For<ISecurityContext>();
            _securityContext.User.Returns(info => new User { Id = 1, Name = "testUser" });
            _messageService = new MessageService(_uow, _securityContext);
        }

        [Fact]
        public void AddUseUserIdFromSecurityContext()
        {
            var messageText = "text";
            var message = _messageService.Add(new MessageDto { Text = messageText });

            Assert.Equal(_securityContext.User.Id, message.UserId);
        }

        [Fact]
        public void AddSetTextFromDto()
        {
            var text = "text";
            var message = _messageService.Add(new MessageDto { Text = text });

            Assert.Equal(text, message.Text);
        }

        [Fact]
        public void AddCallUowSaveMethods()
        {
            var message = _messageService.Add(new MessageDto());

            _uow.Received().Add(message);
            _uow.Received().SaveChanges();
        }

        [Fact]
        public void GetIsOrderMessagesByCreationDate()
        {
            _messages = new List<Message>
            {
                new Message{Id = 1, CreationDateTime = new DateTime(2017, 08,18)},
                new Message{Id = 2, CreationDateTime = new DateTime(2017, 08, 19)}
            };

            var messages = _messageService.Get();

            Assert.Equal(2, messages.First().Id);
            Assert.Equal(1, messages.Last().Id);
        }
    }
}
