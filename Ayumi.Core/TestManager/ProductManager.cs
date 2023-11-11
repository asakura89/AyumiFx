using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TestInterface;
using WK.AppUtility;
using WK.DBUtility;

namespace TestManager
{
    public class ProductManager : WkManagerBase, IProduct
    {
        void IProduct.Insert(ProductData product)
        {
            using (DbAccess db = GetDbAccess("Product.Insert"))
            {
                using (SqlCommand cmd = db.CreateCommand())
                    WkEntitySql.Insert(cmd, product);
            }
        }

        void IProduct.Update(ProductData product)
        {
            using (DbAccess db = GetDbAccess("Product.Update"))
            {
                using (SqlCommand cmd = db.CreateCommand())
                    WkEntitySql.Update(cmd, product);
            }
        }

        void IProduct.Save(ProductData product)
        {
            using (DbAccess db = GetDbAccess("Product.Update"))
            {
                using (SqlCommand cmd = db.CreateCommand())
                    WkEntitySql.Save(cmd, product);
            }
        }

        ProductData IProduct.Get(int productId)
        {
            using (DbAccess db = GetDbAccess("Product.Get"))
            {
                using(SqlCommand cmd = db.CreateCommand())
                {
                    ProductData product = WkEntitySql.SelectById(cmd, new ProductData(productId));
                    if(product == null)
                        throw new Exception(string.Format("Product {0} doesn't exist.", productId));

                    return product;
                }
            }
        }

        void IProduct.Delete(int productId)
        {
            using (DbAccess db = GetDbAccess("Product.Delete"))
            {
                using (SqlCommand cmd = db.CreateCommand())
                    WkEntitySql.Delete(cmd, new ProductData(productId));
            }
        }
    }
}

