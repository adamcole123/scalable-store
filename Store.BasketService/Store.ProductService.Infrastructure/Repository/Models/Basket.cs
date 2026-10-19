using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BasketService.Infrastructure.Repository.Models
{
    public class Basket
    {
        public string BasketId { get; set; } = string.Empty;
        public string[] Items{ get; set; } = [];
    }
}
