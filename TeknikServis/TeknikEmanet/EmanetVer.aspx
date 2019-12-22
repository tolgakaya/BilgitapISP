<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="EmanetVer.aspx.cs" Inherits="TeknikServis.EmanetVer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">Emanet Kaydet</h4>
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
                            <div id="yazdir" runat="server">
                                <div class="form-group">
                                    <label for="kisiBilgileri" class="col-sm-2 control-label">Emanet Verilen:</label>
                                    <div id="kisiBilgileri" class="col-sm-10">
                                        <span id="musteri" runat="server" class="label label-default"></span>
                                        <span id="tarih" runat="server" class="label label-default"></span>
                                        <span id="musteriEkBilgi" runat="server" class="label label-default"></span>
                                    </div>
                                </div>
                                

                                <div class="form-group">
                                    <label for="txtAciklama" class="col-sm-2 control-label">Açıklama</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtAciklama" CausesValidation="true" runat="server" TextMode="MultiLine" Columns="8" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" ControlToValidate="txtAciklama" ErrorMessage="Lütfen ürün giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="cbYazdir" class="col-sm-2 control-label">Emanet Makbuzu</label>
                                <div class="col-sm-10">
                                    <asp:CheckBox ID="cbYazdir" CssClass="checkbox-inline" runat="server" Text="Yazdır" />
                                </div>
                            </div>
                        </div>
                        <div class="btn-group pull-right">
                            <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrup" CssClass="btn btn-info" OnClick="btnKaydet_Click" />
                            <asp:Button ID="btnEmanetler" runat="server" Text="Müşteri Emanetleri" CausesValidation="true" CssClass="btn btn-info" OnClick="btnEmanetler_Click" />

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

        </div>
    </div>
</asp:Content>
