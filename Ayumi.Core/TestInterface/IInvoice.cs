using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestInterface
{
    public interface IInvoice
    {
        InvoiceData GetInvoice(string invoiceId);
        void CreateInvoice(InvoiceData data);
        void UpdateInvoice(InvoiceData data);
        void DeleteInvoice(string invoiceId);
    }
}
