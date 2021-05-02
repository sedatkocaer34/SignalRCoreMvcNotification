using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalRCoreMvcNotification.DataContext;
using SignalRCoreMvcNotification.Models;
using SignalRCoreMvcNotification.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            return View(new UserLoginViewModel());
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
                    var checkPassword = await _passwordHash.VerifHash(userLoginViewModel.Password, checkUser.Password);
                    if (checkPassword)
                    {
                        return RedirectToAction("Index");
                    }
                    userLoginViewModel.IsSuccess = false;
                    userLoginViewModel.Message = "Username Or Password Wrong !";
                }
                userLoginViewModel.IsSuccess = false;
                userLoginViewModel.Message = "Username Not Found !";
            }

            return View(userLoginViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new UserRegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(UserRegisterViewModel userRegisterViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View(userRegisterViewModel);
        }
    }
}
