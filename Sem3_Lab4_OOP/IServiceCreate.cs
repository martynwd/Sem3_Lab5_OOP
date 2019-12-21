using System;
using System.Collections.Generic;
using System.Text;

namespace Sem3_Lab4_OOP
{
    interface IServiceCreate
    {
        Shop CreateShop(int shopId, string shopName);
       Product CreateProduct(string productName, int productCount, decimal productPrice);
    }
}
