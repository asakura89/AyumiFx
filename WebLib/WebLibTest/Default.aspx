<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebLibTest._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <link rel="stylesheet" href="Styles/bootstrap.css" />
        <link rel="stylesheet" href="Styles/site.css" />

        <%--<script type="text/javascript" language="javascript" src="../scripts/JScript.js"></script>
        <script type="text/javascript" language="javascript" src="../scripts/JSUtils.js"></script>--%>
    </head>
    <body>
        <form id="form1" runat="server" enctype="multipart/form-data">
            <div class="navbar navbar-fixed-top bg-color-primary">
                <div class="row">
                    <div class="col-md-3">
                        <div class="navbar-brand">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/header-logo.png" />
                        </div>
                    </div>
                    <div class="col-md-3 app-name">
                        <h4><asp:Label ID="lblAppName" runat="server" /></h4>
                    </div>
                    <div class="col-md-3">&nbsp;</div>
                    <div class="col-md-3">
                        <h6><asp:Label ID="lblUsername" CssClass="user-name" runat="server" /></h6>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Menu ID="mainMenu" Orientation="Horizontal" BackColor="#00377b" ForeColor="White" runat="server">
                            <StaticSelectedStyle BackColor="#00377b" ForeColor="White" Font-Underline="false" />
                            <StaticMenuItemStyle ForeColor="White" HorizontalPadding="5px" VerticalPadding="2px"  />
                            <DynamicMenuStyle BackColor="#00377b" />
                            <DynamicSelectedStyle BackColor="#00377b" ForeColor="White" Font-Underline="false" />
                            <DynamicMenuItemStyle BackColor="#00377b" ForeColor="White" HorizontalPadding="5px" VerticalPadding="2px"  />
                            <DynamicHoverStyle BackColor="White" ForeColor="#00377b"/>
                            <StaticHoverStyle BackColor="#00377b" ForeColor="White" />
                        </asp:Menu>
                    </div>
                </div>
            </div>
            <br />
            <div class="page">
                <br />
                <br />
                <div class="container">
                    <div class="row">
                        <div class="col-md-3 col-centered">
                            <div class="panel panel-default">
                                <div class="panel-heading">Login</div>
                                <div class="panel-body">
                                    <div class="input-group">
                                        <input type="text" id="txtName" class="form-control" runat="server" />
                                    </div>
                                    <div class="input-group">
                                        <input type="password" id="txtPassword" class="form-control" runat="server" />
                                    </div>
                                    <div class="input-group">
                                        <asp:Button ID="btnLogin" CssClass="btn btn-primary" Text="Login" runat="server" OnClick="btnLogin_Click"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="navbar-fixed-bottom footer">
                <div class="row">
                    <div class="col-md-12 col-centered">
                        <p><asp:Label ID="lblFooter" runat="server" /></p>
                    </div>
                </div>
            </div>
        </form>
    </body>
</html>
