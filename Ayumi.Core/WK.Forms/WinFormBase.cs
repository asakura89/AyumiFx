using System;
using System.Windows.Forms;
using WK.Forms.Properties;
using WK.RemotingInterface;

namespace WK.Forms
{
    public class WinFormBase : Form
    {
        protected IObjectFactory objectFactory;
        private readonly ConnectForm connectForm = new ConnectForm();

        protected void InitWinFormBase()
        {
            ConnectToServer();
            MyInitialization();
        }

        private void ConnectToServer()
        {
            while (objectFactory == null)
            {
                connectForm.ShowDialog();
                objectFactory = connectForm.objectFactory;
            }
        }

        protected virtual void MyInitialization()
        {
            throw new NotImplementedException("MyInitialization method is not implemented.");
        }

        protected void ShowError(Exception ex)
        {
            using (var errForm = new ErrorMessageForm())
                errForm.Run(ex);
        }

        protected bool ShowConfirmation(string message)
        {
            DialogResult dialogResult = MessageBox.Show(message,
                   Resources.CONFIRMATION_TITLE, MessageBoxButtons.YesNo);

            return dialogResult == DialogResult.Yes;
        }
    }
}
