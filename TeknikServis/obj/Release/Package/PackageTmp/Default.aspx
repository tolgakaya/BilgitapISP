<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TeknikServis.Default" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>

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
    <script type="text/javascript">
        function doPostBack(o) {
            __doPostBack(o.id, '');
        }
    </script>
    <div class="row kaydir">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>



        <div class="panel panel-info">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Ürün/Parça Seçimi</a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse in" style="height: auto;">
                <div class="panel-body">

                    <!-- liste  alanı başlıyor -->

                    <div class="table-responsive">

                        <asp:UpdatePanel ID="upCrudGrid2" runat="server">
                            <ContentTemplate>
                                <div class="col-md-6 pull-right visible-lg ">
                                    <asp:TextBox runat="server" ID="barkod" CssClass="form-control" OnTextChanged="barkod_TextChanged" AutoPostBack="true" onkeyup="doPostBack(this);" placeholder="barkod" />
                                </div>
                                <div class="input-group custom-search-form col-md-6">
                                    <input runat="server" type="text" id="txtCihazAra" class="form-control" placeholder="Ara..." />
                                    <span class="input-group-btn">
                                        <button id="btnCihazAra" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
                                            <i class="fa fa-search"></i>
                                        </button>
                                    </span>
                                </div>


                                <div class="form-horizontal" id="cihaz">
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <asp:GridView ID="grdCihaz" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                                EmptyDataText="Cihaz seç" EnablePersistedSelection="true" OnRowCommand="grdCihaz_RowCommand" OnSelectedIndexChanged="grdCihaz_SelectedIndexChanged">

                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>


                                                            <asp:LinkButton ID="ekleLink"
                                                                runat="server"
                                                                CssClass="btn btn-success btn-xs"
                                                                CommandName="ekle" CommandArgument='<%#Eval("ID")+ ";" + Container.DisplayIndex  + ";" +Eval("grupid") %>' Text="<i class='fa fa-plus'></i>" />


                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün/Parça/Hizmet" />

                                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="bakiye" HeaderText="Stok" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="satis" HeaderText="Satış Fiyat" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="grupid" HeaderText="Grup" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>

                                        </div>

                                    </div>

                                </div>
                                <div class="panel panel-info">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">Alışveriş Sepeti
                                        </h4>
                                    </div>
                                    <div class="panel-body">


                                        <asp:GridView ID="grdDetay" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="cihaz_id"
                                            EmptyDataText="Detay bilgileri" EnablePersistedSelection="true" OnRowCommand="grdDetay_RowCommand" OnSelectedIndexChanged="grdDetay_SelectedIndexChanged">

                                            <SelectedRowStyle CssClass="danger" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>


                                                        <asp:LinkButton ID="delLink"
                                                            runat="server"
                                                            CssClass="btn btn-danger btn-xs"
                                                            CommandName="del" CommandArgument='<%#Eval("cihaz_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />


                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="cihaz_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                    <HeaderStyle CssClass="visible-lg" />
                                                    <ItemStyle CssClass="visible-lg" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün" />
                                                <asp:BoundField DataField="adet" HeaderText="Miktar" />
                                                <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                <asp:BoundField DataField="vergi_toplami" HeaderText="Vergiler" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                <asp:BoundField DataField="yekun" HeaderText="Yekün" />

                                            </Columns>
                                        </asp:GridView>

                                        <div class="form-horizontal">

                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="toplam_yekun">Yekün</label>
                                                <div class="col-sm-10">
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="toplam_yekun" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="toplam_yekun2" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>

                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" EnableClientScript="true" ControlToValidate="toplam_yekun" CssClass="text-danger" ErrorMessage="Lütfen vergi dahil tutar giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Ondalıklar için virgül kullanınız" ControlToValidate="toplam_yekun" ValidationGroup="valGrup" MinimumValue="0" MaximumValue="1000000" Type="Currency" runat="server" />
                                                </div>
                                            </div>

                                            <div class="col-sm-6">
                                                <asp:Button ID="btnAlimKaydet" CssClass="btn btn-info btn-block pull-right" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Nakit" OnClick="btnAlimKaydet_Click" />
                                            </div>
                                            <div class="col-sm-6">
                                                <asp:Button ID="btnKart" CssClass="btn btn-info btn-block  pull-left" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Kredi Kartı" OnClick="btnKart_Click" />

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>


                        </asp:UpdatePanel>


                    </div>

                    <!-- Liste alanı bitiyor-->
                </div>


            </div>
        </div>

        <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-sm">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">
                                        <i class="fa fa-2x">Kart Tahsilatı</i>

                                        <asp:DropDownList ID="drdPos" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Pos/Banka seçiniz" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>


                                        <div class="btn-group pull-right">

                                            <asp:Button ID="btnKartKaydet" runat="server" Text="Tamam"
                                                CssClass="btn btn-success" OnClick="btnKartKaydet_Click" />
                                            <button class="btn btn-warning" data-dismiss="modal"
                                                aria-hidden="true">
                                                Kapat</button>

                                        </div>
                                    </div>
                                </div>



                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnKartKaydet" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>



    </div>


</asp:Content>
