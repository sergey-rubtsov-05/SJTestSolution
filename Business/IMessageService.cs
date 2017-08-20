using System.Collections.Generic;
using DataModel.Dto;
using DataModel.Enities;

namespace Business
{
    public interface IMessageService
    {
        IList<Message> Get();

        Message AddMessage(MessageDto message);
    }
}