using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestInterface
{
    public interface IStock
    {
        void SaveStock(StockData stock);
        void DeleteStock(int stockId);
        StockData GetStock(int stockId);
    }
}
