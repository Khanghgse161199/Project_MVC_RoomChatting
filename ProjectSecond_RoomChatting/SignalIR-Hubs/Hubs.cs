
using DataService.Repositories.UnitOfWork;
using DataServices.Entities;
using Microsoft.AspNetCore.SignalR;
using Services.RedisServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProjectSecond_RoomChatting.wwwroot.SignalIR_Hubs
{
    public class Hubs : Hub
    {
        private readonly IRedisService _redisService;
        private readonly IUnitOfWork _uow;
        public Hubs(IRedisService redisService, IUnitOfWork unitOfWork)
        {
            _redisService = redisService;
            _uow = unitOfWork;
        }
        public async Task SendAll(string message, string email, string chatId, string createDate)
        {
            if (!string.IsNullOrEmpty(chatId))
            {
                var currentRoomChat = await _uow.ChatRoom.FirstOfDefaultAsync(p =>p.Id == chatId && p.IsActive && p.Admin != null, "RoomUserMappings");
                if (currentRoomChat.RoomUserMappings != null && currentRoomChat.RoomUserMappings.Count > 0)
                {
                    string group = "GroupConnection";
                    foreach (var item in currentRoomChat.RoomUserMappings)
                    {
                        if (item.IsActive)
                        {
                            var connectionId = await _redisService.GetConnectionIdAsync(item.AccId);
                            await Groups.AddToGroupAsync(connectionId, group);
                        }
                    }
                    await Clients.Group(group).SendAsync("ReciveMessage", message, email, chatId, createDate);
                    group = new string("GroupConnection");
                }
                else await Clients.Caller.SendAsync("ReciveMessage", message, email, chatId, createDate);
            }
            else
            {
                await Clients.Caller.SendAsync("ReciveMessage", message, email, chatId, createDate);
            }
        }

        public async Task DeleteAll(string chatId, string idMessage)
        {
            if (!string.IsNullOrEmpty(chatId))
            {
                var currentRoomChat = await _uow.ChatRoom.FirstOfDefaultAsync(p => p.Id == chatId && p.IsActive && p.Admin != null, "RoomUserMappings");
                if (currentRoomChat.RoomUserMappings != null && currentRoomChat.RoomUserMappings.Count > 0)
                {
                    string group = "GroupConnection";
                    foreach (var item in currentRoomChat.RoomUserMappings)
                    {
                        if (item.IsActive)
                        {
                            var connectionId = await _redisService.GetConnectionIdAsync(item.AccId);
                            await Groups.AddToGroupAsync(connectionId, group);
                        }
                    }
                    await Clients.Group(group).SendAsync("DeleteMessage", idMessage);
                    group = new string("GroupConnection");
                }
                else await Clients.Caller.SendAsync("DeleteMessage", idMessage);
            }
            else
            {
                await Clients.Caller.SendAsync("DeleteMessage", idMessage);
            }
        }

        public async Task GetOutRoom(string chatId, string accId)
        {
                if (!string.IsNullOrEmpty(chatId) && !string.IsNullOrEmpty(accId))
                {
                    var currentRoomChat = await _uow.ChatRoom.FirstOfDefaultAsync(p => p.Id == chatId && p.IsActive && p.Admin != null, "RoomUserMappings");
                    if (currentRoomChat.RoomUserMappings != null && currentRoomChat.RoomUserMappings.Count > 0)
                    {
                        var currentUser = await _uow.Users.FirstOfDefaultAsync(p => p.AccId == accId);
                        string group = "GroupConnection";
                        foreach (var item in currentRoomChat.RoomUserMappings)
                        {
                            if (item.IsActive)
                            {
                                var connectionId = await _redisService.GetConnectionIdAsync(item.AccId);
                                await Groups.AddToGroupAsync(connectionId, group);
                            }
                        }
                        await Clients.Group(group).SendAsync("MemberGetOut", currentUser.Fullname, DateTime.Now.ToString("dd/MM/yy hh:mm"));
                        group = new string("GroupConnection");
                    }         
                }                   
        }

        public async Task SaveConnectId(string accId)
        {
            var connectedID = Context.ConnectionId;
            await _redisService.SaveConectionAsync(accId, connectedID);
        }

        //public async Task<string> GetConnectionId(string accId)
        //{
        //    string idConnection = await _redisService.GetConnectionIdAsync(accId);
        //    return idConnection;
        //}

        //public async Task<string> GetSavedConnected(string accId)
        //{
        //    var result = await _redisService.get
        //}
    }
}
