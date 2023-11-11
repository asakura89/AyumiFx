using System;
using System.Data.SqlClient;
using TestInterface;
using WK.AppUtility;
using WK.DBUtility;

namespace TestManager
{
    public class StockManager : WkManagerBase, IStock
    {
        void IStock.SaveStock(StockData stock)
        {
            using (DbAccess dbAccess = GetDbAccess("StockManager.SaveStock"))
            {
                dbAccess.BeginTransaction();
                JustSaveStock(dbAccess, stock);
                dbAccess.CommitTransaction();
            }
        }

        void JustSaveStock(DbAccess dbAccess, StockData stock)
        {
            using (SqlCommand command = dbAccess.CreateCommand())
                WkEntitySql.Save(command, stock);
        }

        void IStock.DeleteStock(int stockId)
        {
            using (DbAccess db = GetDbAccess("Stock.DeleteStock"))
            {
                db.BeginTransaction();
                JustDeleteStock(db, stockId);
                db.CommitTransaction();
            }
        }

        void JustDeleteStock(DbAccess db, int stockId)
        {
            using (SqlCommand command = db.CreateCommand())
                WkEntitySql.Delete(command, new StockData(stockId));
        }
        
        StockData IStock.GetStock(int stockId)
        {
            using (DbAccess db = GetDbAccess("Stock.GetStock"))
            {
                StockData stock = JustGetStock(db, stockId);
                if (stock == null)
                    throw new Exception(string.Format("Stock data {0} doesn't exist", stockId));
                return stock;
            }
        }

        StockData JustGetStock(DbAccess tx, int stockId)
        {
            using (SqlCommand cmd = tx.CreateCommand())
            {
                cmd.CommandText = @"select stk.stockId, stk.productId, stk.qty, prd.Name as productName_J
                    from WK_Stock stk
                    left join WK_Product prd
                    on stk.productId = prd.productId
                    where stk.stockId = @stockId";
                cmd.Parameters.AddWithValue("@stockId", stockId);

                return ExecuteReader(cmd);
            }
        }

        private static StockData ExecuteReader(SqlCommand cmd)
        {
            using (SqlDataReader dataReader = cmd.ExecuteReader())
            {
                if (dataReader.Read())
                    return ReadStockFromDr(dataReader);
                
                return null;
            }
        }

        private static StockData ReadStockFromDr(SqlDataReader dataReader)
        {
            StockData stock = new StockData();
            stock.StockId = (int) dataReader["stockId"];
            stock.ProductId = (int) dataReader["productId"];
            stock.Qty = (int) dataReader["qty"];
            stock.ProductName_J = (string) dataReader["productName_J"];
            return stock;
        }
    }
}
