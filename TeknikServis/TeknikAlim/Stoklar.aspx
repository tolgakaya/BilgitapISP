<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Stoklar.aspx.cs" Inherits="TeknikServis.TeknikAlim.Stoklar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="kaydir">


        <div class="panel panel-info">
            <!-- Default panel contents -->
            <div class="panel-heading">
                Ürün ve Yedek Parça Stokları

            </div>
            <%--<div class="panel-body">
               
            </div>--%>
            <div class="table-responsive ">
                <div class="input-group custom-search-form">
                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />
                    <span class="input-group-btn">
                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <asp:UpdatePanel ID="upCrudGrid" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="ID"
                            EmptyDataText="Stok kaydı" OnRowCommand="grdAlimlar_RowCommand"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="grdAlimlar_PageIndexChanging" OnRowCreated="grdAlimlar_RowCreated" OnRowDataBound="grdAlimlar_RowDataBound">

                            <PagerStyle CssClass="pagination-ys" />
                            <Columns>

                                <%-- <%# Container.DataItemIndex %>' Text="<i class='fa fa-pencil'></i>" /> --%>
                                <asp:TemplateField HeaderStyle-Width="120">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="btnDetay"
                                            runat="server"
                                            CssClass="btn  btn-success btn-xs"
                                            CommandName="detay" CommandArgument='<%# Eval("ID") %>' Text="<i class='fa fa-bar-chart'></i>" />
                                        <asp:LinkButton ID="btnMusteriler"
                                            runat="server"
                                            CssClass="btn  btn-info btn-xs"
                                            CommandName="musteri" CommandArgument='<%# Eval("ID") %>' Text="<i class='fa fa-user'></i>" />
                                        <asp:LinkButton ID="btnGuncelle"
                                            runat="server"
                                            CssClass="btn btn-danger btn-xs"
                                            CommandName="guncelle" CommandArgument='<%#Eval("ID")+ ";" + Container.DisplayIndex +";"+Eval("grupid")  %>' Text="<i class='fa fa-pencil'></i>" />

                                    </ItemTemplate>


                                </asp:TemplateField>

                                <asp:BoundField DataField="cihaz_adi" HeaderText="Ürün/Parça"></asp:BoundField>
                                <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="garanti_suresi" HeaderText="Garanti(AY)" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                <asp:BoundField DataField="giris" HeaderText="Giriş" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="cikis" HeaderText="Çıkış" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                <asp:BoundField DataField="bakiye" HeaderText="Stok" />
                                <asp:BoundField DataField="fiyat" HeaderText="Alış Fiyatı" />
                                <asp:BoundField DataField="satis" HeaderText="Satış Fiyatı" />
                                <asp:BoundField DataField="barkod" HeaderText="Barkod" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />

                            </Columns>

                        </asp:GridView>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAra" EventName="ServerClick" />

                    </Triggers>
                </asp:UpdatePanel>


            </div>
            <div class="panel-footer pull-right">
                <div class=" btn-group">

                    <asp:LinkButton ID="btnYeni"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnYeni_Click"
                        Text="Yeni Ürün" />
                    <asp:LinkButton ID="btnGrup"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnGrup_Click"
                        Text="Yeni Grup" />


                    <asp:LinkButton ID="btnPrint"
                        runat="server"
                        CssClass="btn btn-info visible-lg " OnClick="btnPrnt_Click"
                        Text="<i class='fa fa-print icon-2x'></i>" />
                    <asp:LinkButton ID="btnExportExcel"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportExcel_Click"
                        Text="<i class='fa fa-file-excel-o icon-2x'></i>" />

                    <asp:LinkButton ID="btnExportWord"
                        runat="server"
                        CssClass="btn btn-info visible-lg" OnClick="btnExportWord_Click"
                        Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />

                </div>


            </div>

        </div>
        <!-- yeni ürün Starts here-->
        <div id="cihazModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="cihazModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="cihazModalLabel" class="baslik">Yeni Cihaz/Malzeme Tanımla</h3>
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
                                    <asp:Label runat="server" AssociatedControlID="cihaz_adi" CssClass="col-md-2 control-label">Cihaz Tanımı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="cihaz_adi" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="cihaz_adi" ValidationGroup="cihazGrup" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="aciklama" CssClass="col-md-2 control-label">Cihaz Açıklama</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="aciklama" ValidationGroup="cihazGrup" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="aciklama" ValidationGroup="cihazGrup" ErrorMessage="Lütfen açıklama giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="barcode" CssClass="col-md-2 control-label">Barkod</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="barcode" ValidationGroup="cihazGrup" CssClass="form-control" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="garanti_suresi" CssClass="col-md-2 control-label">Garanti Süresi(Ay)</asp:Label>
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
                        <asp:AsyncPostBackTrigger ControlID="btnYeni" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <%-- yeni ürün bitiyor --%>
        <%-- yeni grup başlıyor --%>
        <div id="grupModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="cihazModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="grupModalLabel" class="baslik">Yeni Cihaz/Malzeme Grubu</h3>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <%--   <script type="text/javascript">
                                        Sys.Application.add_load(jScript);
                                    </script>--%>
                        <div class="modal-body">
                            <div class="form-horizontal">


                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="grupadig" CssClass="col-md-2 control-label">Grup İsmi</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="grupadig" ValidationGroup="cihazGrupf" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="grupadig" ValidationGroup="cihazGrupg" ErrorMessage="Lütfen grup adı giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="kdvg" CssClass="col-md-2 control-label">Kdv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="kdvg" ValidationGroup="cihazGrupg" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="kdvg" ValidationGroup="cihazGrupg" ErrorMessage="Lütfen kdv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="kdvg" runat="server" Type="Currency" ValidationGroup="cihazGrupg" MinimumValue="0" MaximumValue="100" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="otvg" CssClass="col-md-2 control-label">Ötv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="otvg" ValidationGroup="cihazGrupg" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="otvg" Type="Currency" ValidationGroup="cihazGrupg" ErrorMessage="Lütfen ötv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="otvg" runat="server" Type="Currency" ValidationGroup="cihazGrupg" MinimumValue="0" MaximumValue="100" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="oivg" CssClass="col-md-2 control-label">Öiv Oranı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="oivg" ValidationGroup="cihazGrupg" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="oivg" ValidationGroup="cihazGrupg" ErrorMessage="Lütfen öiv oranı giriniz"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="oivg" runat="server" Type="Currency" ValidationGroup="cihazGrupg" MinimumValue="0" MaximumValue="100" />
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnGrupKaydet" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" OnClick="btnGrupKaydet_Click" ValidationGroup="cihazGrupg" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGrup" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <%-- yeni grup bitiyor --%>

        <div id="updateModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="updateModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h4 id="updateModalLabel" class="baslik">Cihaz/Malzeme Güncelle</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">
                                <asp:HiddenField ID="hdnCihazID" runat="server" />
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="drdGrupDuzen" CssClass="col-md-2 control-label">Ürün Grubu</asp:Label>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="drdGrupDuzen" CssClass="form-control" runat="server">
                                            <%--<asp:ListItem Text="Pos/Banka seçiniz" Value="-1"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="cihaz_adi_up" CssClass="col-md-2 control-label">Cihaz Tanımı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="cihaz_adi_up" ValidationGroup="cihazGrup_up" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cihaz_adi_up" ValidationGroup="cihazGrup_up" ErrorMessage="Lütfen ürün cinsi giriniz"></asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hdnCihazStok" runat="server" />
                                        <asp:HiddenField ID="hdnCihazMaliyet" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="aciklama_up" CssClass="col-md-2 control-label">Cihaz Açıklama</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="aciklama_up" ValidationGroup="cihazGrup_up" CssClass="form-control" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="barcode_up" CssClass="col-md-2 control-label">Barkod</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="barcode_up" ValidationGroup="cihazGrup_up" CssClass="form-control" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="garanti_suresi_up" CssClass="col-md-2 control-label">Garanti Süresi(Ay)</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="garanti_suresi_up" ValidationGroup="cihazGrup_up" TextMode="Number" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="garanti_suresi_up" ValidationGroup="cihazGrup_up" ErrorMessage="Lütfen süre giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="stok_up" CssClass="col-md-2 control-label">Stok Miktarı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="stok_up" TextMode="Number" ValidationGroup="cihazGrup_up" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="stok_up" ValidationGroup="cihazGrup_up" ErrorMessage="Henüz stok yoksa sıfır giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="maliyet_up" CssClass="col-md-2 control-label">Alış Fiyatı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="maliyet_up" ValidationGroup="cihazGrup_up" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="maliyet_up" ValidationGroup="cihazGrup_up" ErrorMessage="Birim maliyeti en az sıfır giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="satis_up" CssClass="col-md-2 control-label">Satış Fiyatı</asp:Label>
                                    <div class="col-md-10">
                                        <asp:TextBox runat="server" ID="satis_up" ValidationGroup="cihazGrup_up" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="satis_up" ValidationGroup="cihazGrup_up" ErrorMessage="Henüz fiyat yoksa sıfır giriniz"></asp:RequiredFieldValidator>

                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCihazUpdate" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" OnClick="btnCihazUpdate_Click" ValidationGroup="cihazGrup_up" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdAlimlar" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>


</asp:Content>
