<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Servis.aspx.cs" Inherits="TeknikServis.TeknikTeknik.Servis" %>

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
        <div class="panel-group" id="accordionik">
            <div id="panelContents" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" class="collapsed">Servis Bilgileri</a>

                    </h2>

                </div>
                <div id="collapseOne" class="panel-collapse in" style="height: auto;">
                    <div class="panel-body">

                        <div class="btn-group visible-lg pull-right">
                            <asp:Button ID="btnEkle" runat="server" CssClass="btn btn-info" Text="Detay" OnClick="btnEkle_Click" />
                            <%--<asp:Button ID="btnHesaplar" runat="server" CssClass="btn btn-info" Text="Kararlar" OnClick="btnHesaplar_Click" />--%>
                            <asp:Button ID="btnSonlandir" runat="server" CssClass="btn btn-info" Text="Sonladır" OnClick="btnSonlandir_Click" />
                            <asp:Button ID="btnYol" runat="server" CssClass="btn btn-info" Text="Pusula" OnClick="btnYol_Click" />
                           <asp:Button ID="btnHarita" runat="server" CssClass="btn btn-info" Text="Harita" OnClick="btnHarita_Click" />
                             <asp:Button ID="btnBelge" runat="server" CssClass="btn btn-info" Text="Belge" OnClick="btnBelge_Click" />
                            <asp:LinkButton ID="btnMusteriDetayim"
                                runat="server" Visible="false"
                                CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                                Text="<i class='fa fa-user icon-2x'></i>" />
                        </div>

                        <div class="btn-group visible-sm visible-xs pull-right">
                            <asp:LinkButton ID="btnEkleK"
                                runat="server"
                                CssClass="btn btn-primary " OnClick="btnEkle_Click"
                                Text="<i class='fa fa-cog icon-2x'></i>" />

                            <asp:LinkButton ID="btnSonlandirK"
                                runat="server"
                                CssClass="btn btn-success " OnClick="btnSonlandir_Click"
                                Text="<i class='fa fa-hourglass-end icon-2x'></i>" />
                            <asp:LinkButton ID="btnYolK"
                                runat="server"
                                CssClass="btn btn-warning " OnClick="btnYol_Click"
                                Text="<i class='fa fa-car icon-2x'></i>" />
                            <asp:LinkButton ID="btnMusteriDetayimK"
                                runat="server" Visible="false"
                                CssClass="btn btn-info " OnClick="btnMusteriDetayim_Click"
                                Text="<i class='fa fa-user icon-2x'></i>" />

                        </div>

                        <div class="col-md-12">
                            <h3>
                                <label id="txtMusteri" runat="server" class="label label-danger"></label>
                            </h3>
                            <h3 id="txtKonu" runat="server"></h3>

                            <p id="txtServisAciklama" runat="server" class="lead"></p>

                            <p id="txtServisAdresi" runat="server" class="lead"></p>

                        </div>
                        <div class="col-md-6">
                            <label id="lblKimlik" for="txtKimlikNo">Servis No:</label>
                            <input class="form-control alert-info" id="txtKimlikNo" runat="server" type="text" disabled="disabled" />
                            <label id="lblTarih" for="txtTarih">Servis Tarihi:</label>
                            <input class="form-control alert-warning" id="txtTarih" runat="server" type="datetime" />
                            <label id="lblDurum" for="txtDurum">Durum:</label>
                            <input class="form-control alert-success" id="txtDurum" runat="server" />
                        </div>
                        <div class="col-md-6">
                            <label id="lblTutar" for="txtServisTutar">Servis Tutarı:</label>
                            <input class="form-control alert-info" id="txtServisTutar" runat="server" type="text" disabled="disabled" />
                            <label id="lblUsta" for="txtServisUsta">Görevli Usta:</label>
                            <input class="form-control alert-warning" id="txtServisUsta" runat="server" />
                            <label id="lblCihaz" for="txtServisCihaz">Müşteri Cihazı:</label>
                            <input class="form-control alert-success" id="txtServisCihaz" runat="server" />
                            <label id="lblKullanici" for="txtKullanici">Kullanıcı:</label>
                            <input class="form-control alert-success" id="txtKullanici" runat="server" />
                        </div>
                        <%-- Servis tutarı
                            Görevli usta
                             Cihaz adı--%>
                        <input type="hidden" name="txtServisID" value="" runat="server" id="txtServisID" />
                        <input type="hidden" name="hdnDurumID" value="" runat="server" id="hdnDurumID" />
                        <input type="hidden" name="txtAtanan" value="" runat="server" id="hdnAtananID" />
                        <input type="hidden" name="txtCust" value="" runat="server" id="hdnCustID" />

                    </div>
                </div>

            </div>

            <div id="Div2" runat="server" class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">
                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" class="collapsed">Servis Hesapları</a>

                    </h2>

                </div>
                <div id="collapseTwo" class="panel-collapse collapse" style="height: 0px;">
                    <div class="panel-body">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                            <ProgressTemplate>
                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>


                        <div class="table-responsive">

                            <asp:UpdatePanel ID="upCrudGrid" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <div id="cariOzet" runat="server" class="pull-right">
                                        <span id="txtHesapAdet" runat="server" class="label label-success"></span>
                                        <span id="txtHesapMaliyet" runat="server" class="label label-warning"></span>
                                        <span id="txtHesapTutar" runat="server" class="label label-info"></span>
                                    </div>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="hesapID"
                                        EmptyDataText="Kayıt girilmemiş" OnRowCommand="GridView1_RowCommand" OnRowCreated="GridView1_RowCreated" OnRowDataBound="GridView1_RowDataBound"
                                        OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="10">
                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>

                                            <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" HeaderStyle-Width="100">
                                                <ItemTemplate>


                                                    <asp:LinkButton ID="delLink"
                                                        runat="server"
                                                        CssClass="btn btn-danger btn-xs"
                                                        CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                    <asp:LinkButton ID="btnDuzenle"
                                                        runat="server"
                                                        CssClass="btn btn-primary btn-xs"
                                                        CommandName="duzenle" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />

                                                    <asp:LinkButton ID="btnOnay"
                                                        runat="server"
                                                        CssClass="btn btn-success btn-xs"
                                                        CommandName="onay" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex +";"+ Eval("MusteriID")  %>' Text="<i class='fa fa-check'></i>" />
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">
                                                <ItemTemplate>


                                                    <asp:LinkButton ID="delLinkk"
                                                        runat="server"
                                                        CssClass="btn btn-danger btn-md"
                                                        CommandName="del" CommandArgument='<%#Eval("hesapID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                    <asp:LinkButton ID="btnDuzenlek"
                                                        runat="server"
                                                        CssClass="btn btn-primary btn-md"
                                                        CommandName="duzenle" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-pencil'></i>" />

                                                    <asp:LinkButton ID="btnOnayk"
                                                        runat="server"
                                                        CssClass="btn btn-success btn-md"
                                                        CommandName="onay" CommandArgument='<%#Eval("hesapID")+ ";" + Container.DisplayIndex +";"+ Eval("MusteriID")  %>' Text="<i class='fa fa-check'></i>" />
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cihaz" HeaderText="Ürün/Parça" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>

                                            <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="toplam_maliyet" HeaderText="Toplam Maliyet" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                            <asp:BoundField DataField="onayDurumu" HeaderText="Onay" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                                            <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderTemplate>
                                                    Usta/Dış Servis
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#Eval("disServis")  %>
                                                </ItemTemplate>

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="onayTarih" HeaderText="Onay Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="servisID" HeaderText="SID" HeaderStyle-CssClass="hidden-xs  hidden-lg" ItemStyle-CssClass="hidden-xs hidden-lg"></asp:BoundField>
                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>

                                        </Columns>

                                    </asp:GridView>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                                    <%-- <asp:AsyncPostBackTrigger ControlID="btnPaket" EventName="Click" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                            <div id="onayModalH" class="modal  fade" tabindex="-1" role="dialog"
                                aria-labelledby="addModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-sm">

                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                                        <ContentTemplate>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">
                                                        <div class="alert alert-info text-center">
                                                            <i class="fa fa-2x">Servis hesabını onaylıyor musunuz?</i>
                                                            <div class="checkbox-inline">

                                                                <asp:CheckBox ID="chcSmsH" Text="SMS" runat="server" />
                                                            </div>
                                                            <div class="checkbox-inline">

                                                                <asp:CheckBox ID="chcMailH" Text="Mail" runat="server" />
                                                            </div>

                                                            <div class="btn-group pull-right">

                                                                <asp:Button ID="btnOnayH" runat="server" Text="Tamam"
                                                                    CssClass="btn btn-success" OnClick="btnOnay_ClickH" />
                                                                <button class="btn btn-warning" data-dismiss="modal"
                                                                    aria-hidden="true">
                                                                    Kapat</button>

                                                            </div>
                                                        </div>
                                                    </div>

                                                    <asp:HiddenField ID="hdnHesapIDH" runat="server" />
                                                    <asp:HiddenField ID="hdnMusteriIDH" runat="server" />
                                                    <asp:HiddenField ID="hdnServisIDDH" runat="server" />
                                                    <asp:HiddenField ID="hdnIslemmH" runat="server" />
                                                    <asp:HiddenField ID="hdnYekunnH" runat="server" />
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnOnayH" EventName="Click" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div id="topluModal" class="modal  fade" tabindex="-1" role="dialog"
                                aria-labelledby="addModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-sm">

                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">

                                        <ContentTemplate>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">
                                                        <div class="alert alert-info text-center">
                                                            <i class="fa fa-2x">Servis hesaplarının hepsini onaylıyor musunuz?</i>
                                                            <div class="checkbox-inline">

                                                                <asp:CheckBox ID="chcSmsToplu" Text="SMS" runat="server" />
                                                            </div>
                                                            <div class="checkbox-inline">

                                                                <asp:CheckBox ID="chcMailToplu" Text="Mail" runat="server" />
                                                            </div>
                                                            <div class="btn-group pull-right">

                                                                <asp:Button ID="btnTopluOnayKaydet" runat="server" Text="Tamam"
                                                                    CssClass="btn btn-success" OnClick="btnTopluOnayKaydet_Click" />
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
                                            <asp:AsyncPostBackTrigger ControlID="btnTopluOnay" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnTopluOnayK" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div id="paketModal" class="modal  fade" tabindex="-1" role="dialog"
                                aria-labelledby="paketModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-sm">

                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">

                                        <ContentTemplate>
                                            <div class="modal-body">
                                                <div class="row">

                                                    <div class="col-md-12">
                                                        <div class="alert alert-info text-center">
                                                            <i class="fa fa-2x">Eklenecek paketi seçiniz</i>
                                                            <asp:DropDownList ID="drdPaketler" runat="server" CssClass="form-control">
                                                                <asp:ListItem Text="Servis paketi seçiniz" Value="-1" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="btn-group pull-right">

                                                                <asp:Button ID="btnPaket" runat="server" Text="Uygula"
                                                                    CssClass="btn btn-success" OnClick="btnPaket_Click" />
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
                                            <asp:AsyncPostBackTrigger ControlID="btnPaketEkle" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnPaketEkleK" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div id="tamirciModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="tamirciModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-lg">
                                    <div class="modal-header modal-header-info">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                        <h3 id="tamirciModalLabel" class="baslik">Tamirciye Gönderme</h3>
                                    </div>

                                    <div class="modal-body">

                                        <%--  <div class="panel panel-info">
                                <div class="panel-heading">
                                    SERVİS HESAP VE KARARI
                                </div>--%>
                                        <%-- %> <div class="panel-body"> --%>

                                        <div class="panel panel-info">
                                            <div class="panel-heading">
                                                <h4 class="panel-title">
                                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseMusteri" class="collapsed">Dış Servis Seçimi</a>
                                                </h4>
                                            </div>
                                            <%-- müşteri aramayı ekleyecez --%>
                                            <div id="collapseMusteri" class="panel-collapse in" style="height: auto;">
                                                <div class="panel-body">
                                                    <!-- Müşteri seçim alanı başlıyor -->

                                                    <div class="table-responsive">
                                                        <div class="input-group custom-search-form">
                                                            <input runat="server" type="text" id="txtMusteriSorgu" class="form-control" placeholder="Ara..." />
                                                            <span class="input-group-btn">
                                                                <button id="btnMusteriAra" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                                                                    <i class="fa fa-search"></i>
                                                                </button>
                                                            </span>
                                                        </div>
                                                    </div>

                                                    <!-- Müşteri seç,malanı bitiyor-->
                                                </div>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanelKarar" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="grdMusteri" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                                                    EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="false"
                                                    OnSelectedIndexChanged="grdMusteri_SelectedIndexChanged" OnRowCommand="grdMusteri_RowCommand">
                                                    <SelectedRowStyle CssClass="danger" />

                                                    <Columns>
                                                        <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                                            <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                            <HeaderStyle CssClass="visible-lg" />
                                                            <ItemStyle CssClass="visible-lg" />
                                                        </asp:BoundField>

                                                        <asp:TemplateField HeaderText="Tamirci Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="btnRandom"
                                                                    runat="server"
                                                                    CssClass="btn btn-primary"
                                                                    CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("Ad") %> '>  </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Adres" HeaderText="Adres" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                            <HeaderStyle CssClass="visible-lg" />
                                                            <ItemStyle CssClass="visible-lg" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Telefon" HeaderText="Telefon" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                            <HeaderStyle CssClass="visible-lg" />
                                                            <ItemStyle CssClass="visible-lg" />
                                                        </asp:BoundField>

                                                    </Columns>

                                                </asp:GridView>

                                                <%-- tamircinin yapacağı işlem bilgileri --%>
                                                <div class="panel panel-info">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseKarar">Karar Bilgileri</a>
                                                        </h4>
                                                    </div>
                                                    <div id="collapseKarar" class="panel-collapse in" style="height: auto;">
                                                        <div class="panel-body">
                                                            <div class="form-horizontal">

                                                                <div class="form-group">
                                                                    <label class="col-sm-2 control-label" for="txtIslemParcaTamirci">Yapılacak İşlem</label>
                                                                    <div class="col-sm-10">
                                                                        <input id="txtIslemParcaTamirci" runat="server" type="text" class="form-control" />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" EnableClientScript="true" CssClass="text-danger" ControlToValidate="txtIslemParcaTamirci" ErrorMessage="Lütfen yapılacak işlemi giriniz" ValidationGroup="valGrupTamir"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <asp:HiddenField ID="hdnTamirciID" runat="server" Value="" />
                                                                <div class="form-group">

                                                                    <label class="col-sm-2 control-label" for="txtKDVOraniDuzenleTamirci">KDV Oranı</label>
                                                                    <div class="col-sm-10">
                                                                        <asp:TextBox ID="txtKDVOraniDuzenleTamirci" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">

                                                                    <label class="col-sm-2 control-label" for="txtKDVDuzenleTamirci">KDV Tutarı</label>
                                                                    <div class="col-sm-10">
                                                                        <asp:TextBox ID="txtKDVDuzenleTamirci" Enabled="false" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%-- Burası müşteriye verdiğimiz fiyat --%>
                                                                    <label class="col-sm-2 control-label" for="txtYekunTamirci">Tutar(KDV Dahil)</label>
                                                                    <div class="col-sm-10">
                                                                        <asp:TextBox ID="txtYekunTamirci" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" EnableClientScript="true" ControlToValidate="txtYekunTamirci" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrupTamir"></asp:RequiredFieldValidator>
                                                                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtYekunTamirci" Type="Currency" ValidationGroup="valGrupTamir" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                        <asp:HiddenField ID="hdnHesapIDDuzenTamirci" runat="server" />
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%-- Burası müşteriye verdiğimiz fiyat --%>
                                                                    <label class="col-sm-2 control-label" for="txtTamirciMaliyet">Maliyet(Tamirciye Ödeyeceğimiz)</label>
                                                                    <div class="col-sm-10">
                                                                        <asp:TextBox ID="txtTamirciMaliyet" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" EnableClientScript="true" ControlToValidate="txtTamirciMaliyet" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrupTamir"></asp:RequiredFieldValidator>
                                                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtTamirciMaliyet" Type="Currency" ValidationGroup="valGrupTamir" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>

                                                                    </div>
                                                                </div>
                                                                <div class="form-group">

                                                                    <label class="col-sm-2 control-label" for="txtAciklamaTamirci">Açıklama</label>
                                                                    <div class="col-sm-10">
                                                                        <asp:TextBox ID="txtAciklamaTamirci" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrupTamir"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">

                                                                    <label for="tarihtamirci" class="col-md-2 control-label">İşlem Tarihi</label>
                                                                    <div class="col-md-10">

                                                                        <input type='text' id="tarihtamirci" runat="server" class="form-control" />

                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="btn-group-justified">
                                                                        <div class="col-md-10  pull-right">
                                                                            <asp:Button ID="btnKaydetTamirci" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrupTamir" CssClass="btn btn-info btn-block" OnClick="btnKaydetTamirci_Click" />
                                                                          <button class="btn btn-primary btn-block" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                                                        </div>
                                                                        <%--<div class="col-md-4 ">
                                                                          
                                                                        </div>--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grdMusteri" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnMusteriAra" EventName="ServerClick" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <%--</div>--%>
                                        <%--</div>--%>

                                        <%--  <div class="modal-footer">
                                
                            </div>--%>
                                    </div>


                                </div>

                            </div>

                            <div id="ustaModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="ustaModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-lg">
                                    <div class="modal-header modal-header-info">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                        <h3 id="ustaModalLabel" class="baslik">Usta Seçimi</h3>
                                    </div>

                                    <div class="modal-body">

                                        <%-- <div class="panel panel-info"> --%>
                                        <%-- <div class="panel-heading">
                                    SERVİS İÇİN USTA GÖREVLENDİRİYORSUNUZ
                                </div>--%>

                                        <div class="panel-body">
                                            <div class="panel panel-info">
                                                <div class="panel-heading">
                                                    <h4 class="panel-title">
                                                        <a data-toggle="collapse" data-parent="#accordion" href="#collapseuSTA" class="collapsed">Usta Seçimi</a>
                                                    </h4>
                                                </div>
                                                <%-- müşteri aramayı ekleyecez --%>

                                                <div class="panel-body">
                                                    <!-- Müşteri seçim alanı başlıyor -->

                                                    <div class="table-responsive">
                                                        <div class="input-group custom-search-form">
                                                            <input runat="server" type="text" id="txtUsta" class="form-control" placeholder="Ara..." />
                                                            <span class="input-group-btn">
                                                                <button id="btnUstaAra" runat="server" class="btn btn-default" type="submit" onserverclick="UstaAra">
                                                                    <i class="fa fa-search"></i>
                                                                </button>
                                                            </span>
                                                        </div>

                                                    </div>

                                                    <!-- Müşteri seç,malanı bitiyor-->
                                                </div>

                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="grdUsta" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                                                        EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="false"
                                                        OnSelectedIndexChanged="grdUsta_SelectedIndexChanged">
                                                        <SelectedRowStyle CssClass="danger" />

                                                        <Columns>
                                                            <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                                                <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                                            </asp:ButtonField>
                                                            <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>

                                                            <asp:TemplateField HeaderText="Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnRandom"
                                                                        runat="server"
                                                                        CssClass="btn btn-primary"
                                                                        CommandName="detail" CommandArgument='<%#Eval("CustID") %>' Text=' <%#Eval("Ad") %> '>  </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Adres" HeaderText="Adres" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Telefon" HeaderText="Telefon" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                <HeaderStyle CssClass="visible-lg" />
                                                                <ItemStyle CssClass="visible-lg" />
                                                            </asp:BoundField>

                                                        </Columns>

                                                    </asp:GridView>
                                                    <div class="btn-group pull-right visible-lg">
                                                        <asp:Button ID="btnUsta" runat="server" Text="Görev ver" CssClass="btn btn-info  " OnClick="btnUsta_Click" />
                                                        <asp:Button ID="btnFire" runat="server" Text="Görevden al" CssClass="btn btn-primary " OnClick="btnFire_Click" />
                                                    </div>
                                                    <div class="btn-group pull-right visible-xs visible-sm">
                                                        <asp:Button ID="btnUstaK" runat="server" Text="Ata" CssClass="btn btn-info  " OnClick="btnUsta_Click" />
                                                        <asp:Button ID="btnFireK" runat="server" Text="Çıkar" CssClass="btn btn-primary " OnClick="btnFire_Click" />
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdUsta" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnUstaAra" EventName="ServerClick" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnFire" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnFireK" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnUstaAta" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnUstaAtaK" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                        <%--</div>--%>

                                        <%--  <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                            </div>--%>
                                    </div>


                                </div>

                            </div>

                            <div id="yeniModal" class="modal  fade" tabindex="-1" role="dialog" aria-labelledby="yeniModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-lg">
                                    <div class="modal-header modal-header-info">
                                        <a class="close" data-dismiss="modal" aria-hidden="true">×</a>
                                        <h3 id="yeniModalLabel" class="baslik">Yeni Hesap Bilgileri</h3>
                                    </div>

                                    <div class="modal-body">

                                        <div class="panel panel-info">
                                            <div class="panel-heading">
                                                SERVİS HESAP VE KARARI
                                            </div>
                                            <div class="panel-body">
                                                <div class="panel panel-info">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            <a data-toggle="collapse" data-parent="#accordion" href="#collapseYeni" class="collapsed">Ürün/Parça Seçimi</a>
                                                        </h4>
                                                    </div>
                                                    <div id="collapseYeni" class="panel-collapse collapse" style="height: 0px;">
                                                        <div class="panel-body">
                                                            <!-- Müşteri seçim alanı başlıyor -->

                                                            <div class="table-responsive">
                                                                <div class="input-group custom-search-form ">
                                                                    <span class="input-group-btn">
                                                                        <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="CihazAra">
                                                                            <i class="fa fa-search"></i>
                                                                        </button>
                                                                    </span>
                                                                    <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Ara..." />

                                                                </div>

                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                    <ContentTemplate>

                                                                        <asp:GridView ID="grdCihaz" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                                                            EmptyDataText="Ürün/Parça girilmemiş" OnSelectedIndexChanged="grdCihaz_SelectedIndexChanged">
                                                                            <SelectedRowStyle CssClass="danger" />
                                                                            <Columns>

                                                                                <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç" HeaderText="Seçim">
                                                                                    <ControlStyle CssClass="btn btn-danger"></ControlStyle>
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
                                                                                <asp:BoundField DataField="bakiye" HeaderText="Stok" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                                    <HeaderStyle CssClass="visible-lg" />
                                                                                    <ItemStyle CssClass="visible-lg" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="fiyat" HeaderText="Son Fiyat" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                                                    <HeaderStyle CssClass="visible-lg" />
                                                                                    <ItemStyle CssClass="visible-lg" />
                                                                                </asp:BoundField>
                                                                            </Columns>

                                                                        </asp:GridView>
                                                                        <asp:Button ID="btnAddCihaz" runat="server" Text="Yeni Ürün/Parça" CssClass="btn btn-info"
                                                                            OnClick="btnAddCihaz_Click" />
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                                                                        <asp:AsyncPostBackTrigger ControlID="grdCihaz" EventName="RowCommand" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnEkleH" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnEkleHK" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnTamirciyeK" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnTamirciye" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>


                                                            </div>

                                                            <!-- Müşteri seç,malanı bitiyor-->
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                    <ContentTemplate>

                                                        <div class="panel panel-info">
                                                            <div class="panel-heading">
                                                                <h4 class="panel-title">
                                                                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseKararil">Karar Bilgileri</a>
                                                                </h4>
                                                            </div>
                                                            <div id="collapseKararil" class="panel-collapse in" style="height: auto;">
                                                                <div class="panel-body">
                                                                    <div class="form-horizontal">

                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label" for="txtAdet">Adet</label>
                                                                            <div class="col-sm-10">

                                                                                <asp:TextBox ID="txtAdet" runat="server" Text="1" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label" for="txtCihazAdiGoster">Ürün/Parça</label>
                                                                            <div class="col-sm-10">
                                                                                <input id="txtCihazAdiGoster" runat="server" type="text" class="form-control" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-2 control-label" for="txtIslemParca">Yapılacak İşlem</label>
                                                                            <div class="col-sm-10">
                                                                                <input id="txtIslemParca" runat="server" type="text" class="form-control" />
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" CssClass="text-danger" ControlToValidate="txtIslemParca" ErrorMessage="Lütfen yapılacak işlemi giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">

                                                                            <label class="col-sm-2 control-label" for="txtKDVOraniDuzenle">KDV Oranı</label>
                                                                            <div class="col-sm-10">
                                                                                <asp:TextBox ID="txtKDVOraniDuzenle" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">

                                                                            <label class="col-sm-2 control-label" for="txtKDVDuzenle">KDV Tutarı</label>
                                                                            <div class="col-sm-10">
                                                                                <asp:TextBox ID="txtKDVDuzenle" Enabled="false" runat="server" Text="18" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">

                                                                            <label class="col-sm-2 control-label" for="txtYekun">Tutar(KDV Dahil)</label>
                                                                            <div class="col-sm-10">
                                                                                <asp:TextBox ID="txtYekun" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" EnableClientScript="true" ControlToValidate="txtYekun" ErrorMessage="Lütfen toplam tutar giriniz" CssClass="text-danger" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtYekun" Type="Currency" ValidationGroup="valGrup" CssClass="text-danger" MaximumValue="1000000" MinimumValue="0" ErrorMessage="Ondalık sayılar için virgül kullanınız."></asp:RangeValidator>
                                                                                <asp:HiddenField ID="hdnHesapIDDuzen" runat="server" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">

                                                                            <label class="col-sm-2 control-label" for="txtAciklama">Açıklama</label>
                                                                            <div class="col-sm-10">
                                                                                <asp:TextBox ID="txtAciklama" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label runat="server" AssociatedControlID="chcSms" CssClass="col-md-2 control-label">Seç</asp:Label>
                                                                            <div class="col-md-10">
                                                                                <asp:CheckBox ID="CheckBox3" CssClass="col-md-4 checkbox" Text="SMS" runat="server" />
                                                                                <asp:CheckBox ID="CheckBox4" CssClass="col-md-4 checkbox" Text="Mail" runat="server" />

                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">

                                                                            <label for="tarih2" class="col-md-2 control-label">İşlem Tarihi</label>
                                                                            <div class="col-md-10">

                                                                                <input type='text' id="tarih2" runat="server" class="form-control" />

                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CausesValidation="true" ValidationGroup="valGrup" CssClass="btn btn-info btn-block" OnClick="btnKaydet_Click" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grdCihaz" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                        <div class="modal-footer">
                                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                                        </div>
                                    </div>


                                </div>

                            </div>
                            <!-- Add Record Modal Starts here-->
                            <div id="addModalCihaz" class="modal  fade" tabindex="-1" role="dialog"
                                aria-labelledby="addModalCihazLabel" aria-hidden="true">
                                <div class="modal-dialog modal-content modal-sm">
                                    <div class="modal-header modal-header-info">
                                        <button type="button" class="close" data-dismiss="modal"
                                            aria-hidden="true">
                                            ×</button>
                                        <h3 id="addModalCihazLabel" class="baslik">Yeni Ürün/Parça</h3>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <div class="modal-body">

                                                <div class="form-horizontal">

                                                    <div class="form-group">
                                                        <asp:Label runat="server" AssociatedControlID="txtCihazAdi" CssClass="col-md-10 control-label">Ürün/Parça Adı</asp:Label>
                                                        <div class="col-md-10">
                                                            <asp:TextBox runat="server" ID="txtCihazAdi" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtCihazAdi" ErrorMessage="Lütfen Ürün/Parça adını giriniz"></asp:RequiredFieldValidator>
                                                            <asp:HiddenField ID="hdnGarantiSure" runat="server" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label runat="server" AssociatedControlID="txtCihazAciklama" CssClass="col-md-10 control-label">Açıklama</asp:Label>
                                                        <div class="col-md-10">
                                                            <asp:TextBox runat="server" ID="txtCihazAciklama" CssClass="form-control" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtCihazAciklama" ErrorMessage="Lütfen  açıklama giriniz"></asp:RequiredFieldValidator>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label runat="server" AssociatedControlID="txtGarantiSuresi" CssClass="col-md-10 control-label">Garanti(AY)</asp:Label>
                                                        <div class="col-md-10">
                                                            <asp:TextBox runat="server" ID="txtGarantiSuresi" CssClass="form-control" TextMode="Number" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="musteriGrup2" ControlToValidate="txtGarantiSuresi" ErrorMessage="Lütfen  süre giriniz"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>


                                            <div class="modal-footer">
                                                <asp:Button ID="btnAddCihazRecord" runat="server" Text="Kaydet"
                                                    CssClass="btn btn-info" OnClick="btnAddCihazRecord_Click" ValidationGroup="musteriGrup2" />
                                                <button class="btn btn-info" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    Kapat</button>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>

                                            <asp:AsyncPostBackTrigger ControlID="btnAddCihaz" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>

                        <div class=" btn-group pull-right visible-lg">

                            <asp:LinkButton ID="btnEkleH"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnEkle_ClickH"
                                Text="<i class='fa fa-plus icon-2x'></i>" />
                            <asp:Button ID="btnUstaAta" runat="server" Text="Usta" CssClass="btn btn-info"
                                OnClick="btnUstaAta_Click" />
                            <asp:Button ID="btnTamirciye" runat="server" Text="Tamire" CssClass="btn btn-info"
                                OnClick="btnTamirciye_Click" />

                            <asp:Button ID="btnPaketEkle" runat="server" Text="Paket" CssClass="btn btn-info"
                                OnClick="btnPaketEkle_Click" />

                            <asp:LinkButton ID="btnTopluOnay"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnTopluOnay_Click"
                                Text="<i class='fa fa-check icon-2x'></i>" />
                            <asp:LinkButton ID="btnPrint"
                                runat="server"
                                CssClass="btn btn-info visible-lg" OnClick="btnPrnt_Click"
                                Text="<i class='fa fa-print icon-2x'></i>" />

                            <asp:LinkButton ID="btnExportExcel"
                                runat="server"
                                CssClass="btn btn-info visible-lg " OnClick="btnExportExcel_Click"
                                Text="<i class='fa fa-file-excel-o icon-2x'></i>" />

                            <asp:LinkButton ID="btnExportWord"
                                runat="server"
                                CssClass="btn btn-info visible-lg" OnClick="btnExportWord_Click"
                                Text="<i class='fa fa-wikipedia-w icon-2x'></i>" />


                        </div>

                        <div class=" btn-group pull-right visible-xs visible-sm">


                            <asp:LinkButton ID="btnEkleHK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnEkle_ClickH"
                                Text="<i class='fa fa-plus icon-2x'></i>" />
                            <asp:LinkButton ID="btnUstaAtaK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnUstaAta_Click"
                                Text="<i class='fa fa-user-plus icon-2x'></i>" />

                            <asp:LinkButton ID="btnTamirciyeK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnTamirciye_Click"
                                Text="<i class='fa fa-send icon-2x'></i>" />
                            <asp:LinkButton ID="btnPaketEkleK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnPaketEkle_Click"
                                Text="<i class='fa fa-sort icon-2x'></i>" />

                            <asp:LinkButton ID="btnTopluOnayK"
                                runat="server"
                                CssClass="btn btn-info" OnClick="btnTopluOnay_Click"
                                Text="<i class='fa fa-thumbs-up icon-2x'></i>" />



                        </div>
                    </div>

                    <%--   <div class="panel-footer pull-right">
                    </div>--%>
                </div>

            </div>
        </div>
    </div>
    <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
        aria-labelledby="addModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-content modal-sm">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                <ContentTemplate>
                    <div class="modal-body">
                        <div class="row">

                            <div class="col-md-12">
                                <div class="alert alert-info text-center">
                                    <i class="fa fa-2x">Servis sonlandırmak istiyor musunuz?</i>
                                    <div class="checkbox-inline">

                                        <asp:CheckBox ID="chcSms" Text="SMS" runat="server" />
                                    </div>
                                    <div class="checkbox-inline">

                                        <asp:CheckBox ID="chcMail" Text="Mail" runat="server" />
                                    </div>

                                    <div class="btn-group pull-right">

                                        <asp:Button ID="btnOnay" runat="server" Text="Tamam"
                                            CssClass="btn btn-success" OnClick="btnOnay_Click" />
                                        <button class="btn btn-warning" data-dismiss="modal"
                                            aria-hidden="true">
                                            Kapat</button>

                                    </div>
                                </div>
                            </div>

                            <asp:HiddenField ID="hdnHesapID" runat="server" />
                            <asp:HiddenField ID="hdnMusteriID" runat="server" />
                            <asp:HiddenField ID="hdnServisIDD" runat="server" />

                        </div>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSonlandir" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSonlandirK" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="pager" runat="server">
        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5">

            <Fields>
                <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
                <asp:NumericPagerField ButtonType="Button" RenderNonBreakingSpacesBetweenControls="false" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-primary disabled" NextPreviousButtonCssClass="btn" />
                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
            </Fields>
        </asp:DataPager>

    </div>
    <asp:ListView ID="ListView1" runat="server" OnItemDataBound="ListView1_ItemDataBound" OnDataBound="ListView1_DataBound" OnPagePropertiesChanging="ListView1_PagePropertiesChanging">
        <ItemTemplate>

            <div id="Div1" runat="server" class="panel panel-primary">
                <div class="panel-heading">
                    <h4 class="panel-title"><%#DataBinder.Eval(Container.DataItem,"tarihZaman")%> - <%#DataBinder.Eval(Container.DataItem,"kullanici")%> 
                    </h4>
                </div>
                <div class="panel-body">

                    <h3><span class="label label-danger "><%#DataBinder.Eval(Container.DataItem,"durumAdi")%></span></h3>

                    <h3><%#DataBinder.Eval(Container.DataItem,"baslik")%></h3>


                    <p class="lead"><%#DataBinder.Eval(Container.DataItem,"aciklama")%> </p>


                    <div runat="server" id="resimCerceve">

                        <img id="resHTML" class="img-responsive img-rounded" runat="server" src='<%# Eval("belgeYol") %>' />

                        <asp:TextBox ID="txtYol" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"belgeYol")%>'></asp:TextBox>

                    </div>
                </div>
            </div>

        </ItemTemplate>

    </asp:ListView>


    <div id="Div3" runat="server">
        <asp:DataPager ID="DataPager2" runat="server" PagedControlID="ListView1" PageSize="5">

            <Fields>
                <asp:NextPreviousPagerField ShowLastPageButton="False" ShowNextPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
                <asp:NumericPagerField ButtonType="Button" RenderNonBreakingSpacesBetweenControls="false" NumericButtonCssClass="btn btn-primary" CurrentPageLabelCssClass="btn btn-primary disabled" NextPreviousButtonCssClass="btn" />
                <asp:NextPreviousPagerField ShowFirstPageButton="False" ShowPreviousPageButton="False" ButtonType="Button" ButtonCssClass="btn btn-danger" RenderNonBreakingSpacesBetweenControls="false" />
            </Fields>
        </asp:DataPager>

    </div>


    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#ContentPlaceHolder1_txtBaslamaTarihi').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_uruntarih').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_tarihtamirci').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>
</asp:Content>
