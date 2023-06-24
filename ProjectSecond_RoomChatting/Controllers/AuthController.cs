using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AccountServices;
using Services.AuthServices;
using Services.UserServices;
using Utilities.FileHelper;
using ViewModels.Auth;

namespace ProjectSecond_RoomChatting.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public AuthController(IAccountService accountService, IUserService userService, IAuthService authService)
        {
            _accountService = accountService;
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginViewModel info)
        {
            if(!string.IsNullOrEmpty(info.email) && !string.IsNullOrEmpty(info.password))
            {
                var AccessToken = await _authService.LoginAsync(info.email, info.password);
                if (AccessToken != null)
                {
                    var session = HttpContext.Session;
                    session.SetString("TOKEN", AccessToken.AccessToken);
                    return RedirectToAction("Index","Home");
                }
                else return RedirectToAction(nameof(Index), new { error = "error in login" });
            }
            else return RedirectToAction(nameof(Index), new {error = "Plase input email or password!"});
        }

        [HttpPost("signup")]
        public async Task<IActionResult> signup(SignUpViewModel info)
        {
            string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };
            if (ModelState.IsValid)
            {
                if(info.password == info.confirmPassword)
                {

                using (var memoryStream = new MemoryStream())
                {
                    // copy bytes to memory stream
                    await info.file.CopyToAsync(memoryStream);
                    // check file extension and signature
                    // Check the content length in case the file's only
                    // content was a BOM and the content is actually
                    // empty after removing the BOM.
                    if (memoryStream.Length == 0)
                    {
                        return RedirectToAction("SignUp", new { error = "Lỗi!! file không chứa thông tin nào" });
                    }
                    if (!FileHelper.IsValidFileExtensionAndSignature(info.file.FileName, memoryStream, permittedExtensions))
                    {
                        return RedirectToAction("SignUp", new { error = "Lỗi!! Chỉ được upload file hình như png, jpg, jpeg" });
                    }
                }

                //check file size
                if (info.file.Length > 2097152)
                {
                    return RedirectToAction("SignUp", new { error = "Lỗi!! Chỉ được upload file nhỏ hơn 2MB" });
                }
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/UserAvatars");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //change file name

                var ext = Path.GetExtension(info.file.FileName).ToLowerInvariant();
                var fileName = Path.GetRandomFileName();
                fileName = Path.ChangeExtension(fileName, ext);
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    info.file.CopyTo(stream);
                }
                string imageUrl = "Images/UserAvatars/" + fileName;
                string AccountId = Guid.NewGuid().ToString();
                var isCreateAcc = await _accountService.CreateAccountAsync(info.email, info.password, AccountId);
                if (isCreateAcc)
                {
                    var userCreate = await _userService.CreateUserAsync(AccountId, info.fullname, info.birthDay, imageUrl);
                        if (userCreate)
                        {
                            return RedirectToAction("Index", new { success = "welcome!!!" });
                        }
                        else
                        {
                            return RedirectToAction("SignUp", new { error = "Lỗi!! tạo tài khoản" });
                        }
                }
                else
                {
                    return RedirectToAction("SignUp", new { error = "Lỗi!! tạo tài khoản" });

                }
                }
                else
                {
                    return RedirectToAction("SignUp", new { error = "Lỗi!! mật khẩu không trùng nhau" });
                }
            }
            else
            {
                return RedirectToAction("SignUp", new { error = "Lỗi!! mật khẩu không trùng nhau" });
            }
        }
        public async Task<IActionResult> SignUp(string error, string success)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.error = error;  
            }
            if (!string.IsNullOrEmpty(success))
            {
                ViewBag.success = success;
            }
            return View();
        }
        public async Task<IActionResult> Index(string error, string success)
        {
            var session = HttpContext.Session;
            var token = session.GetString("token");
            if(token == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }           
        }    
    }
}
