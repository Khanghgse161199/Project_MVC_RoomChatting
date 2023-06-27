using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using Microsoft.AspNetCore.Localization;
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
        Task<List<ChatRecentViewModel>> GetAllAsync(string accId);
        Task<ChatDetailViewModel> GetChatDetailAsync(string chatRoomId, string accId);
        Task<bool> joinRoomAsync(string chatId, string accId, string? secrect);
        Task<bool> UpdateCountNotify(string chatRoomId, string accId);
        Task<bool> getOutRoomChatAsync(string idRoom, string accId);
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
                var mappingExist = await _uow.RoomMapping.FirstOfDefaultAsync(p => p.AccId == accId && p.ChatRoomId == chatId && p.IsActive);
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
                                IsActive = true,
                                CountNotify = 0,
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
                                Messaget = item.Message1,
                                IsMessageSystem = item.IsMessageSystem,
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

        public async Task<List<ChatRecentViewModel>> GetAllAsync(string accId)
        {
            var currentRoomMapping = await _uow.RoomMapping.GetAllAsync(p => p.AccId == accId && p.IsActive,null, "ChatRoom,ChatRoom.Messages");
            var newCurrentRoomMapping = currentRoomMapping.AsQueryable();
            var listSort = newCurrentRoomMapping.OrderByDescending(p => p.ChatRoom.LastedUpdate);
            if (currentRoomMapping != null && currentRoomMapping.Count > 0)
            {
                List<ChatRecentViewModel> tmp = new List<ChatRecentViewModel>();
                foreach (var room in listSort)
                {
                    if(room != null && room.ChatRoom != null)
                    {
                        if(room.ChatRoom.Messages != null && room.ChatRoom.Messages.Count > 0)
                        {
                            tmp.Add(new ChatRecentViewModel
                            {
                                Id = room.ChatRoom.Id,
                                Name = room.ChatRoom.Name,
                                CountNotify = room.CountNotify,
                                TimeSend = room.ChatRoom.LastedUpdate,
                                LastMessage = room.ChatRoom.LastMessage,
                                SenderLastMessage = room.ChatRoom.LastSenderMessage,
                            });
                        }
                        else
                        {
                            tmp.Add(new ChatRecentViewModel
                            {
                                Id = room.ChatRoom.Id,
                                Name = room.ChatRoom.Name,
                                CountNotify = room.CountNotify,
                                TimeSend = default(DateTime),
                                LastMessage = null,
                                SenderLastMessage = null,
                            });
                        }
                    }
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
                Serect = info.serectKey,
                LastedUpdate = DateTime.Now,
            };
            await _uow.ChatRoom.AddAsync(newChatRoom);
            await _uow.SaveAsync();
            RoomUserMapping roomUserMapping = new RoomUserMapping() {
                Id = Guid.NewGuid().ToString(),
                AccId = accId,
                ChatRoomId = chatRoomId,
                IsActive = true,
                CountNotify = 0,
            };
            await _uow.RoomMapping.AddAsync(roomUserMapping);
            await _uow.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateCountNotify(string chatRoomId, string accId)
        {
            if (!string.IsNullOrEmpty(chatRoomId) && !string.IsNullOrEmpty(accId))
            {
                var currentRoomMapping = await _uow.RoomMapping.FirstOfDefaultAsync(p => p.AccId == accId && p.ChatRoomId == chatRoomId && p.IsActive);
                if (currentRoomMapping != null)
                {
                    currentRoomMapping.CountNotify = 0;
                    _uow.RoomMapping.Udpate(currentRoomMapping);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> getOutRoomChatAsync(string idRoom, string accId)
        {
            if (!string.IsNullOrEmpty(idRoom) && !string.IsNullOrEmpty(accId))
            {
                var currentRoomChat = await _uow.ChatRoom.FirstOfDefaultAsync(p => p.Id == idRoom && p.IsActive && p.Admin != null, "RoomUserMappings");
                if (currentRoomChat.RoomUserMappings != null && currentRoomChat.RoomUserMappings.Count > 0)
                {
                    var currentUser = await _uow.Users.FirstOfDefaultAsync(p => p.AccId == accId, "Acc,Acc.RoomUserMappings");
                    if (currentUser != null && currentUser.Acc.RoomUserMappings.Count > 0)
                    {
                        var currentRoomMapping = currentUser.Acc.RoomUserMappings.Where(p => p.AccId == accId && p.ChatRoomId == idRoom && p.IsActive).FirstOrDefault();
                        currentRoomMapping.IsActive = false;
                       
                        _uow.RoomMapping.Udpate(currentRoomMapping);
                        var newMessage = new Message()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ChatRoomId = idRoom,
                            Message1 = currentUser.Fullname + " " + "has left the chat room 💁‍→",
                            CreatedDate = DateTime.Now,
                            Sender = currentUser.AccId,
                            ImgSetId = null,
                            IsMessageSystem = true
                        };
                        await _uow.messageRepository.AddAsync(newMessage);
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