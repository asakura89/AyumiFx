using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TestConstant;
using TestInterface;
using WK.Forms;

namespace TestForm
{
    public partial class InvoiceForm : WinFormBase
    {
        private IInvoice invoiceManager;

        public InvoiceForm()
        {
            InitializeComponent();
        }

        protected override void MyInitialization()
        {
            invoiceManager = (IInvoice)objectFactory.CreateObject(InvoiceConstant.ASSEMBLY_NAME, InvoiceConstant.FQN);
        }

        private void SetBlankUi()
        {
            var data = new InvoiceData {InvoiceNo = invoiceNoTxt.Text};
            SetDataToUi(data);
        }

        private void SetDataToUi(InvoiceData data)
        {
            invoiceNoTxt.Text = data.InvoiceNo;
            datePicker.Value = data.Date;
            customerTxt.Text = data.CustomerId;
            
            SetDataItemToUi(data.Items);
        }

        private void SetDataItemToUi(List<InvoiceItemData> items)
        {
            var dataToBind = new BindingList<InvoiceItemData>();
            
            foreach(var item in items)
                dataToBind.Add(item);

            itemGrid.DataSource = dataToBind;
        }

        private InvoiceData GetDataFromUi()
        {
            var data = new InvoiceData();

            data.InvoiceNo = invoiceNoTxt.Text;
            data.Date = datePicker.Value;
            data.CustomerId = customerTxt.Text;
            data.Items = GetDataItemFromUi();

            return data;
        }

        private List<InvoiceItemData> GetDataItemFromUi()
        {
            var boundList = (BindingList<InvoiceItemData>) itemGrid.DataSource;

            return boundList.ToList();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            try
            {
                var data = GetDataFromUi();
                invoiceManager.CreateInvoice(data);

                MessageBox.Show("Create success.");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                var data = GetDataFromUi();
                invoiceManager.UpdateInvoice(data);

                MessageBox.Show("Update success.");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                var data = GetDataFromUi();
                if(MessageBox.Show(string.Format("Do you want to delete invoice '{0}'?", data.InvoiceNo), 
                    "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                invoiceManager.DeleteInvoice(data.InvoiceNo);

                MessageBox.Show("Delete success.");
                SetBlankUi();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void invoiceNoTxt_Leave(object sender, EventArgs e)
        {
            try
            {
                InvoiceData data = invoiceManager.GetInvoice(invoiceNoTxt.Text);
                
                if (data == null)
                {
                    SetBlankUi();
                    return;
                }

                SetDataToUi(data);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void form3_Load(object sender, EventArgs e)
        {
            try
            {
                //InitWinFormBase();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }
    }
}
