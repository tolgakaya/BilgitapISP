<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Giris.aspx.cs" Inherits="TeknikServis.Account.Giris" EnableEventValidation="false" ValidateRequest="false" EnableViewStateMac="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Teknik Servis Giriş</title>
    <!-- Core CSS - Include with every page -->
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
 <%--   <link href="../Content/font-awesome.css" rel="stylesheet" />
    <link href="../Content/pace-theme-big-counter.css" rel="stylesheet" />--%>
    <link href="../Content/style.min.css" rel="stylesheet" />
    <link href="../Content/main-style.min.css" rel="stylesheet" />
       <script src="../Scripts/jquery-2.1.3.min.js"></script>
        <script src="../Scripts/bootstrap.min.js"></script>
   <%--     <script src="../Scripts/jquery.metisMenu.js"></script>--%>
    
</head>
<body class="body-Login-back">
    <form id="form1" runat="server">
<asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="container">

            <div class="row">
                <div class="col-md-4 col-md-offset-4 text-center logo-margin ">
                    <img src="../img/logo.png" alt="" />
                </div>
                <div class="col-md-4 col-md-offset-4">
                    <div class="login-panel panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Giriş Yapın</h3>
                        </div>
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="panel-body">

                            <fieldset>
                                <div class="form-group">

                                    <asp:TextBox runat="server" ID="Email" CssClass="form-control"  />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                        CssClass="text-danger" ErrorMessage="Kullanıcı adını giriniz." />

                                </div>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Şifreyi giriniz." />
                                </div>
                                <div class="form-group">
                                    <asp:CheckBox runat="server" ID="RememberMe" />
                                    <asp:Label runat="server" AssociatedControlID="RememberMe">Beni hatırla</asp:Label>
                                </div>
                                <!-- Change this to a button or input when using this as a form -->
                                <div class="form-group">
                                    
                                        <asp:Button runat="server" OnClick="LogIn" Text="Giriş" CssClass="btn btn-lg btn-info btn-block" />
                                    
                                </div>
                              
                            </fieldset>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Core Scripts -->
     
    </form>
</body>
</html>
