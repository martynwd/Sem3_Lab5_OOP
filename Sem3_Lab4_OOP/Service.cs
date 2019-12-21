using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sem3_Lab4_OOP
{
    class Service : IServiceChange, IServiceCreate, IServiceFind
    {
        private IService _service;
        public Service()
        {
            if (!File.Exists("Service.config"))
            {
                throw new FileNotFoundException("Configuration file is not found");
            }
            using (StreamReader sr = new StreamReader("Service.config"))
            {
                string line;
                while ((line = sr.ReadLine()) == null || line == String.Empty)
                {
                    continue;
                }
                if (String.IsNullOrEmpty(line))
                {
                    throw new EndOfStreamException("Configuration file is empty");
                }
                string[] attribute = line.Split('=');
                if (attribute.Length != 2 || attribute[0] != "stringConnection")
                {
                    throw new ArgumentException("Wrong format of configuration file");
                }
                switch (attribute[1])
                {
                    case "DataBase":
                        _service = new ServiceDb();
                        break;
                    case "File":
                        _service = new ServiceFile();
                        break;
                    default:
                        throw new ArgumentNullException("Unkonown type of connection");
                }
            }
        }
        public void AddProduct(int shopId, Product product) => _service.AddProduct(shopId, product);


        public decimal Buy(int shopId, string productName, int productCount) => _service.Buy(shopId, productName, productCount);

        public Product CreateProduct(string productName, int productCount, decimal productPrice = default) => _service.CreateProduct(productName,
            productCount, productPrice);


        public Shop CreateShop(int shopId, string shopName) => _service.CreateShop(shopId, shopName);

        public Shop FindCheapest(string productName) => _service.FindCheapest(productName);

        public Shop FindCheapestSum(params Product[] products) => _service.FindCheapestSum(products);
    }
}
