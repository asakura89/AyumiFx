using System;
using System.Windows.Forms;
using TestConstant;
using TestForm.Properties;
using TestInterface;
using WK.Forms;
using WK.RemotingInterface;

namespace TestForm
{
    public partial class CompanyForm : WinFormBase
    {
        private ICompany companyManager;

        public CompanyForm()
        {
            InitializeComponent();
        }

        protected override void MyInitialization()
        {
            companyManager = (ICompany)objectFactory.CreateObject(CompanyConstant.ASSEMBLY_NAME, CompanyConstant.FQN);
        }

        private CompanyData GetDataFromUi()
        {
            var data = new CompanyData
                {
                    CompanyId = companyIdTxt.Text, 
                    Name = nameTxt.Text, 
                    Address = addressTxt.Text,
                    IsActive = activeChk.Checked,
                    ActiveDate =  activeDatePicker.Value
                };
            return data;
        }

        private void SetDataToUi(CompanyData data)
        {
            companyIdTxt.Text = data.CompanyId;
            nameTxt.Text = data.Name;
            addressTxt.Text = data.Address;
            activeChk.Checked = data.IsActive;
            activeDatePicker.Value = data.ActiveDate;
        }

        private void SetBlankUi()
        {
            var data = new CompanyData(companyIdTxt.Text);
            SetDataToUi(data);
        }

        private void companyIdTxt_Leave(object sender, EventArgs e)
        {
            try
            {
                CompanyData data = companyManager.GetCompany(companyIdTxt.Text);

                if (data == null) 
                {
                    SetBlankUi();
                    return;
                }

                SetDataToUi(data);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyData data = GetDataFromUi();
                companyManager.InsertCompany(data);

                MessageBox.Show(Resources.INSERT_SUCCESS_MESSAGE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyData data = GetDataFromUi();
                companyManager.UpdateCompany(data);

                MessageBox.Show(Resources.UPDATE_SUCCESS_MESSAGE);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyData data = GetDataFromUi();

                if (MessageBox.Show(string.Format(Resources.DELETE_CONFIRMATION_MESSAGE, data.CompanyId), 
                    Resources.CONFIRMATION_TITLE, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                companyManager.DeleteCompany(data.CompanyId);
                MessageBox.Show(Resources.DELETE_SUCCESS_MESSAGE);

                SetBlankUi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //companyManager.Dispose();
        }


        private void form1_Load(object sender, EventArgs e)
        {
            try
            {
                //InitWinFormBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void activeChk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                activeDatePicker.Enabled = activeChk.Checked;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
