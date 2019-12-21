using System;
using System.Collections.Generic;
using System.Text;

namespace Sem3_Lab4_OOP
{
    interface IServiceFind
    {
      Shop FindCheapestSum(params Product[] products);
       Shop FindCheapest(string productName);

    }
}
