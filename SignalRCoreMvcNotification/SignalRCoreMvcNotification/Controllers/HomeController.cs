using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SignalRCoreMvcNotification.DataContext;
using SignalRCoreMvcNotification.Helpers;
using SignalRCoreMvcNotification.Models;
using SignalRCoreMvcNotification.Models.Domain;
using SignalRCoreMvcNotification.Models.ViewModels;
using SignalRCoreMvcNotification.Security;
using SignalRCoreMvcNotification.Security.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<UsersHub> _hubContext;
        private SignalRCoreDataContext _dataContext;
        private IPasswordHash _passwordHash;
        readonly IConfiguration _configuration;
        public HomeController(IHubContext<UsersHub> hubContext , SignalRCoreDataContext dataContext 
        ,IPasswordHash passwordHash, IConfiguration configuration)
        {
            _passwordHash = passwordHash;
            _hubContext = hubContext;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            userLoginViewModel.Submit = true;
            if(ModelState.IsValid)
            {
                //check username
                var checkUser = await _dataContext.User.Where(x => x.Username == userLoginViewModel.Username).FirstOrDefaultAsync();
                if (checkUser!=null)
                {
                    //check password
                    var checkPassword =  _passwordHash.VerifHash(userLoginViewModel.Password, checkUser.Password);
                    if (checkPassword)
                    {
                        SetUserNamePrincipal(checkUser.Username);
                        TokenHandler tokenHandler = new TokenHandler(_configuration);
                        string token = tokenHandler.CreateAccessToken(checkUser.Id,5);

                        Response.Cookies.Append("token", token,
                        new CookieOptions { HttpOnly = true });

                        return RedirectToAction(ActionNameHelper.HomeIndex);
                    }
                    userLoginViewModel.IsSuccess = false;
                    userLoginViewModel.Message = "Username Or Password Wrong !";
                }
                userLoginViewModel.IsSuccess = false;
                userLoginViewModel.Message = "Username Or Password Wrong !";
            }

            return View(userLoginViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            userRegisterViewModel.Submit = true;
            if (ModelState.IsValid)
            {
                if (userRegisterViewModel.Password!=userRegisterViewModel.PasswordMatch)
                {
                    userRegisterViewModel.IsSuccess = false;
                    userRegisterViewModel.Message = "Password and Password confirm not match !";
                    return View(userRegisterViewModel);
                }
                var checkUser = await _dataContext.User.FirstOrDefaultAsync(x=>x.Username== userRegisterViewModel.Username) 
                    ?? await _dataContext.User.FirstOrDefaultAsync(x => x.Email == userRegisterViewModel.Email);
                if (checkUser == null)
                {
                    var hashPassword = _passwordHash.Hash(userRegisterViewModel.Password);
                    try
                    {
                        var addUser = await _dataContext.User.AddAsync(new User()
                        {
                            Email = userRegisterViewModel.Email,
                            Username = userRegisterViewModel.Username,
                            Password = hashPassword
                        });
                        _dataContext.SaveChanges();
                        return RedirectToAction(ActionNameHelper.HomeLogin);
                    }
                    catch (Exception ex)
                    {
                        userRegisterViewModel.IsSuccess = false;
                        userRegisterViewModel.Message = "Something Went Wrong";
                    }
                }
                else
                {
                    userRegisterViewModel.IsSuccess = false;

                    if (userRegisterViewModel.Username == checkUser.Username)
                        userRegisterViewModel.Message = "Username Already Exists!";
                    else
                        userRegisterViewModel.Message = "Email Already Exists";
                }
            }
            return View(userRegisterViewModel);
        }

        [Authorize]
        [HttpPost]
        public JsonResult CreateNotification([FromBody] NotificationViewModel notificationViewModel)
        {
            if (ModelState.IsValid)
            {
                notificationViewModel.NotifyType = "info";
                _dataContext.Notifications.Add(new Notifications()
                {
                    CreatedDate=DateTime.Now,
                    NotificationName= notificationViewModel.NotificationName,
                    NotificationDescription=notificationViewModel.NotificationDescription,
                    UserId = ((UserContextModel)HttpContext.Items["User"]).Id
                });
                _dataContext.SaveChanges();
                _hubContext.Clients.All.SendAsync("notifyMessage", notificationViewModel);
                return Json(new { status = true, message = "Notify Inserted." });
            }
            return Json(new { status = false, message = "Please fill all input" });
        }

        public void SetUserNamePrincipal(string Username)
        {
            var identity = new ClaimsIdentity(new[]
            { new Claim(ClaimTypes.Name, Username)},
            CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(ActionNameHelper.HomeLogin);
        }
    }
}
