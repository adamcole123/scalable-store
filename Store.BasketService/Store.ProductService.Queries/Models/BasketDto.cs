using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BasketService.Queries.Models
{
    public class BasketDto
    {
        public string Id { get; set; } = string.Empty;
        public string[] Items { get; set; } = [];
    }
}
