using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.Entity;

namespace Sem3_Lab4_OOP
{
    class ServiceDb : IService
    {
        private ServiceDbContext _serviceDbContext;
        public ServiceDb()
        {
            _serviceDbContext = new ServiceDbContext();
        }

        public void AddProduct(int shopId, Product product)
        {
            var db = _serviceDbContext.Shops.Include(o => o.Products);
            var shop = db.FirstOrDefault(o => o.ShopId == shopId);
            if (shop == null)
            {
                throw new ArgumentException($"Shop with id \"{shopId}\" doesn't exist");
            }
            var obj = shop.Products.FirstOrDefault(o => o.Name == product.Name && o.Shop.ShopId == shopId);
            if (obj != null)
            {
                obj.Price = product.Price;
                obj.Count += product.Count;
            }
            else
            {
                product.Shop = shop;
                shop.Products.Add(product);
            }
            _serviceDbContext.SaveChanges();
        }

        public decimal Buy(int shopId, string productName, int productCount)
        {
            var product = _serviceDbContext.Products
                .Where(o => o.Shop.ShopId == shopId && o.Name == productName)
                .FirstOrDefault();
            if (product == null)
            {
                throw new ArgumentException($"There is no product \"{productName}\" in shop with id \"{shopId}\"");
            }
            if (product.Count < productCount)
            {
                throw new ArgumentException($"Not enough product\nAmount of product \"{productName}\": {product.Count}");
            }
            product.Count -= productCount;
            _serviceDbContext.SaveChanges();
            return productCount * product.Price;
        }

        public Product CreateProduct(string productName, int productCount, decimal productPrice = default)
        {
            return new Product(productName, productCount, productPrice);
        }

        public Shop CreateShop(int shopId, string shopName)
        {
            var shop = _serviceDbContext.Shops.FirstOrDefault(o => o.ShopId == shopId || o.Name == shopName);
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
            else
            {
                var newShop = new Shop(shopId, shopName);
                _serviceDbContext.Shops.Add(newShop);
                _serviceDbContext.SaveChanges();
                return newShop;
            }
        }

        public Shop FindCheapest(string productName)
        {
            var shop = _serviceDbContext.Products
                .Include(o => o.Shop)
                .Where(o => o.Name == productName)
                .OrderBy(o => o.Price)
                .ThenByDescending(o => o.Count)
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
            //var productsName = products.Select(x => x.Name);
            decimal minCost = decimal.MaxValue;
            Shop shopMinCost = null;
            var shopList = _serviceDbContext.Shops.ToList();
            var productList = _serviceDbContext.Products.ToList();
            foreach (var shop in shopList)
            {
                var productsInShop = productList.Where(o => o.Shop.ShopId == shop.ShopId && products.Select(x => x.Name).Contains(o.Name));
                if (productsInShop == null || productsInShop.ToArray().Length != products.Length)
                {
                    continue;
                }

                bool productCountOk = true;
                Product productWithoutCount;
                foreach (var product in products)
                {
                    productWithoutCount = productsInShop.Where(x => x.Name == product.Name && x.Count < product.Count).FirstOrDefault();

                    if (productWithoutCount != null)
                    {
                        productCountOk = false;
                        break;
                    }
                }
                if (!productCountOk)
                {
                    continue;
                }

                var cost = productsInShop.Sum(x => x.Price * products.Where(o => x.Name == o.Name).FirstOrDefault().Count);
                if (cost < minCost)
                {
                    minCost = cost;
                    shopMinCost = shop;
                }
            }
            if (shopMinCost == null)
            {
                throw new ArgumentException($"This set of products doesn't exist");
            }
            return shopMinCost;
        }

        //Testing
        public void ShowShops()
        {
            var list = _serviceDbContext.Shops.ToList();
            foreach (var el in list)
            {
                Console.WriteLine(el.Name);
            }
        }
    }
}
