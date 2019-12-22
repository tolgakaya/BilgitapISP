<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="MesajGonder.aspx.cs" Inherits="TeknikServis.MesajGonder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h4 id="baslik" runat="server" class="panel-title"></h4>
            </div>
            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                <ProgressTemplate>

                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                        <asp:Image ID="imgUpdateProgress3" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                    </div>

                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="txtGonderen" class="col-sm-2 control-label">Gönderen</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtGonderen" CausesValidation="true" runat="server" Columns="5" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="txtGonderen" ErrorMessage="Lütfen gönderen bilgisi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="txtMesaj" class="col-sm-2 control-label">Mesaj İçeriği</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtMesaj" CausesValidation="true" runat="server" TextMode="MultiLine" Columns="5" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" ControlToValidate="txtMesaj" ErrorMessage="Lütfen mesaj giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <asp:Button ID="btnKaydet" runat="server" Text="GÖNDER" CausesValidation="true" ValidationGroup="valGrup" CssClass="btn btn-info btn-block" OnClick="btnKaydet_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div class="panel-footer">
                <div class="alert alert-info">
                    <asp:TextBox ID="liste" CausesValidation="true" runat="server" TextMode="MultiLine" Columns="5" CssClass="form-control"></asp:TextBox>
                    <%--<h4 id="liste" runat="server"></h4>--%>
                </div>
                <div id="istihbarat" runat="server" visible="false" class="alert alert-info">
                    <%-- KULLANİCİ BAŞLIYOR --%>
                    <h3 id="alarm" runat="server"></h3>


                    <%-- KULLANICI BİTİYOR --%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
