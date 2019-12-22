<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="OdemeTahsilatlar.aspx.cs" Inherits="TeknikServis.TeknikCari.OdemeTahsilatlar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("Kaydı silmek istiyor musunuz?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }


    </script>

    <div class="kaydir">
        <div class="row">
            <!--quick info section -->
            <div class="col-lg-4">
                <div class="alert alert-info text-center">
                    <i class="fa fa-balance-scale fa-3x" id="txtKasa" runat="server"></i>&nbsp; Kasa
                </div>
            </div>
            <div class="col-lg-4">
                <div class="alert alert-success text-center">
                    <i class="fa  fa-university fa-3x" id="txtBanka" runat="server"></i>&nbsp; Banka
                </div>
            </div>
            <div class="col-lg-4">
                <div class="alert alert-info text-center">
                    <i class="fa fa-cc-visa fa-3x" id="txtPos" runat="server"></i>&nbsp; POS

                </div>
            </div>

        </div>
        <div class="row">
            <!--quick info section -->
            <div class="col-lg-4">
                <div class="alert alert-info text-center">
                    <i class="fa fa-calendar fa-3x" id="txtVerilenCek" runat="server"></i>&nbsp; Verilen/Çek
                </div>
            </div>
            <div class="col-lg-4">
                <div class="alert alert-success text-center">
                    <i class="fa  fa-thumbs-up fa-3x" id="txtAlinanCek" runat="server"></i>&nbsp; Alınan/Çek
                </div>
            </div>
            <div class="col-lg-4">
                <div class="alert alert-info text-center">
                    <i class="fa fa-credit-card fa-3x" id="txtExtre" runat="server"></i>&nbsp; Kart/Extre

                </div>
            </div>

        </div>
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">ÖDEME VE TAHSİLATLAR</label>
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
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive">

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                    DataKeyNames="odemeID"
                    EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated"
                    OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" PageSize="10">
                    <PagerStyle CssClass="pagination-ys" />
                    <Columns>

                        <asp:TemplateField>
                            <ItemTemplate>

                                <asp:LinkButton ID="btnOde"
                                    runat="server"
                                    CssClass="btn btn-primary btn-xs visible-lg"
                                    Text="<i class='fa fa-money'></i>" />

                                <asp:LinkButton ID="delLink"
                                    runat="server"
                                    CssClass="btn btn-danger btn-xs visible-lg"
                                    CommandName="del" CommandArgument='<%#Eval("odemeID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />


                                <asp:LinkButton ID="btnOdeK"
                                    runat="server"
                                    CssClass="btn btn-primary visible-xs visible-sm"
                                    Text="<i class='fa fa-money'></i>" />

                                <asp:LinkButton ID="delLinkK"
                                    runat="server"
                                    CssClass="btn btn-danger visible-xs visible-sm"
                                    CommandName="del" CommandArgument='<%#Eval("odemeID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />



                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:BoundField DataField="odemeID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="kullaniciID" HeaderText="kullaniciID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                            <HeaderStyle CssClass="gizlisutun" />
                            <ItemStyle CssClass="gizlisutun" />
                        </asp:BoundField>
                        <asp:BoundField DataField="musteriID" HeaderText="musteriID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                            <HeaderStyle CssClass="gizlisutun" />
                            <ItemStyle CssClass="gizlisutun" />
                        </asp:BoundField>
                        <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                        </asp:BoundField>
                        <asp:BoundField DataField="odemeMiktari" HeaderText="Miktar" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                        </asp:BoundField>
                        <asp:BoundField DataField="odemeTarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                        </asp:BoundField>
                        <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="tahsilatOdeme_turu" HeaderText="Tür" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="tahsilat_odeme" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="islem_adres" HeaderText="İşlem Yeri" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="islem_tarihi" HeaderText="İşlem Tarihi" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
                        </asp:BoundField>
                        <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg">
                            <HeaderStyle CssClass="visible-lg" />
                            <ItemStyle CssClass="visible-lg" />
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
