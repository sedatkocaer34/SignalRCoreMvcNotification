using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Redis
{
    public interface IRedisService
    {
        void set<T>(string key, T valueObject, int expiration);

        T GetById<T>(string key);

        List<T> GetAll<T>(string key);

        void Remove(string key);
    }
}
