using System;
using TestInterface;
using WK.AppUtility;
using WK.DBUtility;

namespace TestManager
{
    public sealed class InvoiceManager : WkManagerBase, IInvoice
    {
        #region IInvoice Members

        InvoiceData IInvoice.GetInvoice(string invoiceId)
        {
            using (DbAccess dbManager = GetDbAccess("Invoice.GetInvoice()"))
            {
                using (var sqlcommand = dbManager.CreateCommand())
                    return WkEntitySql.SelectById(sqlcommand, new InvoiceData(invoiceId));
            }
        }

        void IInvoice.CreateInvoice(InvoiceData data)
        {
            using (DbAccess dbAccess = GetDbAccess("Invoice.CreateInvoice()"))
            {
                dbAccess.BeginTransaction();
                using (var sqlCommand = dbAccess.CreateCommand())
                    WkEntitySql.Insert(sqlCommand, data);
                dbAccess.CommitTransaction();
            }
        }

        void IInvoice.UpdateInvoice(InvoiceData data)
        {
            using (var dbManager = GetDbAccess("Invoice.UpdateInvoice()"))
            {
                dbManager.BeginTransaction();
                using (var sqlCommand = dbManager.CreateCommand())
                    WkEntitySql.Update(sqlCommand, data);
                dbManager.CommitTransaction();
            }
        }

        void IInvoice.DeleteInvoice(string invoiceId)
        {
            using (DbAccess dbManager = GetDbAccess("Invoice.DeleteInvoice()"))
            {
                dbManager.BeginTransaction();
                using (var sqlCommand = dbManager.CreateCommand())
                    WkEntitySql.Delete(sqlCommand, new InvoiceData(invoiceId));
                dbManager.CommitTransaction();
            }
        }

        #endregion
    }
}
