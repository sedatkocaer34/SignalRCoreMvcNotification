using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalRCoreMvcNotification.DataContext;
using SignalRCoreMvcNotification.Models;
using SignalRCoreMvcNotification.Models.Domain;
using SignalRCoreMvcNotification.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public HomeController(IHubContext<UsersHub> hubContext , SignalRCoreDataContext dataContext 
        ,IPasswordHash passwordHash)
        {
            _passwordHash = passwordHash;
            _hubContext = hubContext;
            _dataContext = dataContext;
        }

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
                var checkUser = await _dataContext.User.Where(x => x.Email == userLoginViewModel.Username).FirstOrDefaultAsync();
                if (checkUser!=null)
                {
                    //check password
                    var checkPassword =  _passwordHash.VerifHash(userLoginViewModel.Password, checkUser.Password);
                    if (checkPassword)
                    {
                        SetUserNamePrincipal(checkUser.Username);
                        return RedirectToAction("Index");
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
                        return RedirectToAction("Login");
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

        public void SetUserNamePrincipal(string Username)
        {
            var identity = new ClaimsIdentity(new[]
            { new Claim(ClaimTypes.Name, Username)},
            CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
