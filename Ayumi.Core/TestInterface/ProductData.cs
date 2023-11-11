using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WK.DBUtility;

namespace TestInterface
{
    [Serializable]
    [WkTable("WK_Product")]
    public class ProductData
    {
        [WkField(DbFieldType.PRIMARY_KEY)]
        public int ProductId { get; set; }

        [WkField]
        public string Name { get; set; }

        [WkField]
        public string Description { get; set; }

        public ProductData(int productId):this()
        {
            ProductId = productId;
        }

        public ProductData()
        {
            ProductId = 0;
            Name = "";
            Description = "";
        }
    }
}
