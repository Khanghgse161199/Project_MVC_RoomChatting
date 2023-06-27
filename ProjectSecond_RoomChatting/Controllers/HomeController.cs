using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.RoomChattingServices;
using Services.UserServices;
using ViewModels.ChatRoom;
using ViewModels.Home;

namespace ProjectSecond_RoomChatting.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IChattingService _chattingService;
        public HomeController(IUserService userService, IAuthService authService, IChattingService chattingService)
        {
            _userService = userService;
            _authService = authService;
            _chattingService = chattingService; 
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateChatRoomViewModel createChatRoomViewModel)
        {
            var session = HttpContext.Session;
            string token = session.GetString("TOKEN");
            if (!string.IsNullOrEmpty(token))
            {
                var checkToken = await _authService.checkTokenAsyc(token);
                if (checkToken != null)
                {
                    if (createChatRoomViewModel != null)
                    {
                        var isCreate = await _chattingService.CreateChatRoomAsync(createChatRoomViewModel, checkToken.accId);
                        if (isCreate)
                        {
                            return RedirectToAction("Index", new { success = "create chat success" });
                        }
                        else
                        {
                            return RedirectToAction("Index", new { error = "error when create" });
                        }
                    }
                    else return RedirectToAction("Index", new { error  = "error when create"});
                }else return RedirectToAction("Index", "Auth");
            }else return RedirectToAction("Index", "Auth");
        }

        public async Task<IActionResult> Index(string error, string success, string chatid)
        {
            var session = HttpContext.Session;
            string token = session.GetString("TOKEN");
            if (!string.IsNullOrEmpty(token))
            {
                var checkToken = await _authService.checkTokenAsyc(token);
                if (checkToken != null)
                {
                    if (!string.IsNullOrEmpty(chatid))
                    {
                        var isUpdateCountNotify = await _chattingService.UpdateCountNotify(chatid, checkToken.accId);
                    }
                    var profile = await _userService.GetProfileAsync(checkToken.accId);
                    IndexHomeVIewModel indexHomeVIewModel = new IndexHomeVIewModel() {
                        ProfileViewModel = profile,
                        chatRooms = await _chattingService.GetAllAsync(checkToken.accId)
                    };
                    if (!string.IsNullOrEmpty(error))
                    {
                        ViewBag.error = error;
                    }
                    if (!string.IsNullOrEmpty(success))
                    {
                        ViewBag.success = success;  
                    }
                    if(chatid != null)
                    {
                        var chatDetail = await _chattingService.GetChatDetailAsync(chatid, checkToken.accId);
                        ViewBag.CurrentChat = chatid;
                        indexHomeVIewModel.ChatDetailViewModel = chatDetail;
                    }
                    ViewBag.sender = checkToken.accId;
                    return View(indexHomeVIewModel);
                }
                return RedirectToAction("Index", "Auth");
            }
            else
            {
                return RedirectToAction("Index", "Auth");
            } 
        }
    }
}
