using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCoreMvcNotification.Redis
{
    public class RedisManager : IRedisService
    {
        public bool Any(string key)
        {
            using (IRedisClient client = new RedisClient())
            {
               return client.ContainsKey(key);
            }
        }

       

        public T Get<T>(string key)
        {
            using (IRedisClient client = new RedisClient())
            {
                if(Any(key))
                {
                    string jsondata = client.GetValue(key);
                    return JsonConvert.DeserializeObject<T>(jsondata);
                }
                return default;
            }
        }

        public void Remove(string key)
        {
            using (IRedisClient client = new RedisClient())
            {
                var redisdata = client.Remove(key);
            }
        }

        public void set<T>(string key, T valueObject)
        {
            using (IRedisClient client = new RedisClient())
            {
                string jsonData = JsonConvert.SerializeObject(valueObject);
                client.SetValue(key, jsonData,TimeSpan.FromMinutes(60));
            }
        }
    }
}
