using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Models
{
    public class BaseViewModel
    {
        public bool Submit { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
