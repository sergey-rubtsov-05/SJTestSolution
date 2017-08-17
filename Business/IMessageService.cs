using DataAccess.Dto;
using DataModel.Common;
using DataModel.Enities;

namespace Business
{
    public interface IMessageService
    {
        Page<Message> GetMessages(PageSettings pageSettings);

        Message AddMessage(MessageDto message);
    }
}