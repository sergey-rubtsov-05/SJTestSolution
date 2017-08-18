using System.Collections.Generic;
using DataModel.Dto;
using DataModel.Enities;

namespace Business
{
    public interface IMessageService
    {
        IList<Message> GetMessages();

        Message AddMessage(MessageDto message);
    }
}