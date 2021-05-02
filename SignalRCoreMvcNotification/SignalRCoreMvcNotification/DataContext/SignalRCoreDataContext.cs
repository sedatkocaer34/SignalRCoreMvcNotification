using Microsoft.EntityFrameworkCore;
using SignalRCoreMvcNotification.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.DataContext
{
    public class SignalRCoreDataContext:DbContext
    {
        public SignalRCoreDataContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<User> User { get; set; }
    }
}
