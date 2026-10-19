using Store.BasketService.Infrastructure.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using Store.BasketService.Infrastructure.Repository.Configuration;

namespace Store.BasketService.Infrastructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        IDatabase db;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            db = redis.GetDatabase();
        }
        public Basket Get(string id)
        {
            var val = db.HashGetAll(new RedisKey(id));
            return new Basket()
            {
                BasketId = id,
                Items = val.Select(x => x.Value.ToString()).ToArray(),
            };
        }

        public void AddItem(string basketId, string productId)
        {
            db.SetAdd(basketId, productId);
        }

        public void RemoveItem(string basketId, string productId)
        {
            db.SetRemove(basketId, productId);
        }
    }
}
