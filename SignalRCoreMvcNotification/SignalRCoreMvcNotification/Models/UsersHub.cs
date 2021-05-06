using Microsoft.AspNetCore.SignalR;
using SignalRCoreMvcNotification.Models.ViewModels;
using SignalRCoreMvcNotification.Redis;
using SignalRCoreMvcNotification.Security.Jwt;
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

        [Authorize]
        public override Task OnConnectedAsync()
        {
            var currentUser = Context.User;
            var onlineuser = _redisService.Get<List<OnlineUserViewModel>>("onlineuser");
            if (onlineuser==null)
            {
                onlineuser = new List<OnlineUserViewModel>();
            }

            onlineuser.Add(new OnlineUserViewModel() { ContextId = Context.ConnectionId,Name=currentUser.Identity.Name });
            _redisService.set<List<OnlineUserViewModel>>("onlineuser",onlineuser);

            Clients.All.SendAsync("connectuserlist", onlineuser);
            return base.OnConnectedAsync();
        }

        [Authorize]
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var onlineuser = _redisService.Get<List<OnlineUserViewModel>>("onlineuser");
            if (onlineuser != null)
            {
                onlineuser.Remove(onlineuser.FirstOrDefault(x => x.ContextId == Context.ConnectionId));
                _redisService.set<List<OnlineUserViewModel>>("onlineuser", onlineuser);
                Clients.All.SendAsync("connectuserlist", onlineuser);
            }
            return base.OnDisconnectedAsync(exception);
        }

    }
}
