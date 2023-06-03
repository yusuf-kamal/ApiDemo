using Core.Entities.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ResponseCasheService : IResponseCasheService
    {
        private readonly IDatabase _database;
        public ResponseCasheService( IConnectionMultiplexer redis)
        {
            _database= redis.GetDatabase();
        }
        public async Task CasheResponseAsync(string cashekey, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var serilizedResponse=JsonSerializer.Serialize(response, options);
            await _database.StringSetAsync(cashekey,serilizedResponse,timeToLive);
        }

        public async Task<string> GetCashedResponse(string cashekey)
        {
            var cashedResponse= await _database.StringGetAsync(cashekey);
            if (cashedResponse.IsNullOrEmpty)
                return null;
            return cashedResponse;
        }
    }
}
