<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="BayiAyarlari.aspx.cs" Inherits="TeknikServis.Admin.BayiAyarlari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--  <link rel="stylesheet" href="https://rawgit.com/enyo/dropzone/master/dist/dropzone.css" />--%>
    <link href="../Content/dropzone.css" rel="stylesheet" />
    <script src="../Scripts/Dropzone.js"></script>
    <script type="text/javascript">
        Dropzone.options.frmMain = {

            paramName: "file", // The name that will be used to transfer the file
            maxFilesize: 2, // MB
            url: "BayiAyarlari.aspx",
            acceptedFiles: "image/*",
            accept: function (file, done) {

                done();

            },
            init: function () {
                this.on("addedfile", function () {
                    if (this.files[1] != null) {
                        this.removeFile(this.files[0]);
                    }
                });
            },
        }

    </script>
    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">Yönetici/Firma Ayarları</h4>
            </div>

            <div class="panel-body">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>

                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>

                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="form-horizontal">

                            <div class="form-group">
                                <label for="FirmaTam" class="col-sm-2 control-label">Firma İsmi</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="FirmaTam" CausesValidation="true" runat="server" Columns="5" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="FirmaTam" ErrorMessage="Lütfen bayi firma ismini giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Tel" class="col-sm-2 control-label">Firma Telefonu</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="Tel" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" EnableClientScript="true" ControlToValidate="Tel" ErrorMessage="Lütfen bayi telefonunu giriniz." ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Email" class="col-sm-2 control-label">Firma Epostası</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="Email" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" EnableClientScript="true" ControlToValidate="Email" ErrorMessage="Bayi epostasını giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <%-- bayi web adresini kaydetmiyoruz çünkü o firmanın urlsinden alınıyor --%>
                            <div class="form-group">
                                <label for="txtUrl" class="col-sm-2 control-label">Firma Web</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtUrl" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" EnableClientScript="true" ControlToValidate="txtUrl" ErrorMessage="Müşteriye sogulama alanı sağlamak için bir url giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="Adres" class="col-sm-2 control-label">Firma Adresi</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="Adres" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" EnableClientScript="true" ControlToValidate="Adres" ErrorMessage="Bayinin adresini giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">

                                <label for="tarih2" class="col-sm-2 control-label">Son Fatura Tarihi</label>
                                <div class="col-sm-10">
                                    <input type='text' id="tarih2" runat="server" class="form-control" />
                                </div>

                            </div>
                             <div class="form-group">

                                 <label for="chcCiftTaraf" class="col-sm-2 control-label">Fatura nüshası</label>
                                      <div class="col-md-10">
                                          <asp:CheckBox ID="chcCiftTaraf" Text="Aynı sayfada" runat="server" CssClass="form-control" />

                                      </div>
                                  </div>
                            <div class="form-group">
                                <label for="fall" class="col-sm-2 control-label">LOGO</label>
                                <div id="frmMain" class="col-sm-10 dropzone">
                                    <div id="fall" class="fallback">

                                        <input name="file" type="file" />
                                        <asp:Label ID="lblFallbackMessage" runat="server" />
                                    </div>
                                </div>
                            </div>

                        </div>
                        <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrup" CssClass="btn btn-primary btn-block" OnClick="btnKaydet_Click" />

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>


        </div>
    </div>
</asp:Content>
