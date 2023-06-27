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
        Task<bool> DeleteMessageAsync(string idMessage, string accId, string currentRoomId);
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
            var mapping = await _uow.RoomMapping.FirstOfDefaultAsync(p => p.ChatRoomId == chatId && p.AccId == accId, "ChatRoom");
            if(mapping != null)
            {
                Message newMessage = new Message() {
                    Id = Guid.NewGuid().ToString(),
                    ChatRoomId = chatId,
                    CreatedDate = DateTime.Now,
                    Message1 = message,
                    Sender = accId,
                    IsMessageSystem = false
                };
                var roomMappings = await _uow.RoomMapping.GetAllAsync(p => p.ChatRoomId == mapping.ChatRoomId && p.IsActive);
                foreach (var item in roomMappings)
                {
                   if(item.AccId != accId)
                    {
                        item.CountNotify = item.CountNotify + 1;
                        _uow.RoomMapping.Udpate(item);
                    }
                }
                User user = await _uow.Users.FirstOfDefaultAsync(p => p.AccId == newMessage.Sender);
                mapping.ChatRoom.LastMessage = newMessage.Message1;
                mapping.ChatRoom.LastSenderMessage = user.Fullname;
                mapping.ChatRoom.LastedUpdate = DateTime.Now;   
                _uow.ChatRoom.Udpate(mapping.ChatRoom);
                await _uow.messageRepository.AddAsync(newMessage);
                await _uow.SaveAsync();
                return true;
            }else return false;
        }

        public async Task<bool> DeleteMessageAsync(string idMessage, string accId, string currentRoomId)
        {
            if (!string.IsNullOrEmpty(idMessage) && !string.IsNullOrEmpty(accId))
            {
                var currentMessage = await _uow.messageRepository.FirstOfDefaultAsync(p => p.Id == idMessage);
                if (currentMessage != null)
                {
                    if (currentMessage.Sender == accId && currentMessage.ChatRoomId == currentRoomId)
                    {
                        _uow.messageRepository.RemoveAsync(currentMessage.Id);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
    }
}
