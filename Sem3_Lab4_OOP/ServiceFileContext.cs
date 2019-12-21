using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Sem3_Lab4_OOP
{
    class ServiceFileContext
    {
        //private StreamWriter _swShops;
        //private StreamWriter _swProducts;
        public ServiceFileContext()
        {
            ReadShops();
            ReadProducts();
        }

        public List<Shop> Shops { get; set; }
        public SortedSet<Product> Products { get; set; }

        public void SaveShops()
        {
            //File.WriteAllText("Shops.csv", String.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (var el in Shops)
            {
                sb.Append($"{el.ShopId},{el.Name}");
                sb.AppendLine();
            }
            using (StreamWriter sw = new StreamWriter("Shops.csv", false))
            {
                sw.WriteLine(sb.ToString());
            }
        }

        public void SaveProducts()
        {
            if (Products.Count == 0)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            string productName = new string(Products.FirstOrDefault().Name.ToCharArray());
            sb.Append(productName);
            foreach (var el in Products)
            {
                if (productName != el.Name)
                {
                    sb.AppendLine();
                    productName = new string(el.Name.ToCharArray());
                    sb.Append(productName);
                }
                sb.Append($",{el.Shop.ShopId},{el.Count},{el.Price.ToString("0.00", new CultureInfo("en-US"))}");
            }
            using (StreamWriter sw = new StreamWriter("Products.csv", false))
            {
                sw.WriteLine(sb.ToString());
            }
        }

        private void ReadShops()
        {
            FileStream fileStream = new FileStream("Shops.csv", FileMode.OpenOrCreate);
            fileStream.Close();
            Shops = new List<Shop>();
            using (StreamReader sr = new StreamReader("Shops.csv"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    string[] shopAttributes = line.Split(',');
                    if (shopAttributes.Length != 2)
                    {
                        Shops = null;
                        throw new FormatException("File \"Shops.csv\" has wrong format");
                    }
                    if (!Int32.TryParse(shopAttributes[0], out int shopId))
                    {
                        Shops = null;
                        throw new FormatException($"Cannot convert ShopId \"{shopId}\" to int");
                    }
                    else
                    {
                        Shops.Add(new Shop(shopId, shopAttributes[1]));
                    }
                }
            }
            //_swShops = new StreamWriter("Shops.txt", true);
        }
        private void ReadProducts()
        {
            FileStream fileStream = new FileStream("Products.csv", FileMode.OpenOrCreate);
            fileStream.Close();
            Products = new SortedSet<Product>();
            using (StreamReader sr = new StreamReader("Products.csv"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    string[] productAttributes = line.Split(',');
                    if ((productAttributes.Length - 1) % 3 != 0 && productAttributes.Length != 1)
                    {
                        Products = null;
                        throw new FormatException("File \"Products.csv\" has wrong format");
                    }
                    string productName = productAttributes[0];
                    for (int i = 0; i < productAttributes.Length / 3; i++)
                    {
                        if (!Int32.TryParse(productAttributes[3 * i + 1], out int productShopId) ||
                            !Int32.TryParse(productAttributes[3 * i + 2], out int productCount) ||
                            !Decimal.TryParse(productAttributes[3 * i + 3], System.Globalization.NumberStyles.Any, new CultureInfo("en-US"),
                                out decimal productPrice)
                            )
                        {
                            Products = null;
                            throw new FormatException($"Cannot convert attributes of product \"{productName}\"");
                        }
                        Product product = new Product(productName, productCount, productPrice);
                        Shop shop = Shops.Where(o => o.ShopId == productShopId).FirstOrDefault();
                        if (shop == null)
                        {
                            Products = null;
                            throw new ArgumentException($"Shop with id \"{productShopId}\" doesn't exist");
                        }
                        product.Shop = shop;
                        shop.Products.Add(product);
                        Products.Add(product);
                    }
                }
            }
            //_swProducts = new StreamWriter("Products.csv", true);
        }
    }
}
