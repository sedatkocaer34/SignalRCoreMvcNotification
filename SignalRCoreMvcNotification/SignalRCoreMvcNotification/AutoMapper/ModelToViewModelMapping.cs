using AutoMapper;
using SignalRCoreMvcNotification.Models.Domain;
using SignalRCoreMvcNotification.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.AutoMapper
{
    public class ModelToViewModelMapping:Profile
    {
        public ModelToViewModelMapping()
        {
            CreateMap<Notifications, NotificationViewModel>();
        }
    }
}
