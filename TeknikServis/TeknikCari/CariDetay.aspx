<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="CariDetay.aspx.cs" Inherits="TeknikServis.CariDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->


            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">MÜŞTERİ EXTRESİ</label>

                    <asp:DropDownList ID="drdKritik" runat="server" CssClass="pull-right text-info" AutoPostBack="true" OnSelectedIndexChanged="drdKritik_SelectedIndexChanged">
                        <asp:ListItem Text="Şu tarihe kadar" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Haftalık" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Aylık" Value="30"></asp:ListItem>
                        <asp:ListItem Text="Üç Aylık" Value="90"></asp:ListItem>
                        <asp:ListItem Text="Altı Aylık" Value="180"></asp:ListItem>
                        <asp:ListItem Text="Yıllık" Value="365"></asp:ListItem>
                        <asp:ListItem Text="Hepsi" Value="3000"></asp:ListItem>
                    </asp:DropDownList>


                </h4>
            </div>
            <div class="table-responsive">

                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="musteriAdi"
                    EmptyDataText="Kayıt girilmemiş" OnPageIndexChanging="GridView2_PageIndexChanging" OnRowCreated="GridView2_RowCreated" OnRowDataBound="GridView2_RowDataBound" AllowPaging="true" PageSize="50">
                    <PagerStyle CssClass="pagination-ys" />
                    <SelectedRowStyle CssClass="danger" />
                    <Columns>

                        <asp:BoundField DataField="musteriAdi" HeaderText="Kişi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>

                        <asp:BoundField DataField="borc" HeaderText="Borç" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                        </asp:BoundField>
                        <asp:BoundField DataField="alacak" HeaderText="Alacak" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                        </asp:BoundField>
                        <asp:BoundField DataField="tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                        </asp:BoundField>
                        <asp:BoundField DataField="islem" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Konu" HeaderText="Konu" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                        </asp:BoundField>
                          <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                        </asp:BoundField>
                          <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg visible-sm visible-xs" ItemStyle-CssClass="visible-lg visible-sm visible-xs">
                            <HeaderStyle CssClass="visible-lg visible-sm visible-xs" />
                            <ItemStyle CssClass="visible-lg visible-sm visible-xs" />
                        </asp:BoundField>

                    </Columns>

                </asp:GridView>

            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">


                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnPrnt_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />


                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />



                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />

                    <asp:LinkButton ID="btnMusteriDetayim"
                        runat="server" Visible="false"
                        CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                        Text="<i class='fa fa-user icon-2x'></i>" />

                </div>

            </div>
        </div>
    </div>
</asp:Content>
