using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Sem3_Lab4_OOP
{
    class ServiceFile : IService
    {
        private ServiceFileContext _serviceFileContext;
        //private List<Shop> _shops;
        //private SortedSet<Product> _products;

        public ServiceFile()
        {
            _serviceFileContext = new ServiceFileContext();
        }
        public void AddProduct(int shopId, Product product)
        {
            var shop = _serviceFileContext.Shops.Where(x => x.ShopId == shopId).FirstOrDefault() ??
                throw new ArgumentException($"Shop with id \"{shopId}\" doesn't exist");
            var obj = _serviceFileContext.Products.Where(x => x.Name == product.Name && x.Shop.ShopId == shop.ShopId).FirstOrDefault();
            if (obj != null)
            {
                obj.Count += product.Count;
                obj.Price = product.Price;
            }
            else
            {
                shop.Products.Add(product);
                product.Shop = shop;
                _serviceFileContext.Products.Add(product);
            }
            _serviceFileContext.SaveProducts();

        }

        public decimal Buy(int shopId, string productName, int productCount)
        {
            var product = _serviceFileContext.Products.Where(x => x.Name == productName && x.Shop.ShopId == shopId).FirstOrDefault();
            if (product == null)
            {
                throw new ArgumentException($"There is no product \"{productName}\" in shop with id \"{shopId}\"");
            }
            if (product.Count < productCount)
            {
                throw new ArgumentException($"Not enough product\nAmount of product \"{productName}\": {product.Count}");
            }
            product.Count -= productCount;
            _serviceFileContext.SaveProducts();
            return product.Price * productCount;
        }

        public Product CreateProduct(string productName, int productCount, decimal productPrice = default)
        {
            return new Product(productName, productCount, productPrice);
        }

        public Shop CreateShop(int shopId, string shopName)
        {
            var shop = _serviceFileContext.Shops.Where(x => x.ShopId == shopId || x.Name == shopName).FirstOrDefault();
            if (shop != null)
            {
                if (shop.ShopId == shopId)
                {
                    throw new ArgumentException($"Shop with id \"{shopId}\" already exists");
                }
                else
                {
                    throw new ArgumentException($"Shop with name \"{shopName}\" already exists");
                }
            }
            var newShop = new Shop(shopId, shopName);
            _serviceFileContext.Shops.Add(newShop);
            _serviceFileContext.SaveShops();
            return newShop;
        }

        public Shop FindCheapest(string productName)
        {
            var shop = _serviceFileContext.Products
                .Where(x => x.Name == productName)
                .OrderBy(x => x.Price)
                .ThenByDescending(x => x.Count)
                .FirstOrDefault()
                .Shop;
            if (shop == null)
            {
                throw new ArgumentException($"Product with name \"{productName}\" doesn't exist");
            }
            else
            {
                return shop;
            }
        }

        public Shop FindCheapestSum(params Product[] products)
        {
            Shop shop = null;
            decimal minCost = decimal.MaxValue;
            foreach (var el in _serviceFileContext.Shops)
            {
                var productsInShop = el.Products.Where(x => products.Select(o => o.Name).Contains(x.Name)
                    && x.Count >= products.Where(o => o.Name == x.Name).FirstOrDefault().Count);
                if (productsInShop == null || productsInShop.ToArray().Length != products.Length)
                {
                    continue;
                }
                var cost = productsInShop.Sum(x => x.Price * products.Where(o => o.Name == x.Name).FirstOrDefault().Count);
                if (cost < minCost)
                {
                    minCost = cost;
                    shop = el;
                }
            }
            if (shop == null)
            {
                throw new ArgumentException("This set of products doesn't exist");
            }
            return shop;
        }
    }
}
