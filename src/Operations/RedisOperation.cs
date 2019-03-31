using ClientRedis.Conect;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <param name="expiry">tempo de expiração</param>
        public static void Add(T obj, string key, TimeSpan? expiry = null)
        {
            if (expiry == null)
            {
                GetDatabase().StringSet(key, JsonConvert.SerializeObject(obj));
                return;
            }

            GetDatabase().StringSet(key, JsonConvert.SerializeObject(obj), expiry);
        }

        /// <summary>
        /// Adiciona um dado no redis de forma assincrona.
        /// </summary>
        /// <param name="obj">objeto que será adicionado no redis</param>
        /// <param name="keys">chave desse objeto</param>
        /// <param name="expiry">tempo de expiração</param>
        /// <returns></returns>
        public static async Task AddAsync(T obj, string key, TimeSpan? expiry = null)
        {
            if (expiry == null)
            {
                await GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject(obj));
                return;
            }

            await GetDatabase().StringSetAsync(key, JsonConvert.SerializeObject(obj), expiry);
        }

        /// <summary>
        /// Adiciona um dado no redis
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="keys"></param>
        /// <param name="expiry">tempo de expiração</param>
        public static void Add(T obj, IList<string> keys, TimeSpan? expiry = null)
        {
            foreach(var key in keys)
            {
                Add(obj, key, expiry);
            }
        }

        /// <summary>
        /// Adiciona um dado no redis de forma assincrona.
        /// </summary>
        /// <param name="obj">objeto que será adicionado no redis</param>
        /// <param name="keys">chaves desse objeto</param>
        /// <param name="expiry">tempo de expiração</param>
        /// <returns></returns>
        public static async Task AddAsync(T obj, IList<string> keys, TimeSpan? expiry = null)
        {
            foreach(var key in keys)
            {
                await AddAsync(obj, key, expiry);
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
        /// Obtém um objeto salvo no redis de forma assincrona
        /// </summary>
        /// <param name="key">chave do registro</param>
        /// <returns></returns>
        public static async Task<T> GetAsync(string key)
        {
            return JsonConvert.DeserializeObject<T>(await GetDatabase().StringGetAsync(key));
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