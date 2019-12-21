using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Sem3_Lab4_OOP
{
    class Shop
    {
        public Shop()
        {
            Products = new List<Product>();
        }

        public Shop(int shopId, string shopName)
        {
            Products = new List<Product>();
            ShopId = shopId;
            Name = shopName;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShopId { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }

    }
}
