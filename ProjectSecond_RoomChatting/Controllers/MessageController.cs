using DataServices.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.MessageServices;
using ViewModels.Home;

namespace ProjectSecond_RoomChatting.Controllers
{
    public class MessageController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMessageService _Mess;
        public MessageController(IAuthService authService, IMessageService messageService)
        {
            _authService = authService;
            _Mess = messageService;
        }

        [HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage(IndexHomeVIewModel info)
        {
            if (info.CreateMessageViewModel != null)
            {
                var session = HttpContext.Session;
                string token = session.GetString("TOKEN");
                if (!string.IsNullOrEmpty(token))
                {
                    var checkToken = await _authService.checkTokenAsyc(token);
                    if (checkToken != null)
                    {
                        if (info.CreateMessageViewModel != null)
                        {
                            var created = await _Mess.CreateMessageAsync(info.CreateMessageViewModel.chatId, info.CreateMessageViewModel.Message, checkToken.accId);
                            if (created)
                            {
                                //share id
                                return RedirectToAction("Index", "Home", new { chatId = info.CreateMessageViewModel.chatId });
                            }
                            else return RedirectToAction("Index", "Home", new { error = "Error when create Message" });
                        }
                        else return RedirectToAction("Index", "Home", new { error = "Null Message" });
                    }
                    else return RedirectToAction("Login", "Auth", new { error = "Login First" });
                }
                else return RedirectToAction("Login", "Auth", new { error = "Login First" });
            }
            else return RedirectToAction("Index", "Home", new { error = "Null Message" });
        }

        public async Task<IActionResult> DeleteMessage(string messageId, string currentChatRoom)
        {
            if (!string.IsNullOrEmpty(messageId) && !string.IsNullOrEmpty(currentChatRoom))
            {
                var session = HttpContext.Session;
                string token = session.GetString("TOKEN");
                var checkToken = await _authService.checkTokenAsyc(token);
                if (checkToken != null)
                {
                    var isDelete = await _Mess.DeleteMessageAsync(messageId, checkToken.accId, currentChatRoom);
                    if (isDelete)
                    {
                        return RedirectToAction("Index", "Home", new { chatId = currentChatRoom });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { error = "Error when delete message" });
                    }
                }
                else return RedirectToAction("Login", "Auth", new { error = "Login-first" });
            }
            else return RedirectToAction("Index", "Home", new { error = "Null ID-Message" });
        }
    }
}
