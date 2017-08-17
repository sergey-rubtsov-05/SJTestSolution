using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Dto;
using DataModel.Common;
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
            throw new NotImplementedException();
        }
    }
}
