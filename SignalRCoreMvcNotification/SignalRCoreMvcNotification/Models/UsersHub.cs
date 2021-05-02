using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Models
{
    public class UsersHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine("User Connected => Connection Id ",Context.ConnectionId);

            return base.OnConnectedAsync();
        }
    }
}
