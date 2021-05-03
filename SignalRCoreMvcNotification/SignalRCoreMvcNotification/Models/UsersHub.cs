using Microsoft.AspNetCore.SignalR;
using SignalRCoreMvcNotification.Models.ViewModels;
using SignalRCoreMvcNotification.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Models
{
    public class UsersHub : Hub
    {
        private IRedisService _redisService;
        public UsersHub(IRedisService redisService)
        {
            _redisService = redisService;
        }
        public override Task OnConnectedAsync()
        {
            var currentUser = Context.User;

            Clients.All.SendAsync("connectuser", currentUser.Identity.Name);
            _redisService.set<OnlineUserViewModel>("onlineuser", new OnlineUserViewModel() { Id = 1, Name = currentUser.Identity.Name }, 60);

            return base.OnConnectedAsync();
        }

    }
}
