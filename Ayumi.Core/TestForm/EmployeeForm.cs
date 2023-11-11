using System;
using System.Linq;
using System.Windows.Forms;
using TestConstant;
using TestForm.Properties;
using TestInterface;
using WK.Forms;

namespace TestForm
{
    public partial class EmployeeForm : WinFormBase
    {
        private IEmployee employeeManager;
        
        public EmployeeForm()
        {
            InitializeComponent();
        }

        private void form4_Load(object sender, EventArgs e)
        {
            try
            {
                InitWinFormBase();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected override void MyInitialization()
        {
            employeeManager = (IEmployee)objectFactory.CreateObject(EmployeeConstant.ASSEMBLY_NAME, EmployeeConstant.FQN);
        }

        private void SetDataToUi(EmployeeData data)
        {
            employeeIdTxt.Text = data.EmployeeId;
            nameTxt.Text = data.Name;
            phoneNumberGrid.SetBindingDataSource(data.PhoneNumbers);
        }

        private void ShowBlankData()
        {
            var newData = new EmployeeData(employeeIdTxt.Text);
            SetDataToUi(newData);
        }

        private EmployeeData GetDataFromUi()
        {
            var data = new EmployeeData();
            data.EmployeeId = employeeIdTxt.Text.Trim();
            data.Name = nameTxt.Text.Trim();
            data.PhoneNumbers = phoneNumberGrid.GetBoundDataSource<EmployeeData.PhoneNumberData>().ToList();
            return data;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                var data = GetDataFromUi();
                employeeManager.SaveEmployee(data);
                MessageBox.Show(Resources.SAVE_SUCCCESS_MESSAGE);
            }
            catch(Exception ex)
            {
                ShowError(ex);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsConfirmDelete()) return;

                employeeManager.DeleteEmployee(employeeIdTxt.Text.Trim());
                MessageBox.Show(Resources.DELETE_SUCCESS_MESSAGE);
                ShowBlankData();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private bool IsConfirmDelete()
        {
            string message = string.Format(Resources.CONFIRM_DELETE_MESSAGE, employeeIdTxt.Text);
            return ShowConfirmation(message);
        }

        private void employeeIdTxt_Leave(object sender, EventArgs e)
        {
            try
            {
                var data = employeeManager.GetEmployee(employeeIdTxt.Text.Trim());
                SetDataToUi(data);
            }
            catch (Exception ex)
            {
                ShowError(ex);
                ShowBlankData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeSpan time = new TimeSpan(0,0,26589);
            MessageBox.Show(string.Format("Hour:{0} Minute:{1} Second:{2}", time.Hours, time.Minutes, time.Seconds));
        }
    }
}
