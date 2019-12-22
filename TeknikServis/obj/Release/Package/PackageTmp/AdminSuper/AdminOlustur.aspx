<%@ Page Title="" Language="C#" MasterPageFile="Super.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="AdminOlustur.aspx.cs" Inherits="TeknikServis.AdminSuper.AdminOlustur" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2><%: Title %></h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
               FİRMA KAYDI
            </div>
            <div class="panel-body">

                <div class="form-horizontal">
                   

                    <asp:ValidationSummary runat="server" CssClass="text-danger" />
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtConfig" CssClass="col-md-2 control-label">Config</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtConfig" Enabled="false" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfig"
                                CssClass="text-danger" ErrorMessage="Bir config giriniz." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="txtAy" CssClass="col-md-2 control-label">Lisans Süresi(Ay)</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtAy" TextMode="Number" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAy"
                                CssClass="text-danger" ErrorMessage="Lütfen lisans süresi giriniz" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Firma" CssClass="col-md-2 control-label">Firma</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Firma" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Firma"
                                CssClass="text-danger" ErrorMessage="Bir firma adı giriniz." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="The email field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">Kullanıcı Adı</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="Kullanıcı adı giriniz." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Adres" CssClass="col-md-2 control-label">Adres</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Adres" CssClass="form-control" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Adres"
                                CssClass="text-danger" ErrorMessage="The adres field is required." />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Telefon" CssClass="col-md-2 control-label">Telefon</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Telefon" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Telefon"
                                CssClass="text-danger" ErrorMessage="The telefon field is required." />
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Web" CssClass="col-md-2 control-label">Web</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Web" CssClass="form-control" TextMode="Url" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Web"
                                CssClass="text-danger" ErrorMessage="The web field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TamFirma" CssClass="col-md-2 control-label">Tam Firma Adı</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="TamFirma" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="TamFirma"
                                CssClass="text-danger" ErrorMessage="The firma adı field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="The password field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirm password</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                            <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" ID="btnFelan2" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>


            </div>
        </div>

    </div>
</asp:Content>
