<%@ Page Title="Şifre Değişikliği" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManagePassword.aspx.cs" Inherits="TeknikServis.Account.ManagePassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <div class="kaydir">
        <div id="panelContents" runat="server" class="panel panel-info">
            <div class="panel-heading">
               <h2 class="baslik"> Şifre Değişikliği</h2>
            </div>
            <div class="form-horizontal">
                <section id="passwordForm">
                    <asp:PlaceHolder runat="server" ID="setPassword" Visible="false">
                        <p>
                            Yerel bir şifreniz bulunmamaktadır. Bir şifre ekleyerek bu şifreyle giriş yapabilirsiniz.
                        </p>
                        <div class="form-horizontal">
                            <h4>Set Password Form</h4>
                            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                            <hr />
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="password" CssClass="col-md-2 control-label">Password</asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="password" TextMode="Password" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="password"
                                        CssClass="text-danger" ErrorMessage="The password field is required."
                                        Display="Dynamic" ValidationGroup="SetPassword" />
                                    <asp:ModelErrorMessage runat="server" ModelStateKey="NewPassword" AssociatedControlID="password"
                                        CssClass="text-danger" SetFocusOnError="true" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="confirmPassword" CssClass="col-md-2 control-label">Confirm password</asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="confirmPassword" TextMode="Password" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="confirmPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required."
                                        ValidationGroup="SetPassword" />
                                    <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="confirmPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match."
                                        ValidationGroup="SetPassword" />

                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10">
                                    <asp:Button runat="server" Text="Set Password" ValidationGroup="SetPassword" OnClick="SetPassword_Click" CssClass="btn btn-default" />
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder runat="server" ID="changePasswordHolder" Visible="false">
                        <div class="form-horizontal">
                       
                            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
                            <div class="form-group">
                                <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword" CssClass="col-md-2 control-label">Şimdiki Şifre</asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                                        CssClass="text-danger" ErrorMessage="Şimdiki geçerli şifrenizi giriniz."
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword" CssClass="col-md-2 control-label">Yeni Şifre</asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                                        CssClass="text-danger" ErrorMessage="Lütfen şifre giriniz, bir adet büyük bir adet küçük harf bir adet sayı ve bir adet özel karakter giriniz"
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Yeni Şifre(Tekrar)</asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Yeni şifrenizi tekrar giriniz."
                                        ValidationGroup="ChangePassword" />
                                    <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Yeni şifreniz birbiriyle uyuşmuyor"
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10">
                                    <asp:Button runat="server" Text="Şifreyi Değiştir" ValidationGroup="ChangePassword" OnClick="ChangePassword_Click" CssClass="btn btn-primary btn-block" />
                                </div>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </section>
            </div>

        </div>
    
    </div>
</asp:Content>
