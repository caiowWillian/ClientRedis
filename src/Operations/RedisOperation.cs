using ClientRedis.Conect;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace ClientRedis.Operations
{
    public class RedisOperation<T> 
        where T : class
    {
        private static IDatabase _database;

        /// <summary>
        /// Adiciona um dado no redis
        /// </summary>
        /// <param name="obj">objeto que será armazendo</param>
        /// <param name="key"></param>
        public static void Add(T obj, string key, TimeSpan? expiry = null)
        {
            if(expiry == null)
            {
                GetDatabase().StringSet(key, JsonConvert.SerializeObject(obj));

                return;
            }

            GetDatabase().StringSet(key, JsonConvert.SerializeObject(obj), expiry);
        }

        /// <summary>
        /// Adiciona um dado no redis
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="keys"></param>
        public static void Add(T obj, IList<string> keys, TimeSpan? expiry = null)
        {
            foreach(var key in keys)
            {
                Add(obj, key, expiry);
            }
        }

        /// <summary>
        /// Obtém um objeto salvo no redis
        /// </summary>
        /// <param name="key">chave do registro</param>
        /// <returns></returns>
        public static T Get(string key)
        {
            return JsonConvert.DeserializeObject<T>(GetDatabase().StringGet(key));
        }

        /// <summary>
        /// Obtém o redis
        /// </summary>
        /// <returns></returns>
        protected static IDatabase GetDatabase()
        {
            if(_database != null)
            {
                return _database;
            }

            _database = ConectRedis.Connection.GetDatabase();

            return _database;
        }
    }
}