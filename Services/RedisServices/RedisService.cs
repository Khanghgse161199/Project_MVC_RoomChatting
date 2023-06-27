using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RedisServices
{
    public interface IRedisService
    {
        Task<bool> SaveConectionAsync(string accId, string connectId);
        Task<string> GetConnectionIdAsync(string accId);
    }
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _Cache;
        public RedisService(IDistributedCache cache)
        {
            _Cache = cache;
        }
        public async Task<bool> SaveConectionAsync(string accId, string connectId)
        {
            //chứa các tùy chọn để định cấu hình mục trong _Cache.
            var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
            .SetSlidingExpiration(TimeSpan.FromMinutes(10));
            var typeBytes = Encoding.UTF8.GetBytes(connectId);
            //ghi dữ liệu vào cache
            await _Cache.SetAsync(accId, typeBytes, options);
            return true;
        }

        public async Task<string> GetConnectionIdAsync(string accId)
        {
            var resut = await _Cache.GetAsync(accId);
            if (resut != null)
            {
                var stringValue = Encoding.UTF8.GetString(resut);
                return stringValue;
            }
            else return string.Empty;
        }

    }
}
