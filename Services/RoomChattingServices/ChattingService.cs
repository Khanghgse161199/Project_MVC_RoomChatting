using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ChatRoom;

namespace Services.RoomChattingServices
{
    public interface IChattingService
    {
        Task<bool> CreateChatRoomAsync(CreateChatRoomViewModel info, string accId);
        Task<List<ChatRoom>> GetAllAsync(string accId);
        Task<ChatDetailViewModel> GetChatDetailAsync(string chatRoomId, string accId);
        Task<bool> joinRoomAsync(string chatId, string accId, string? secrect);
    }
    public class ChattingService: IChattingService
    {
        private readonly IUnitOfWork _uow;
        public ChattingService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<bool> joinRoomAsync(string chatId, string accId, string? secrect)
        {
            var chatRoom = await _uow.ChatRoom.FirstOfDefaultAsync(p => p.Id == chatId);
           if (chatRoom != null)
            {
                var mappingExist = await _uow.RoomMapping.FirstOfDefaultAsync(p => p.AccId == accId && p.ChatRoomId == chatId);
                if (mappingExist == null)
                {
                    if (chatRoom.IsPrivate)
                    {
                        if (chatRoom.Serect == secrect)
                        {
                            RoomUserMapping roomUserMapping = new RoomUserMapping()
                            {
                                AccId = accId,
                                ChatRoomId = chatId,
                                Id = Guid.NewGuid().ToString(),
                                IsActive = true
                            };

                            await _uow.RoomMapping.AddAsync(roomUserMapping);
                            await _uow.SaveAsync();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        RoomUserMapping roomUserMapping = new RoomUserMapping()
                        {
                            AccId = accId,
                            ChatRoomId = chatId,
                            Id = Guid.NewGuid().ToString(),
                            IsActive = true
                        };

                        await _uow.RoomMapping.AddAsync(roomUserMapping);
                        await _uow.SaveAsync();
                        return true;
                    }
                }
                else return false;          
            }else return false;
        }
        public async Task<ChatDetailViewModel> GetChatDetailAsync(string chatRoomId, string accId)
        {
            var mapping = await _uow.RoomMapping.FirstOfDefaultAsync(p => p.ChatRoomId == chatRoomId && p.AccId == accId);
            if (mapping != null)
            {
                var chatRoom = await _uow.ChatRoom.FirstOfDefaultAsync(p => p.Id == chatRoomId && p.IsActive);
                if (chatRoom != null)
                {
                    ChatDetailViewModel chatDetailViewModel = new ChatDetailViewModel()
                    {
                        ChatRoom = new ChatRoomViewModel()
                        {
                            Id = chatRoomId,
                            Admin = chatRoom.Admin,
                            IsPrivate = chatRoom.IsPrivate,
                            Name = chatRoom.Name,
                        },
                        messages = new List<MessageViewModel>()
                    };
                    var Messages = await _uow.messageRepository.GetAllAsync(p => p.ChatRoomId == chatRoomId, o => o.OrderBy(p => p.CreatedDate));
                    if (Messages != null && Messages.Count > 0)
                    {
                        foreach (var item in Messages)
                        {
                            var acc = await _uow.Accounts.FirstOfDefaultAsync(p => p.Id == accId && p.IsActive);
                            var message = new MessageViewModel()
                            {
                                Id = item.Id,
                                DateTime = item.CreatedDate,
                                Sender = item.Sender,
                                SenderName = acc.Email,
                                Messaget = item.Message1                               
                            };

                            if (!string.IsNullOrEmpty(item.ImgSetId)){
                                var img = await _uow.Images.FirstOfDefaultAsync(p => p.ImgSetId == item.ImgSetId);
                                message.ImgUrl = img.ImgUrl;
                            }
                            chatDetailViewModel.messages.Add(message);
                        }

                    }
                    return chatDetailViewModel;
                } else return null;

            }
            else return null;
        }

        public async Task<List<ChatRoom>> GetAllAsync(string accId)
        {
            var currentRoomMapping = await _uow.RoomMapping.GetAllAsync(p => p.AccId == accId && p.IsActive,null,"ChatRoom");
            if (currentRoomMapping != null)
            {
                List<ChatRoom> tmp = new List<ChatRoom>();
                foreach (var room in currentRoomMapping)
                {
                    tmp.Add(room.ChatRoom);
                }
                return tmp;
            }
            else return null;
        }

        public async Task<bool> CreateChatRoomAsync(CreateChatRoomViewModel info, string accId)
        {
            string chatRoomId = Guid.NewGuid().ToString();
            ChatRoom newChatRoom = new ChatRoom() { 
                Id = chatRoomId,
                Name = info.Name,
                Creator = accId,
                Admin = accId,
                IsActive = true,
                IsPrivate = info.IsPrivate,
                Serect = info.serectKey
            };
            await _uow.ChatRoom.AddAsync(newChatRoom);
            RoomUserMapping roomUserMapping = new RoomUserMapping() {
                Id = Guid.NewGuid().ToString(),
                AccId = accId,
                ChatRoomId = chatRoomId,
                IsActive = true
            };
            await _uow.RoomMapping.AddAsync(roomUserMapping);
            await _uow.SaveAsync();
            return true;
        }
    }
}