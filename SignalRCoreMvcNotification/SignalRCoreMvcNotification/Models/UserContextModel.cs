using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Models
{
    public class UserContextModel
    {
        public UserContextModel(int ıd, string userName)
        {
            Id = ıd;
            UserName = userName;
        }

        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
