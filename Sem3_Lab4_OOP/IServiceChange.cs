using System;
using System.Collections.Generic;
using System.Text;

namespace Sem3_Lab4_OOP
{
    interface IServiceChange
    {

        void AddProduct(int shopId, Product product);
        decimal Buy(int shopId, string productName, int productCount);

    }
}
