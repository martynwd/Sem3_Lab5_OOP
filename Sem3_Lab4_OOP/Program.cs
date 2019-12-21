using System;

namespace Sem3_Lab4_OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            Service service = new Service();
            //service.CreateShop(1, "Окей");
            //System.IO.File.WriteAllText("Shops.csv", "Test");            
            //service.CreateShop(6, "Ашан");
            //service.ShowShops();
            var product1 = service.CreateProduct("Шоколад 'Аленка'", 1);
            //service.AddProduct(1, product1);
            var product2 = service.CreateProduct("Телевизор PHILIPS", 6);
            //service.AddProduct(1, product2);
            //service.AddProduct(6, service.CreateProduct("Телевизор PHILIPS", 1, (decimal)21000.00));
            //Console.WriteLine(service.FindCheapest("Шоколад 'Аленка'").Name);
            //Console.WriteLine($"Price for product: {service.Buy(1, "Шоколад 'Аленка'", 20)}");
            Console.WriteLine(service.FindCheapestSum(product1, product2).Name);
        }
    }
}
