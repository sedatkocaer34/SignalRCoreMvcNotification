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
        public List<T> GetAll<T>(string key)
        {
            using (IRedisClient client = new RedisClient())
            {
                List<T> dataList = new List<T>();
                List<string> allKeys = client.SearchKeys(key);
                foreach (string keyvalue in allKeys)
                {
                    dataList.Add(client.Get<T>(keyvalue));
                }
                return dataList;
            }
        }

        public T GetById<T>(string key)
        {
            using (IRedisClient client = new RedisClient())
            {
                var redisdata = client.Get<T>(key);

                return redisdata;
            }
        }

        public void Remove(string key)
        {
            using (IRedisClient client = new RedisClient())
            {
                var redisdata = client.Remove(key);
            }
        }

        public void set<T>(string key, T valueObject, int expiration)
        {
            using (IRedisClient client = new RedisClient())
            {
                var redisdata = client.Set<T>(key,valueObject,DateTime.Now.AddMinutes(expiration));
            }
        }
    }
}
