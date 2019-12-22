<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="KartExtre.aspx.cs" Inherits="TeknikServis.TeknikCari.KartExtre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

                            <asp:DropDownList ID="drdKart" runat="server" CssClass="pull-right text-danger" AutoPostBack="true" OnSelectedIndexChanged="drdAlinan_SelectedIndexChanged">
                                <asp:ListItem Text="Kart Seçiniz" Value="sec"></asp:ListItem>
                                <asp:ListItem Text="Bütün Kartlar" Value="hepsi"></asp:ListItem>

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
                        <div id="cariOzet" runat="server" class="pull-right">
                            <span id="txtAdet" runat="server" class="label label-warning"></span>
                            <span id="txtTutar" runat="server" class="label label-primary"></span>
                        </div>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="kart_id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated"
                            OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%-- <div class="visible-lg visible-xs visible-sm">--%>


                                        <asp:LinkButton ID="delLink"
                                            runat="server"
                                            CssClass="btn btn-danger btn-sm"
                                            CommandName="del" CommandArgument='<%#Eval("kart_id") %>' Text="<i class='fa fa-money'></i>" />

                                        <%--  </div>--%>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:TemplateField>


                                <asp:BoundField DataField="kart_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="kart_adi" HeaderText="Kart"></asp:BoundField>

                                <asp:BoundField DataField="musteri_adi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="tutar" HeaderText="Tutar" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="extre_tarih" HeaderText="Extre Tarihi" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm" DataFormatString="{0:D}">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>

                                <asp:BoundField DataField="tarih" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Musteri_Adi" HeaderText="Kişi Adı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>

                            </Columns>

                        </asp:GridView>

                    </div>
                    <div class="panel-footer pull-right">
                        <div class=" btn-group">

                            <asp:LinkButton ID="btnYeni"
                                runat="server"
                                CssClass="btn btn-info " OnClick="btnYeni_Click"
                                Text="Yeni Kart" />

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

                        </div>

                    </div>

                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="drdKart" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="drdCekildi" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnKasaKaydet" EventName="Click" />

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
                                        <i class="fa fa-2x">Extre ödemesi yapmak istiyor musunuz?</i>

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

        <!-- Add Record Modal Starts here-->
        <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="addModalLabel">Kart Tanımı</h3>
                </div>
                <asp:UpdatePanel ID="upAdd" runat="server">

                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtKartAdi" CssClass="col-md-4 control-label">Kart Adı</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtKartAdi" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtKartAdi" ErrorMessage="Kart adı giriniz."></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtKartAciklama" CssClass="col-md-4 control-label">Açıklama</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtKartAciklama" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtKartAciklama" ErrorMessage="Kart açıklaması giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtDevredenBakiye" CssClass="col-md-4 control-label">Bakiye</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtDevredenBakiye" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtDevredenBakiye" ErrorMessage="Lütfen bakiye giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtDevredenBakiye" Type="Currency" MinimumValue="0" MaximumValue="10000000" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">

                                    <label for="datetimepicker2" class="col-md-4 control-label">Extre Tarihi</label>
                                    <div class="input-group date col-md-8" id='datetimepicker2'>
                                        <%--<input type='text' id="tarih" runat="server" class="form-control col-md-10" />--%>
                                        <asp:TextBox runat="server" ID="tarih" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="tarih" ErrorMessage="Lütfen tarih giriniz"></asp:RequiredFieldValidator>

                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnKartKaydet" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" ValidationGroup="musteriGrup2" OnClick="btnKartKaydet_Click" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnYeni" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--Add Record Modal Ends here-->

    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#datetimepicker2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>
</asp:Content>
