using System;
using WebLib.Extensions.Model;
using WebLibTest.Factories;
using WebLibTest.Presenters;

namespace WebLibTest
{
    public partial class _Default : System.Web.UI.Page
    {
        private LoginPresenter loginPresenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitPresenter();
            CheckUserAuth();
        }

        private void InitPresenter()
        {
            ISimpleServiceFactory serviceFactory = new WebLibTestServiceFactory();
            loginPresenter = new LoginPresenter(serviceFactory);
            loginPresenter.CurrentUserIp = Request.UserHostAddress;
        }

        private void CheckUserAuth()
        {
            if (loginPresenter.CurrentUserId == null)
                loginPresenter.RedirectPage(this, "~/View/Homepage.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                loginPresenter.Login(txtName.Value, txtPassword.Value);
                //Session[SessionConstant.SESS_ID_USER] = txtName.Value;
                loginPresenter.RedirectPage(this, "~/View/Homepage.aspx");
            }
            catch (Exception ex)
            {
                loginPresenter.OnException(this, ex);
                //ClientScript.RegisterClientScriptBlock(Page.GetType(), "Redirect", "window.open(\"" + ResolveUrl("~/") + "\", \"_self\")", true);
                loginPresenter.RedirectPageByClientScript(this, ResolveUrl("~/"));
            }
        }
    }
}
