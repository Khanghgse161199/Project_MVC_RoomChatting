using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.RoomChattingServices;
using System.Threading.Tasks.Dataflow;
using ViewModels.ChatRoom;

namespace ProjectSecond_RoomChatting.Controllers
{
    public class ChatController : Controller
    {
        private readonly IAuthService _Auth;
        private readonly IChattingService _ChattingService;

        public ChatController(IAuthService auth, IChattingService chattingService)
        {
            _Auth = auth;
            _ChattingService = chattingService;
        }

        [HttpPost("JoinChatRoom")]
        public async Task<IActionResult> JoinChatRoom(JoinChatViewModel joinchatViewModel)
        {
            var session = HttpContext.Session;
            string token = session.GetString("TOKEN");
            if (!string.IsNullOrEmpty(token))
            {
                var checkToken = await _Auth.checkTokenAsyc(token);
                if (checkToken != null)
                {
                    var joined = await _ChattingService.joinRoomAsync(joinchatViewModel.chatId, checkToken.accId, joinchatViewModel.secrect);
                        if (joined)
                        {
                            return RedirectToAction("Index", "Home", new { success = "Join success" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { error = "Join Error" });
                        }
                }
                else return RedirectToAction("Login", "Auth", new { error = "Login Frist" });

            }
            else return RedirectToAction("Login", "Auth", new { error = "Login Frist" });

            }
      
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
