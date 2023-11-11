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
    public class StockMutationManager : WkManagerBase, IStockMutation
    {
        private readonly IStock stockMgr;

        public StockMutationManager()
        {
            stockMgr = new StockManager();
        }

        void IStockMutation.Save(StockMutationData mutationData)
        {
            using (DbAccess dbAccess = GetDbAccess("StockMutation.Save"))
            {
                dbAccess.BeginTransaction();
                JustSave(dbAccess, mutationData);
                DoEffect(mutationData);
                dbAccess.CommitTransaction();
            }
        }

        void DoEffect(StockMutationData mutationData)
        {
            foreach(StockMutationData.Detail item in mutationData.Items)
            {
                StockData stock = stockMgr.GetStock(item.StockId);
                stock.Qty += item.MutationQty;
                if(stock.Qty<0)
                    throw new Exception("Stock cannot be less than zero.");
                stockMgr.SaveStock(stock);
            }
        }

        void JustSave(DbAccess dbAccess, StockMutationData mutationData)
        {
            using (SqlCommand command = dbAccess.CreateCommand())
            {
                WkEntitySql.Save(command, mutationData);
            }
        }

        void IStockMutation.Delete(string mutationId)
        {
            using (DbAccess db = GetDbAccess("StockMutation.Delete"))
            {
                db.BeginTransaction();
                JustDelete(db, mutationId);
                db.CommitTransaction();
            }
        }

        private static void JustDelete(DbAccess db, string mutationId)
        {
            using (SqlCommand cmd = db.CreateCommand())
            {
                WkEntitySql.Delete(cmd, new StockMutationData(mutationId));
            }
        }

        StockMutationData IStockMutation.Get(string mutationId)
        {
            using (DbAccess db = GetDbAccess("StockMutation.Get"))
            {
                StockMutationData mutation = null;
                using (SqlCommand cmd = db.CreateCommand())
                {
                    mutation = WkEntitySql.SelectById(cmd, new StockMutationData(mutationId));
                    if(mutation == null)
                        throw new Exception("Mutation data doesn't exist");
                }
                mutation.Items = GetDetails(db, mutationId);
                return mutation;
            }
        }

        List<StockMutationData.Detail> GetDetails(DbAccess db, string mutationId)
        {
            using (SqlCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = @"select det.StockId, det.ProductId, det.MutationQty
                    stk.ProductId as ProductId_J, prd.Name as ProductName_J 
                    from WK_StockMutationDetail det 
                    left join WK_Stock stk
                    on det.StockId = stk.StockId
                    left join WK_Product prd
                    on stk.productId = prd.ProductId";

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                    return DataReaderUtility.ReadMultipleDataFromDr<StockMutationData.Detail>(dataReader).ToList();

            }
        }
    }
}
