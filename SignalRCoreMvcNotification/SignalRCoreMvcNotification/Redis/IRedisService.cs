using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Redis
{
    public interface IRedisService
    {
        void set<T>(string key, T valueObject);

        T Get<T>(string key);

      
        void Remove(string key);

        bool Any(string key);
    }
}
