<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Kartlar.aspx.cs" Inherits="TeknikServis.TeknikCari.Kartlar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="kaydir">
        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h4 id="baslikkk" runat="server" class="panel-title">
                    <label id="baslik" runat="server">KART TANIMLARI</label>

                </h4>
            </div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <script type="text/javascript">
                        Sys.Application.add_load(jScript);
                    </script>
                    <div class="table-responsive">

                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="kart_id"
                            EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated"
                            OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnOde"
                                            runat="server"
                                            CssClass="btn btn-danger"
                                            CommandName="extre" CommandArgument='<%#Eval("kart_id") %>' Text="<i class='fa fa-list'></i>" />
                                        <asp:LinkButton ID="btnExtreOde"
                                            runat="server"
                                            CssClass="btn btn-success"
                                            CommandName="extre" CommandArgument='<%#Eval("kart_id") %>' Text="<i class='fa fa-money'></i>" />
                                        <asp:LinkButton ID="btnOdemeler"
                                            runat="server"
                                            CssClass="btn btn-primary"
                                            CommandName="extre" CommandArgument='<%#Eval("kart_id") %>' Text="<i class='fa fa-money'></i>" />

                                        <asp:LinkButton ID="btnGuncelle"
                                            runat="server"
                                            CssClass="btn btn-info"
                                            CommandName="guncel" CommandArgument='<%#Eval("kart_id") + ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />

                                    </ItemTemplate>
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:TemplateField>


                                <asp:BoundField DataField="kart_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>

                                <asp:BoundField DataField="kart_adi" HeaderText="Kart" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="extre_tarih" HeaderText="Extre Tarihi" DataFormatString="{0:d}">
                                    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                    <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                </asp:BoundField>
                                <asp:BoundField DataField="bakiye" HeaderText="Bakiye" DataFormatString="{0:C}">
                                    <HeaderStyle CssClass="visible-lg" />
                                    <ItemStyle CssClass="visible-lg" />
                                </asp:BoundField>
                            </Columns>

                        </asp:GridView>

                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnKartKaydetup" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnKartKaydet" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnYeni"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnEkle_Click"
                        Text="Yeni Kart" />
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

                </div>

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
                    <h3 id="addModalLabel" class="baslik">Kart Tanımı</h3>
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
                                <%--             <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtDevredenBakiye" CssClass="col-md-4 control-label">Bakiye</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtDevredenBakiye" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtDevredenBakiye" ErrorMessage="Lütfen bakiye giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtDevredenBakiye" Type="Currency" MinimumValue="0" MaximumValue="10000000" runat="server" />
                                    </div>
                                </div>--%>

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
        <div id="upModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="upModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="upModalLabel" class="baslik">Kart Tanımı</h3>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtKartAdiup" CssClass="col-md-4 control-label">Kart Adı</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtKartAdiup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="musteriGrup22" ControlToValidate="txtKartAdiup" ErrorMessage="Kart adı giriniz."></asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdnKartID" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtKartAciklamaup" CssClass="col-md-4 control-label">Açıklama</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtKartAciklamaup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="musteriGrup22" ControlToValidate="txtKartAciklamaup" ErrorMessage="Kart açıklaması giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <asp:HiddenField ID="hdnBakiye" runat="server" />
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtDevredenBakiyeup" CssClass="col-md-4 control-label">Bakiye</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtDevredenBakiyeup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="musteriGrup22" ControlToValidate="txtDevredenBakiyeup" ErrorMessage="Lütfen bakiye giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ValidationGroup="musteriGrup22" ControlToValidate="txtDevredenBakiyeup" Type="Currency" MinimumValue="0" MaximumValue="10000000" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">

                                    <label for="datetimepicker22" class="col-md-4 control-label">Extre Tarihi</label>
                                    <div class="input-group date col-md-8" id='datetimepicker22'>
                                        <%--<input type='text' id="tarih" runat="server" class="form-control col-md-10" />--%>
                                        <asp:TextBox runat="server" ID="tarihup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="musteriGrup22" ControlToValidate="tarihup" ErrorMessage="Lütfen tarih giriniz"></asp:RequiredFieldValidator>

                                        <span class="input-group-addon">
                                            <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnKartKaydetup" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" ValidationGroup="musteriGrup22" OnClick="btnKartKaydetup_Click" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>


    </div>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#datetimepicker2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

            $('#datetimepicker22').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>

</asp:Content>
