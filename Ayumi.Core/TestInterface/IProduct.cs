using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestInterface
{
    public interface IProduct
    {
        void Insert(ProductData product);
        void Update(ProductData product);
        ProductData Get(int productId);
        void Delete(int productId);
        void Save(ProductData product);
    }
}
