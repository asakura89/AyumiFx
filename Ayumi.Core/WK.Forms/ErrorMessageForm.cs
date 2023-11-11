using System;
using System.Windows.Forms;
using WK.Forms.Properties;

namespace WK.Forms
{
    public partial class ErrorMessageForm : Form
    {
        public ErrorMessageForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showDetailButton_Click(object sender, EventArgs e)
        {
            ToggleStackTraceVisibility();
        }

        private bool isStackTraceVisible;

        private void ToggleStackTraceVisibility()
        {
            if (isStackTraceVisible)
                HideStackTrace();
            else
                ShowStackTrace();

            isStackTraceVisible = !isStackTraceVisible;
        }

        private void ShowStackTrace()
        {
            Height = closeButton.Top + closeButton.Height + stackTraceTxt.Height + 50;
            showDetailButton.Text = Resources.HIDE_BUTTON_TEXT;
        }

        private void HideStackTrace()
        {
            Height = closeButton.Top + closeButton.Height + 32;
            showDetailButton.Text = Resources.SHOW_DETAIL_BUTTON_TEXT;
        }

        public void Run(Exception ex)
        {
            InitDialog(ex);
            CenterToParent();
            ShowDialog();
        }

        private void InitDialog(Exception ex)
        {
            errorMessageLabel.Text = ex.Message;
            stackTraceTxt.Text = ex.StackTrace;
            HideStackTrace();
        }
    }
}
