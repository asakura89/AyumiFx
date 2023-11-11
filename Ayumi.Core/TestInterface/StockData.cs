using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WK.DBUtility;

namespace TestInterface
{
    [Serializable]
    [WkTable("WK_Stock")]
    public class StockData
    {
        [WkField(DbFieldType.PRIMARY_KEY)]
        public int StockId { get; set; }

        [WkField]
        public int ProductId { get; set; }

        public string ProductName_J { get; set; }

        [WkField]
        public int Qty { get; set; }

        public StockData(int stockId) : this()
        {
            StockId = stockId;
        }

        public StockData()
        {
            StockId = 0;
            ProductId = 0;
            ProductName_J = "";
            Qty = 0;
        }
    }
}
