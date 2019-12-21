using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sem3_Lab4_OOP
{
    class Product : IComparable<Product>
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        //[Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public Shop Shop { get; set; }

        public Product() { }
        public Product(string name, int count, decimal price = default)
        {
            Name = name;
            Count = count;
            Price = price;
        }

        public int CompareTo(Product other) => Name.CompareTo(other.Name) == 0
            ? Shop.ShopId.CompareTo(other.Shop.ShopId)
            : Name.CompareTo(other.Name);
    }
}
