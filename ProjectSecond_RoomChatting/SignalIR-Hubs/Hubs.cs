
using DataService.Repositories.UnitOfWork;
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
                    List<string> tmp = new List<string>();
                    string group = "noName";
                    foreach (var item in currentRoomChat.RoomUserMappings)
                    {
                        var connectionId = await _redisService.GetConnectionIdAsync(item.AccId);
                        await Groups.AddToGroupAsync(connectionId, group);
                    }
                    await Clients.Group(group).SendAsync("ReciveMessage", message, email, chatId, createDate);
                    group = new string("noName");
                }
                else await Clients.Caller.SendAsync("ReciveMessage", message, email, chatId, createDate);
            }
            else
            {
                await Clients.Caller.SendAsync("ReciveMessage", message, email, chatId, createDate);
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
