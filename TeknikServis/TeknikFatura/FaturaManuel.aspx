<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="FaturaManuel.aspx.cs" Inherits="TeknikServis.FaturaManuel" %>

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

        <div class="panel panel-info">
            <div class="panel-heading">
                MANUEL FATURA BASIMI
            </div>
            <div class="panel-body">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="panel-group" id="accordion">

                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Fatura Kalemleri</a>
                            </h4>
                        </div>
                        <%-- servis tipleri başlıyor --%>
                        <div id="collapseTwo" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grdTip" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdTip_RowDataBound" OnRowCommand="grdTip_RowCommand" CssClass="table table-bordered table-hover" DataKeyNames="cinsi" EmptyDataText="Kayıt girilmemiş">
                                                <Columns>

                                                    <asp:TemplateField HeaderText="İşlem" ShowHeader="False">
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="delLink"
                                                                runat="server"
                                                                CssClass="btn btn-danger btn-xs"
                                                                CommandName="del" CommandArgument='<%#Eval("cinsi") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />

                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="cinsi" HeaderText="Kalem" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="mik" HeaderText="Adet" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tutar" HeaderText="Vergisiz Tutar" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>

                                                </Columns>

                                            </asp:GridView>
                                            <asp:Button ID="btnAddTip" runat="server" Text="Yeni" CssClass="btn btn-info"
                                                OnClick="btnAddTip_Click" />
                                        </ContentTemplate>
                                        <%--<Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnARATip" EventName="ServerClick" />
                                            </Triggers>--%>
                                    </asp:UpdatePanel>


                                    <!-- Add Record Modal Starts here-->
                                    <div id="addModalTip" class="modal  fade" tabindex="-1" role="dialog"
                                        aria-labelledby="addModalLabelTip" aria-hidden="true">
                                        <div class="modal-dialog modal-content modal-sm">
                                            <div class="modal-header modal-header-info">
                                                <button type="button" class="close" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    ×</button>
                                                <h3 id="addModalLabelTip">Fatura Kalemi</h3>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <script type="text/javascript">
                                                        Sys.Application.add_load(jScript);
                                                    </script>
                                                    <div class="modal-body">


                                                        <div class="form-group">
                                                            <asp:Label runat="server" AssociatedControlID="txtCinsi" CssClass="col-md-10 control-label">Ürün/Hizmet</asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:TextBox runat="server" ID="txtCinsi" CssClass="form-control" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ValidationGroup="tipGrupAdd" ControlToValidate="txtCinsi" ErrorMessage="Ürün/Hizmet giriniz"></asp:RequiredFieldValidator>

                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:Label runat="server" AssociatedControlID="txtMik" CssClass="col-md-10 control-label">Miktar</asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:TextBox runat="server" ID="txtMik" TextMode="Number" CssClass="form-control" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ValidationGroup="tipGrupAdd" ControlToValidate="txtMik" ErrorMessage="Miktar giriniz"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:Label runat="server" AssociatedControlID="txtFiyat" CssClass="col-md-10 control-label">Birim Fiyat</asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:TextBox runat="server" ID="txtFiyat" CssClass="form-control" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ValidationGroup="tipGrupAdd" ControlToValidate="txtFiyat" ErrorMessage="Fiyat giriniz"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="tipGrupAdd" ControlToValidate="txtFiyat" MaximumValue="10000000" MinimumValue="0" ErrorMessage="Küsurat için virgül kullanınız"></asp:RangeValidator>

                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:Label runat="server" AssociatedControlID="txtTutar" CssClass="col-md-10 control-label">Vergisiz Tutar</asp:Label>
                                                            <div class="col-md-10">
                                                                <asp:TextBox runat="server" ID="txtTutar" CssClass="form-control" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="tipGrupAdd" ControlToValidate="txtCinsi" ErrorMessage="Tutar giriniz"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ID="RangeValidator2" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="tipGrupAdd" ControlToValidate="txtTutar" MaximumValue="10000000" MinimumValue="0" ErrorMessage="Küsurat için virgül kullanınız"></asp:RangeValidator>

                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="modal-footer">
                                                        <asp:Button ID="btnAddRecordTip" runat="server" Text="Kaydet"
                                                            CssClass="btn btn-info" OnClick="btnAddRecordTip_Click" ValidationGroup="tipGrupAdd" />
                                                        <button class="btn btn-info" data-dismiss="modal"
                                                            aria-hidden="true">
                                                            Kapat</button>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddRecordTip" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <!--Add Record Modal Ends here-->
                                </div>

                            </div>
                        </div>
                        <%-- servis tipleri bitiyor --%>
                    </div>
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" class="collapsed">Fatura Bilgileri</a>
                            </h4>
                        </div>

                        <div id="collapseThree" class="panel-collapse collapse">
                            <div class="panel-body">

                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <h3>Fatura Bilgileri</h3>
                                        <div class="form-group">
                                            <label for="txtIsim" class="control-label">Müşteri Adı</label>
                                            <input id="txtIsim" runat="server" type="text" class="form-control" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="txtIsim" ErrorMessage="Lütfen müşteri adı giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtTC" class="control-label">Tc No</label>
                                            <asp:TextBox ID="txtTC" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" ControlToValidate="txtTC" ErrorMessage="Tc no yada vergi numarası giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator3" runat="server" Type="Double" EnableClientScript="true" ValidationGroup="valGrup" ControlToValidate="txtTC" MaximumValue="99999999999" MinimumValue="10000000000" ErrorMessage="Geçerli bir TC giriniz"></asp:RangeValidator>

                                        </div>
                                        <div class="form-group">
                                            <label for="txtVD" class="control-label">Vergi Dairesi</label>
                                            <asp:TextBox ID="txtVD" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" EnableClientScript="true" ControlToValidate="txtVD" ErrorMessage="Vergi dairesi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>

                                        </div>
                                        <div class="form-group">
                                            <label for="txtTutar2" class="control-label">Tutar</label>
                                            <asp:TextBox ID="txtTutar2" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" EnableClientScript="true" ControlToValidate="txtTutar2" ErrorMessage="Fatura toplam tutarını(Vergisiz) giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator4" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="valGrup" ControlToValidate="txtTutar2" MaximumValue="100000000" MinimumValue="1" ErrorMessage="Küsüratlar için virgül kullanınız"></asp:RangeValidator>

                                        </div>
                                        <div class="form-group">
                                            <label for="txtKDV2" class="control-label">KDV Tutarı</label>
                                            <asp:TextBox ID="txtKDV2" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" EnableClientScript="true" ControlToValidate="txtKDV2" ErrorMessage="Toplam KDV tutarını giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator5" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="valGrup" ControlToValidate="txtKDV2" MaximumValue="100000000" MinimumValue="0" ErrorMessage="Küsüratlar için virgül kullanınız"></asp:RangeValidator>

                                        </div>
                                        <div class="form-group">
                                            <label for="txtOIV2" class="control-label">ÖİV Tutarı</label>
                                            <asp:TextBox ID="txtOIV2" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" EnableClientScript="true" ControlToValidate="txtOIV2" ErrorMessage="ÖİV tutarını giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator6" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="valGrup" ControlToValidate="txtOIV2" MaximumValue="100000000" MinimumValue="0" ErrorMessage="Küsüratlar için virgül kullanınız"></asp:RangeValidator>

                                        </div>
                                        <div class="form-group">
                                            <label for="txtYekun2" class="control-label">Yekun</label>
                                            <asp:TextBox ID="txtYekun2" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" EnableClientScript="true" ControlToValidate="txtYekun2" ErrorMessage="Yekun tutarı giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator7" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="valGrup" ControlToValidate="txtYekun2" MaximumValue="100000000" MinimumValue="1" ErrorMessage="Küsüratlar için virgül kullanınız"></asp:RangeValidator>

                                        </div>
                                        <div class="form-group">

                                            <label for="tarih2" class="col-sm-2 control-label">Fatura Tarihi</label>
                                            <div class="col-sm-12">

                                                <input type='text' id="tarih2" runat="server" class="form-control" />

                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnKaydet" runat="server" Text="Yazdır" CssClass="btn btn-info btn-block" OnClick="btnKaydet_Click" ValidationGroup="valGrup" />

                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAddRecordTip" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>

                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>


    <script type="text/javascript">
        function pageLoad(sender, args) {

            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>
</asp:Content>
