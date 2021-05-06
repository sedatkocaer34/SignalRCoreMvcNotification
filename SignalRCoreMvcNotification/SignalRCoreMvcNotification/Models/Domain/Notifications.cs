using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Models.Domain
{
    public class Notifications
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NotificationName { get; set; }
        public string NotificationDescription { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
