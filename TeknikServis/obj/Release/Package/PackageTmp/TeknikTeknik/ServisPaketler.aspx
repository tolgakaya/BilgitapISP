<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ServisPaketler.aspx.cs" Inherits="TeknikServis.TeknikTeknik.ServisPaketler" %>
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
                Servis Hesap Paketleri
            </div>
            <div class="panel-body">
                <div class="panel-group" id="accordion">

                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">Paket Detayları</a>
                            </h4>
                        </div>
                        <div id="collapseTwo" class="panel-collapse in" style="height: auto;">
                            <div class="panel-body">

                                <!-- Ürün seçim alanı başlıyor -->

                                <div class="table-responsive">

                                    <asp:UpdatePanel ID="upCrudGrid2" runat="server">
                                        <ContentTemplate>

                                            <asp:GridView ID="grdDetay" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="detay_id"
                                                EmptyDataText="Detay bilgileri" EnablePersistedSelection="true" OnRowCommand="grdDetay_RowCommand" OnSelectedIndexChanged="grdDetay_SelectedIndexChanged">

                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>


                                                            <asp:LinkButton ID="delLink"
                                                                runat="server"
                                                                CssClass="btn btn-danger btn-xs"
                                                                CommandName="del" CommandArgument='<%#Eval("detay_id") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />


                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="detay_id" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="IslemParca" HeaderText="İşlem/Parça" />
                                                    <asp:BoundField DataField="Aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="cihaz_id" HeaderText="Ürün/Parça ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün/Parça" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="cihaz_gsure" HeaderText="Garanti Süresi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="adet" HeaderText="Adet" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="KDV" HeaderText="KDV(%)" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                    <asp:BoundField DataField="Yekun" HeaderText="Tutar(KDV Dahil)" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button Text="Detay Ekle" ID="btnDetayEkle" OnClick="btnDetayEkle_Click" CssClass="btn btn-info" runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnDetayKaydet" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>


                                </div>

                                <!-- Ürün seçimalanı bitiyor-->
                            </div>


                        </div>
                    </div>

                    <!-- detay Modal Starts here-->
                    <div id="detayModal" class="modal  fade" tabindex="-1" role="dialog"
                        aria-labelledby="detayModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-lg">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal"
                                    aria-hidden="true">
                                    ×</button>
                                <h3 id="detayModalLabel" class="baslik">Hesap Detayı</h3>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>

                                    <div class="modal-body">
                                        <div class="input-group custom-search-form col-md-12">
                                            <input runat="server" type="text" id="txtCihazAra" class="form-control" placeholder="Ara..." />
                                            <span class="input-group-btn">
                                                <button id="btnCihazAra" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
                                                    <i class="fa fa-search"></i>
                                                </button>
                                            </span>
                                        </div>


                                        <div class="form-horizontal" id="cihaz">
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="grdCihaz" CssClass="col-md-12 control-label">Ürün/Parça/Hizmet Seçimi</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:GridView ID="grdCihaz" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                                        EmptyDataText="Ürün/Parça seç" EnablePersistedSelection="true" OnRowCommand="grdCihaz_RowCommand" OnSelectedIndexChanged="grdCihaz_SelectedIndexChanged">

                                                        <SelectedRowStyle CssClass="danger" />
                                                        <Columns>

                                                            <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                                <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Ürün/Parça Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">

                                                                <ItemTemplate>


                                                                    <asp:LinkButton ID="btnRandom"
                                                                        runat="server"
                                                                        CssClass="btn btn-primary"
                                                                        CommandName="detail" CommandArgument='<%#Eval("ID") %>' Text=' <%#Eval("cihaz_adi") %> '>
                           
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>

                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="garanti_suresi" HeaderText="Garanti(AY)" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="giris" HeaderText="Toplam Giriş" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="cikis" HeaderText="Toplam Çıkış" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="bakiye" HeaderText="Stok Miktarı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>

                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:Button ID="btnYeniCihaz" runat="server" Text="Yeni Tanımla"
                                                        CssClass="btn btn-info" OnClick="btnYeniCihaz_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtIslemParca" CssClass="col-md-12 control-label">İşlem/Parça</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtIslemParca" ValidationGroup="detayGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtIslemParca" ValidationGroup="detayGrup" ErrorMessage="Lütfen işlem giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtCihaz" CssClass="col-md-12 control-label">Ürün/Parça</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtCihaz" ValidationGroup="detayGrup" CssClass="form-control" />
                                                    <asp:HiddenField ID="txtGarantiSure" runat="server" Value="12" />
                                                </div>
                                            </div>


                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtAdet" CssClass="col-md-12 control-label">Adet/Miktar</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtAdet" TextMode="Number" Text="1" ValidationGroup="detayGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtAdet" ValidationGroup="detayGrup" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtKdv" CssClass="col-md-12 control-label">Kdv Oranı</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtKdv" ValidationGroup="detayGrup" Text="18" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtKdv" ValidationGroup="detayGrup" ErrorMessage="Lütfen KDV giriniz"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtKDV" runat="server" ValidationGroup="detayGrup" MinimumValue="0" MaximumValue="9999999" />

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtYekun" CssClass="col-md-12 control-label">Yekün</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtYekun" ValidationGroup="detayGrup" Text="18" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtYekun" ValidationGroup="detayGrup" ErrorMessage="Lütfen toplam giriniz"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtYekun" runat="server" ValidationGroup="detayGrup" MinimumValue="0" MaximumValue="9999999" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtDetayAciklama" CssClass="col-md-12 control-label">Açıklama</asp:Label>
                                                <div class="col-md-12">
                                                    <asp:TextBox runat="server" ID="txtDetayAciklama" ValidationGroup="detayGrup" CssClass="form-control" />

                                                </div>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="modal-footer">
                                        <asp:Button ID="btnDetayKaydet" runat="server" Text="Detay Kaydet"
                                            CssClass="btn btn-info" OnClick="btnDetayKaydet_Click" ValidationGroup="detayGrup" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnDetayEkle" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCihazKaydet" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCihazAra" EventName="ServerClick" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <!-- Add Record Modal Starts here-->
                    <div id="cihazModal" class="modal  fade" tabindex="-1" role="dialog"
                        aria-labelledby="cihazModalLabel" aria-hidden="true">
                        <div class="modal-dialog modal-content modal-sm">
                            <div class="modal-header modal-header-info">
                                <button type="button" class="close" data-dismiss="modal"
                                    aria-hidden="true">
                                    ×</button>
                                <h3 id="cihazModalLabel" class="baslik">Yeni Ürün/Parça/Hizmet Tanımla</h3>
                            </div>
                            <asp:UpdatePanel ID="upAdd2" runat="server">
                                <ContentTemplate>
                                    <%--   <script type="text/javascript">
                                        Sys.Application.add_load(jScript);
                                    </script>--%>
                                    <div class="modal-body">
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="drdGrup" CssClass="col-md-2 control-label">Ürün Grubu</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="drdGrup" CssClass="form-control" runat="server">
                                                        <%--<asp:ListItem Text="Pos/Banka seçiniz" Value="-1"></asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="cihaz_adi" CssClass="col-md-10 control-label">Ürün/Parça Tanımı</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="cihaz_adi" ValidationGroup="cihazGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="cihaz_adi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen Ürün/Parça cinsi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="aciklama" CssClass="col-md-10 control-label">Ürün/Parça Açıklama</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="aciklama" ValidationGroup="cihazGrup" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="aciklama" ValidationGroup="cihazGrup" ErrorMessage="Lütfen açıklama giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="garanti_suresi" CssClass="col-md-10 control-label">Garanti Süresi(Ay)</asp:Label>
                                                <div class="col-md-10">
                                                    <asp:TextBox runat="server" ID="garanti_suresi" ValidationGroup="cihazGrup" TextMode="Number" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="garanti_suresi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen süre giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>


                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnCihazKaydet" runat="server" Text="Kaydet"
                                            CssClass="btn btn-info" OnClick="btnCihazKaydet_Click" ValidationGroup="cihazGrup" />
                                        <button class="btn btn-info" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnYeniCihaz" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!--Add Record Modal Ends here-->
                    <!--detay Modal Ends here-->
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" class="collapsed">Paket Bilgileri</a>
                            </h4>
                        </div>
                        <div id="collapseThree" class="panel-collapse collapse">
                            <div class="panel-body">


                                <!-- servis bilgileri başlıyor-->

                                <asp:UpdatePanel ID="upBilgi" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="form-group">
                                                <label class="col-sm-2 col-sm-2 control-label" for="txtPaketAdi">Paket Adı</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtPaketAdi" CausesValidation="true" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" EnableClientScript="true" ControlToValidate="txtPaketAdi" CssClass="text-danger" ErrorMessage="Lütfen Paket Adı giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="txtAciklama">Açıklama</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtAciklama" CausesValidation="true" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="5" ValidationGroup="valGrup"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" EnableClientScript="true" ControlToValidate="txtAciklama" CssClass="text-danger" ErrorMessage="Lütfen açıklama giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-sm-2 control-label" for="toplam_tutar">Tutar</label>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="toplam_tutar" CausesValidation="true" Enabled="false" runat="server" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                    </ContentTemplate>
                                    <Triggers>

                                        <asp:AsyncPostBackTrigger ControlID="btnAlimKaydet" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>


                                <asp:Button ID="btnAlimKaydet" CssClass="btn btn-info btn-lg btn-block" runat="server" CausesValidation="true" ValidationGroup="valGrup" Text="Kaydet" OnClick="btnAlimKaydet_Click" />


                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>

    </div>

</asp:Content>
