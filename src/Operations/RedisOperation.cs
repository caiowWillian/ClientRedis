using ClientRedis.Conect;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace ClientRedis.Operations
{
    public class RedisOperation<T> 
        where T : class
    {
        protected static IDatabase _database;

        /// <summary>
        /// Adiciona um dado no redis
        /// </summary>
        /// <param name="obj">objeto que será armazendo</param>
        /// <param name="key"></param>
        public static void Add(T obj, string key)
        {
            var cache = ConectRedis.Connection.GetDatabase();
            GetDatabase().StringSet(key, JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// Adiciona um dado no redis
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="keys"></param>
        public static void Add(T obj, IList<string> keys)
        {
            var cache = ConectRedis.Connection.GetDatabase();

            foreach(var key in keys)
            {
                GetDatabase().StringSet(key, JsonConvert.SerializeObject(obj));
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