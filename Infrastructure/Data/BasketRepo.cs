﻿using Core.Entities;
using Core.Entities.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BasketRepo : IBasketRepo
    {
        private readonly IDatabase _database; 
        public BasketRepo(IConnectionMultiplexer redis)
        {
            _database=redis.GetDatabase();
        }
        public async Task DeleteBasketAsync(string basketId)
        =>await _database.KeyDeleteAsync(basketId);

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data=await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket),TimeSpan.FromDays(30));
            if (!created)
                return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
