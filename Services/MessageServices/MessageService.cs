using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MessageServices
{
    public interface IMessageService
    {
        Task<bool> CreateMessageAsync(string chatId, string message, string accId);
    }
    public class MessageService: IMessageService
    {
        private readonly IUnitOfWork _uow;
        public MessageService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<bool> CreateMessageAsync(string chatId, string message, string accId)
        {
            var mapping = await _uow.RoomMapping.FirstOfDefaultAsync(p => p.ChatRoomId == chatId && p.AccId == accId);
            if(mapping != null)
            {
                Message newMessage = new Message() {
                    Id = Guid.NewGuid().ToString(),
                    ChatRoomId = chatId,
                    CreatedDate = DateTime.Now,
                    Message1 = message,
                    Sender = accId,
                };
                await _uow.messageRepository.AddAsync(newMessage);
                await _uow.SaveAsync();
                return true;
            }else return false;
        }
    }
}
