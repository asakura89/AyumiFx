using System;
using System.Windows.Forms;
using WK.RemotingInterface;

namespace WK.Forms
{
    public partial class ConnectForm : Form
    {
        public IObjectFactory objectFactory;

        public ConnectForm()
        {
            InitializeComponent();
            serverTxt.Text = "KEA-1-PC";
            portTxt.Text = "10000";
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                objectFactory = RemotingClientLib.CreateObjectFactory(serverTxt.Text, int.Parse(portTxt.Text));
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
