using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.BasketService.Infrastructure.Repository.Configuration
{
    public class RedisConfiguration
    {
        public string Endpoint { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
