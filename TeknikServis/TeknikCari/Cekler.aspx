<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Cekler.aspx.cs" Inherits="TeknikServis.TeknikCari.Cekler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("Kasa üzerinden ödeme kaydediyorsunuz?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }


    </script>

    <div class="kaydir">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="panel panel-info">

                    <!-- Default panel contents -->
                    <div class="panel-heading">
                        <h4 id="baslikkk" runat="server" class="panel-title">
                            <label id="baslikk" runat="server">ÖDEME VE TAHSİLATLAR</label>
                            <asp:DropDownList ID="drdKritik" runat="server" CssClass="pull-right text-info" AutoPostBack="true" OnSelectedIndexChanged="drdKritik_SelectedIndexChanged">
                                <asp:ListItem Text="Şu tarihe kadar" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Haftalık" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Aylık" Value="30"></asp:ListItem>
                                <asp:ListItem Text="Üç Aylık" Value="90"></asp:ListItem>
                                <asp:ListItem Text="Altı Aylık" Value="180"></asp:ListItem>
                                <asp:ListItem Text="Yıllık" Value="365"></asp:ListItem>
                                <asp:ListItem Text="Hepsi" Value="3000"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="drdAlinan" runat="server" CssClass="pull-right text-danger" AutoPostBack="true" OnSelectedIndexChanged="drdAlinan_SelectedIndexChanged">
                                <asp:ListItem Text="Alınan/Verilene göre" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Alınanlar" Value="true"></asp:ListItem>
                                <asp:ListItem Text="Verilenler" Value="false"></asp:ListItem>
                                <asp:ListItem Text="Hepsi" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="drdCekildi" runat="server" CssClass="pull-right text-danger" AutoPostBack="true" OnSelectedIndexChanged="drdCekildi_SelectedIndexChanged">
                                <asp:ListItem Text="Tahsil Edilme/Ödemeye göre" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Tahsil Edilen/Ödenen" Value="true"></asp:ListItem>
                                <asp:ListItem Text="Tahsil Edilmeyen/Ödenmeyen" Value="false"></asp:ListItem>
                                <asp:ListItem Text="Hepsi" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </h4>
                    </div>
                    <%--<div class="panel-body">
               
            </div>--%>
                    <div class="table-responsive">

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="cek_id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated"
                            OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%-- <div class="visible-lg visible-xs visible-sm">--%>
                                        <asp:LinkButton ID="bankaLink"
                                            runat="server"
                                            CssClass="btn btn-success btn-sm"
                                            CommandName="bank" CommandArgument='<%#Eval("cek_id") %>' Text="<i class='fa fa-money'></i>" />

                                        <asp:LinkButton ID="delLink"
                                            runat="server"
                                            CssClass="btn btn-danger btn-sm"
                                            CommandName="del" CommandArgument='<%#Eval("cek_id") %>' Text="<i class='fa fa-money'></i>" />

                                        <%--  </div>--%>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:TemplateField>


                                <asp:BoundField DataField="cek_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Musteri_ID" HeaderText="Musteri_ID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                    <HeaderStyle CssClass="gizlisutun" />
                                    <ItemStyle CssClass="gizlisutun" />
                                </asp:BoundField>
                                <asp:BoundField DataField="belge_no" HeaderText="belge_no"></asp:BoundField>
                                <asp:BoundField DataField="musteri_adi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tutar" HeaderText="Miktar" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vade_tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm" DataFormatString="{0:D}">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="masraf" HeaderText="Masraf" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="nettutar" HeaderText="Net Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tahsilat_tarih" HeaderText="Tahsilat" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="alinan" HeaderText="Alınan/Verilen" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>


                            </Columns>

                        </asp:GridView>

                    </div>
                    <div class="panel-footer pull-right">
                        <div class=" btn-group">

                            <asp:Button ID="btnEkle" runat="server" Text="Yeni" CssClass="btn btn-danger"
                                OnClick="btnEkle_Click" />

                            <asp:LinkButton ID="btnPrint"
                                runat="server"
                                CssClass="btn btn-info " OnClick="btnPrnt_Click"
                                Text="<i class='fa fa-print icon-2x'></i>" />


                            <asp:LinkButton ID="btnExportExcel"
                                runat="server"
                                CssClass="btn btn-warning " OnClick="btnExportExcel_Click"
                                Text="<i class='fa fa-file-excel-o icon-2x'></i>" />



                            <asp:LinkButton ID="btnExportWord"
                                runat="server"
                                CssClass="btn btn-primary " OnClick="btnExportWord_Click"
                                Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />

                            <asp:LinkButton ID="btnMusteriDetayim"
                                runat="server" Visible="false"
                                CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                                Text="<i class='fa fa-user icon-2x'></i>" />


                        </div>

                    </div>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="drdKritik" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="drdAlinan" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="drdCekildi" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnKasaKaydet" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnBankaKaydet" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-sm">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">
                                        <i class="fa fa-2x">Aktif kasa üzerinden ödeme/tahsilatı onaylayor musunuz?</i>

                                        <div class="btn-group pull-right">

                                            <asp:Button ID="btnKasaKaydet" runat="server" Text="Tamam"
                                                CssClass="btn btn-success" OnClick="btnKasaKaydet_Click" />
                                            <button class="btn btn-warning" data-dismiss="modal"
                                                aria-hidden="true">
                                                Kapat</button>

                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdnCekID" runat="server" />

                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="bankaModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="bankaModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-sm">

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">
                                        <i class="fa fa-2x">Banka seçiniz!</i>
                                        <asp:DropDownList ID="drdBanka" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Banka seçiniz" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <div class="btn-group pull-right">

                                            <asp:Button ID="btnBankaKaydet" runat="server" Text="Tamam"
                                                CssClass="btn btn-success" OnClick="btnBankaKaydet_Click" />
                                            <button class="btn btn-warning" data-dismiss="modal"
                                                aria-hidden="true">
                                                Kapat</button>

                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdnCekIdBanka" runat="server" />

                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
