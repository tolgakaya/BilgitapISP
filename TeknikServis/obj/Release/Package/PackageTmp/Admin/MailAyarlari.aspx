<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="MailAyarlari.aspx.cs" Inherits="TeknikServis.Admin.MailAyarlari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">Mail Tema Ayarları</h4>
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
                                <label for="drdDurum" class="col-sm-2 control-label">İşlem seçiniz</label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="drdDurum" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drdDurum_SelectedIndexChanged">
                                        <asp:ListItem Text="Seçiniz..." Value="sec" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Yaklaşan Taksit" Value="yaklasan_taksit" Selected="False"></asp:ListItem>
                               
                                        <asp:ListItem Text="Yeni Servis Kaydi" Value="baslangic" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Servis Tamamlandı" Value="sonlanma" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Servis Kararı" Value="karar_bekleniyor" Selected="False"></asp:ListItem>
                                        <asp:ListItem Text="Servis Kararı Onaylandı" Value="karar_onaylandi" Selected="False"></asp:ListItem>
                                    </asp:DropDownList>

                                </div>

                            </div>
                            <div class="form-group">
                                <label for="txtGonderen" class="col-sm-2 control-label">Gönderen</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtGonderen" CausesValidation="true" runat="server" Columns="5" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="txtGonderen" ErrorMessage="Lütfen gönderen bilgisi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtKonu" class="col-sm-2 control-label">Mail Konu</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtKonu" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" EnableClientScript="true" ControlToValidate="txtKonu" ErrorMessage="Lütfen konu satırı giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtFirmaTam" class="col-sm-2 control-label">Tam Firma İsmi</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtFirmaTam" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" EnableClientScript="true" ControlToValidate="txtFirmaTam" ErrorMessage="Mailde görünecek bayi ismi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtUrl" class="col-sm-2 control-label">Geri Dönüş Url</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtUrl" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" EnableClientScript="true" ControlToValidate="txtUrl" ErrorMessage="Müşteriye sogulama alanı sağlamak için bir url giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtTelefon" class="col-sm-2 control-label">Firma Telefon No</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtTelefon" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" EnableClientScript="true" ControlToValidate="txtTelefon" ErrorMessage="Bayinin telefonun numarasını giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtAdres" class="col-sm-2 control-label">Firma Adresi</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtAdres" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" EnableClientScript="true" ControlToValidate="txtAdres" ErrorMessage="Bayinin adres bilgisini giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                         
                            <div class="form-group">
                                <label for="txtMesaj" class="col-sm-2 control-label">Mesaj İçeriği</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtMesaj" CausesValidation="true" runat="server" TextMode="MultiLine" Columns="5" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" ControlToValidate="txtMesaj" ErrorMessage="Lütfen mesaj giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="chcAktif" class="col-sm-2 control-label">Kullanım</label>
                                <div class="col-sm-10">
                                    <asp:CheckBox ID="chcAktif" CssClass="form-control" Text="Aktif olarak kullanılacak" runat="server" />
                                </div>
                            </div>
                            <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrup" CssClass="btn btn-primary btn-block" OnClick="btnKaydet_Click" />

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>
