<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="MusteriDetayBilgileri.aspx.cs" Inherits="TeknikServis.MusteriDetayBilgileri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("Emanet dönüşünü onaylıyor musunuz?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);

        }


    </script>
    <script type="text/javascript">
        function Confirm2() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value2";

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
            <!-- Default panel contents -->
            <div class="panel-heading">
                <h2 class="baslik">
                    <label runat="server" id="baslik"></label>
                </h2>
            </div>
            <%-- <div class="panel-body">--%>
            <div class="visible-lg">
                <div class="btn-group  pull-right">

                    <asp:LinkButton ID="btnTahsilat"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-money'>Tahsilat</i>" OnClick="btnOdeme_Click" />
                    <asp:LinkButton ID="btnOde"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-money'>Ödeme</i>" OnClick="btnOde_Click" />

                    <asp:LinkButton ID="btnSatis"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-shopping-cart'>Hızlı Servis</i>" OnClick="btnSatis_Click" />

                    <asp:LinkButton ID="btnMesaj"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-phone-square'>Mesaj</i>" OnClick="btnMesaj_Click" />
                    <asp:LinkButton ID="btnMail"
                        runat="server"
                        CssClass="btn btn-info "
                        Text="<i class='fa fa-envelope'>Mail</i>" OnClick="btnMail_Click" />
                    <asp:LinkButton ID="btnYeniIstihbarat"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-pencil'>Not</i>" OnClick="btnYeniIstihbarat_Click" />
                    <asp:Button ID="btnFatura" runat="server" CssClass="btn btn-info" Text="Fatura" OnClick="btnFatura_Click" />
                    <asp:LinkButton ID="btnYol"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-taxi'>Yol</i>" OnClick="btnYol_Click" />

                    <asp:LinkButton ID="btnNokta"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-globe'>Harita</i>" OnClick="btnNokta_Click" />


                    <asp:LinkButton ID="btnExtre"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="Extre" OnClick="btnExtre_Click" />
                    <asp:LinkButton ID="btnKredi"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="Kredi" OnClick="btnKredi_Click" />
                </div>
            </div>
            <div class="pull-right visible-xs visible-sm">
                <div class="btn-group-justified ">

                    <asp:LinkButton ID="btnOdemeK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-money'>Tahsil</i>" OnClick="btnOdeme_Click" />
                    <asp:LinkButton ID="btnOdeK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-money'>Öde</i>" OnClick="btnOde_Click" />
                    <asp:LinkButton ID="btnSatisK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-wrench'></i>" OnClick="btnSatis_Click" />
                    <asp:LinkButton ID="btnYolK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-taxi'></i>" OnClick="btnYol_Click" />
                </div>
                <div class="btn-group-justified ">
                    <asp:LinkButton ID="btnMesajK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-phone-square'></i>" OnClick="btnMesaj_Click" />
                    <asp:LinkButton ID="btnMailK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-envelope'></i>" OnClick="btnMail_Click" />

                    <asp:LinkButton ID="btnNoktaK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="<i class='fa fa-globe'></i>" OnClick="btnNoktaK_Click" />
                    <asp:LinkButton ID="btnExtreK"
                        runat="server"
                        CssClass="btn btn-info"
                        Text="Extre" OnClick="btnExtre_Click" />
                </div>
            </div>
        </div>
        <!-- Welcome -->
        <%--  <div class="col-lg-12">--%>

        <div class="alert alert-info">
            <%-- KULLANİCİ BAŞLIYOR --%>


            <span class="label label-default" id="spnAdres" runat="server"></span>
            <span class="label label-default" id="spnTc" runat="server"></span>
            <span class="label label-default" id="spnTel" runat="server"></span>
            <%-- KULLANICI BİTİYOR --%>
        </div>

        <%--   </div>--%>
        <!--end  Welcome -->

        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
            <ProgressTemplate>

                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                </div>

            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="istihbarat" runat="server" visible="false" class="alert alert-info">

                    <h3 id="alarm" runat="server"></h3>

                </div>
                <div id="eksikbilgiler" runat="server" visible="false" class="alert alert-info">

                    <h3 id="eksikler" runat="server" class="txt-danger"></h3>

                </div>


                <div class="row">
                    <div class="col-lg-4">
                        <div class="panel panel-primary text-center no-boder">
                            <div class="panel-body green">
                                <i class="fa fa-bar-chart-o fa-3x"></i>
                                <h3 id="netBorc" runat="server"></h3>
                            </div>
                            <div class="panel-footer">
                                <span class="panel-eyecandy-title">Borçları
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="panel panel-primary text-center no-boder">
                            <div class="panel-body blue">
                                <i class="fa fa-pencil-square-o fa-3x"></i>
                                <h3 id="netAlacak" runat="server"></h3>
                            </div>
                            <div class="panel-footer">
                                <span class="panel-eyecandy-title">Alacakları
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="panel panel-primary text-center no-boder">
                            <div class="panel-body yellow">
                                <i class="fa fa fa-floppy-o fa-3x"></i>
                                <h3 id="netBakiye" runat="server"></h3>
                            </div>
                            <div class="panel-footer">
                                <span runat="server" id="bakiye_bilgi" class="panel-eyecandy-title">Bakiye
                                </span>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-lg-12">

                        <!--    Context Classes  -->
                        <div id="panelKredi" runat="server" visible="False" class="panel panel-info">
                            <div class="panel-heading">
                                <i class="fa fa-bar-chart-o fa-fw"></i>Kredi Yüklemeleri
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a id="A1" runat="server" class="btn btn-success" onclick="confirm()">Son kredi iptal et</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            </div>

                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">

                                            <asp:GridView ID="grdKrediler" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID"
                                                EmptyDataText="Henüz kredi yüklenmemiş." AllowPaging="true" PageSize="10" OnPageIndexChanging="grdKrediler_PageIndexChanging">
                                                <SelectedRowStyle CssClass="danger" />
                                                <PagerStyle CssClass="pagination-ys" />
                                                <Columns>

                                                    <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="tutar" HeaderText="Tutar" DataFormatString="{0:C}"></asp:BoundField>
                                                    <asp:BoundField DataField="paketadi" HeaderText="PAket"></asp:BoundField>
                                                    <asp:BoundField DataField="islem_tarihi" HeaderText="İşlem Tarihi" DataFormatString="{0:d}"></asp:BoundField>
                                                    <asp:BoundField DataField="gecerlilik_tarihi" HeaderText="Geçerlilik" DataFormatString="{0:d}"></asp:BoundField>
                                                    <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı"></asp:BoundField>


                                                </Columns>

                                            </asp:GridView>
                                        </div>

                                    </div>

                                </div>
                                <!-- /.row -->
                            </div>
                            <!-- /.panel-body -->
                        </div>
                        <!--  end  Context Classes  -->
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnIstihbarat" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnIstihbaratSil" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnKrediKaydet" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>



        <div class="row">
            <!--quick info section -->
            <div class="col-lg-3">
                <div class="alert alert-info text-center">
                    <i class="fa fa-calendar fa-3x" id="servisSayisi" runat="server"></i>&nbsp;Aktif Servis
 
                </div>

            </div>
            <div class="col-lg-3">
                <div class="alert alert-success text-center">
                    <i class="fa  fa-thumbs-up fa-3x" id="onayBekleyen" runat="server"></i>&nbsp;Onay Bekleniyor  
                </div>
            </div>
            <div class="col-lg-3">
                <div class="alert alert-info text-center">
                    <i class="fa fa-rss fa-3x" id="emanetSayisi" runat="server"></i>&nbsp; Emanet Sayısı

                </div>
            </div>
            <div class="col-lg-3">
                <div class="alert alert-warning text-center">
                    <i class="fa  fa-pencil fa-3x" id="taksitSayisi" runat="server"></i>&nbsp;Taksit/Fatura
                </div>
            </div>
            <!--end quick info section -->
        </div>

        <div class="row">
            <div class="col-lg-12">

                <!--Simple table example -->
                <div id="panelServis" runat="server" visible="false" class="panel panel-info">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>Açılan Son 10 Servis
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a id="linkServis" runat="server" href="/TeknikTeknik/ServislerCanli.aspx">Müşteri Servisleri</a>
                                        </li>
                                        <li><a id="linkServisEkle" runat="server" href="/TeknikTeknik/ServisEkle.aspx">Yeni Servis</a>
                                        </li>
                                        <li><a id="linkUstaServis" runat="server" href="/TeknikTeknik/ServisMaliyetleri.aspx">Usta'nın Servisleri</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                    </div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">

                                    <asp:GridView ID="grdServis" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="serviceID"
                                        EmptyDataText="Aktif servis yok" OnRowCreated="grdServis_RowCreated" OnRowDataBound="GridView1_RowDataBound">


                                        <RowStyle ForeColor="White" />

                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <ItemTemplate>
                                                    <div class="visible-lg">


                                                        <asp:LinkButton ID="btnHesap"
                                                            runat="server"
                                                            CssClass="btn btn-info btn-xs"
                                                            Text="<i class='fa fa-bell'></i>" />

                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="visible-lg" />

                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Detay" HeaderStyle-CssClass="visible-xs visible-sm" ItemStyle-CssClass="visible-xs visible-sm">

                                                <ItemTemplate>

                                                    <asp:LinkButton ID="btnKucuk"
                                                        runat="server"
                                                        CssClass="btn btn-primary"
                                                        Text="<i class='fa fa-refresh fa-spin'></i>" />

                                                </ItemTemplate>
                                                <ItemStyle CssClass="visible-xs visible-sm" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="serviceID" HeaderText="ID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                                <%--     <HeaderStyle CssClass="visible-lg" />
                                <ItemStyle CssClass="visible-lg" />--%>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="baslik" HeaderText="Konu">
                                                <%--    <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri Adı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <%-- <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                            </asp:BoundField>

                                            <asp:BoundField DataField="acilmaZamani" HeaderText="Tarih" HeaderStyle-CssClass="visible-lg " DataFormatString="{0:d}" ItemStyle-CssClass="visible-lg">
                                                <%--     <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="sonDurum" HeaderText="Durum">

                                                <%--  <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="yekun" HeaderText="Tutar" DataFormatString="{0:C}" />
                                            <asp:BoundField DataField="kullanici" HeaderText="Kulanıcı" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <%--  <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />--%>
                                            </asp:BoundField>

                                        </Columns>

                                    </asp:GridView>
                                </div>

                            </div>

                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.panel-body -->
                </div>
                <!--End simple table example -->

            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">

                <!--    Context Classes  -->
                <div id="panelKarar" runat="server" visible="false" class="panel panel-info">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>Onay Bekleyen 10 Hesap
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a runat="server" id="linkHesaplar" href="/TeknikTeknik/ServisHesaplar.aspx?onay=false">Hesaplara Git</a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                    </div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">

                                    <asp:GridView ID="grdKarar" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="hesapID"
                                        EmptyDataText="Onaylanmamış servis kararı yok">

                                        <Columns>

                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="tarihZaman" HeaderText="Hesap Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
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

                            </div>

                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.panel-body -->
                </div>
                <!--  end  Context Classes  -->
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">

                <!--    Context Classes  -->
                <div id="panelTamir" runat="server" visible="false" class="panel panel-info">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>Yaptığı Son Tamirler
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a runat="server" id="aTamir" href="/TeknikTeknik/ServisHesaplar.aspx?tamir=false">Hesaplara Git</a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                    </div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">

                                    <asp:GridView ID="grdTamir" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="hesapID"
                                        EmptyDataText="Onaylanmamış tamir kararı yok">

                                        <Columns>

                                            <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="yekun" HeaderText="Yekün" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="tarihZaman" HeaderText="Hesap Tarihi" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>

                                        </Columns>

                                    </asp:GridView>
                                </div>

                            </div>

                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.panel-body -->
                </div>
                <!--  end  Context Classes  -->
            </div>
        </div>
        <%-- alimlar başlıyor --%>

        <div class="row">
            <div class="col-lg-12">

                <!--    Context Classes  -->
                <div id="panelAlim" runat="server" visible="false" class="panel panel-info">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>Son 10 Satınalmalar
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a runat="server" id="linkSatinAlma" href="/TeknikAlim/Alimlar.aspx">Hesaplara Git</a>
                                        </li>
                                        <li><a runat="server" id="linkSatinDetay" href="/TeknikAlim/AlimDetaylar.aspx">Alim Detyalarına Git</a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                    </div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">

                                    <asp:GridView ID="grdAlimlar" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover" DataKeyNames="alim_id"
                                        EmptyDataText="Satın alma kaydı bulunmuyor"
                                        OnRowCreated="grdAlimlar_RowCreated">

                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>


                                            <asp:TemplateField HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <ItemTemplate>
                                                    <div class="visible-lg">
                                                        <asp:LinkButton ID="btnDetay"
                                                            runat="server"
                                                            CssClass="btn btn-success"
                                                            CommandName="detay" CommandArgument='<%# Eval("alim_id") %>' Text="<i class='fa fa-pencil'></i>" />


                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="visible-lg" />

                                            </asp:TemplateField>


                                            <asp:BoundField DataField="konu" HeaderText="Konu"></asp:BoundField>

                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama"></asp:BoundField>
                                            <asp:BoundField DataField="belge_no" HeaderText="Belge No" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg"></asp:BoundField>
                                            <asp:BoundField DataField="toplam_tutar" HeaderText="Tutar" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                            <asp:BoundField DataField="toplam_kdv" HeaderText="KDV" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                                            <asp:BoundField DataField="toplam_yekun" HeaderText="Yekün" DataFormatString="{0:C}" />
                                            <asp:BoundField DataField="alim_tarih" HeaderText="Tarih" DataFormatString="{0:d}"></asp:BoundField>
                                            <asp:BoundField DataField="kullanici" HeaderText="Kullanıcı" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg"></asp:BoundField>


                                        </Columns>

                                    </asp:GridView>
                                </div>

                            </div>

                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.panel-body -->
                </div>
                <!--  end  Context Classes  -->
            </div>
        </div>

        <%-- alımlar bitiyor --%>
        <div class="row">
            <div class="col-lg-12">

                <!--    Context Classes  -->
                <div id="panelYedek" runat="server" visible="false" class="panel panel-info">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>Emanet Ürünler
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a id="linkEmanetler" runat="server" href="/TeknikEmanet/Emanetler.aspx?onay=false">Emanetler</a>
                                        </li>
                                        <li><a id="linkEmanetYeni" runat="server" href="/TeknikEmanet/EmanetEkle.aspx">Yeni Emanet</a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                    </div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">

                                    <asp:GridView ID="grdYedek" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="yedekID"
                                        EmptyDataText="Emenet kaydı yok" OnRowCommand="grdYedek_RowCommand">
                                        <Columns>

                                            <asp:TemplateField HeaderText="İşlem" ShowHeader="False">
                                                <ItemTemplate>

                                                    <asp:LinkButton ID="delLink"
                                                        runat="server"
                                                        CssClass="btn btn-danger"
                                                        CommandName="del" CommandArgument='<%#Eval("yedekID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-check'>İade</i>" />

                                                </ItemTemplate>
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:TemplateField>


                                            <asp:BoundField DataField="yedekID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="musteriID" HeaderText="musteriID" HeaderStyle-CssClass="gizlisutun" ItemStyle-CssClass="gizlisutun">
                                                <HeaderStyle CssClass="gizlisutun" />
                                                <ItemStyle CssClass="gizlisutun" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="musteriAdi" HeaderText="Müşteri" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="urunAciklama" HeaderText="Emanet" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="verilmeTarihi" HeaderText="Verilme" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="donusTarih" HeaderText="İade" DataFormatString="{0:D}" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                <HeaderStyle CssClass="visible-lg" />
                                                <ItemStyle CssClass="visible-lg" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="donmeDurumu" HeaderText="Durum" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
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

                            </div>

                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.panel-body -->
                </div>
                <!--  end  Context Classes  -->
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">

                <!--    Context Classes  -->
                <div id="panelOdeme" runat="server" visible="false" class="panel panel-info">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>Yapılan Ödemeler
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a id="linkOdemeler" runat="server">Bütün Ödemeler</a>
                                        </li>
                                        <li><a id="linkOdemeYeni" runat="server">Yeni Ödeme</a>
                                        </li>

                                    </ul>
                                </div>
                            </div>
                    </div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">

                                    <asp:GridView ID="grdOdeme" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                        DataKeyNames="odemeID"
                                        EmptyDataText="Henüz ödeme yapılmamış"
                                        AllowPaging="true" PageSize="10">
                                        <PagerStyle CssClass="pagination-ys" />
                                        <Columns>

                                            <asp:BoundField DataField="odemeMiktari" HeaderText="Miktar" DataFormatString="{0:C}" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="tahsilat_odeme" HeaderText="Tahsilat/Ödeme" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm" DataFormatString="{0:d}">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="tahsilatOdeme_turu" HeaderText="Tür" />
                                            <asp:BoundField DataField="islem_adres" HeaderText="İşlem Yeri" />
                                            <asp:BoundField DataField="odemeTarih" HeaderText="Ödeme Tarihi" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm" DataFormatString="{0:d}">
                                                <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                                <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="aciklama" HeaderText="Açıklama">
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

                            </div>

                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.panel-body -->
                </div>
                <!--  end  Context Classes  -->
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">

                <!--    Context Classes  -->
                <div id="panelUrun" runat="server" visible="false" class="panel panel-info">
                    <div class="panel-heading">
                        <i class="fa fa-bar-chart-o fa-fw"></i>Müşteri Ürün/Parça
                            <div class="pull-right">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                                        İşlemler
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu pull-right" role="menu">
                                        <li><a id="linkCUrunAra" runat="server">Ürün/Parça Ara</a>
                                        </li>


                                    </ul>
                                </div>
                            </div>
                    </div>

                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>

                                            <asp:GridView ID="grdUrun" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="urunID"
                                                EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="true" OnRowCommand="grdUrun_RowCommand" OnRowCreated="grdUrun_RowCreated" OnRowDataBound="grdUrun_RowDataBound">

                                                <SelectedRowStyle CssClass="danger" />
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                                        <ItemTemplate>

                                                            <asp:LinkButton ID="delUrun"
                                                                runat="server"
                                                                CssClass="btn btn-danger btn-sm"
                                                                CommandName="del" CommandArgument='<%#Eval("urunID") %>' OnClientClick="Confirm()" Text="<i class='fa fa-trash-o'></i>" />
                                                            <asp:LinkButton ID="btnIade"
                                                                runat="server"
                                                                CssClass="btn btn-success btn-sm"
                                                                CommandName="iade" CommandArgument='<%#Eval("urunID")+ ";" + Container.DisplayIndex  %>' Text="<i class='fa fa-check'></i>" />

                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="urunID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Cinsi" HeaderText="Ürün" />

                                                    <asp:BoundField DataField="garantiBaslangic" HeaderText="Garanti Başlangıcı" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="garantiBitis" HeaderText="Garanti Bitişi" HeaderStyle-CssClass="visible-lg" DataFormatString="{0:D}" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="garantiSuresi" HeaderText="Garanti Süresi" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="durum" HeaderText="Durum" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="satis_tutar" HeaderText="Satış Tutarı"></asp:BoundField>
                                                    <asp:BoundField DataField="iade_tutar" HeaderText="İade Tutarı"></asp:BoundField>
                                                    <asp:BoundField DataField="musteriID" HeaderText="Müşteri ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                                        <HeaderStyle CssClass="visible-lg" />
                                                        <ItemStyle CssClass="visible-lg" />
                                                    </asp:BoundField>
                                                </Columns>

                                            </asp:GridView>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>

                            </div>

                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.panel-body -->
                </div>
                <!--  end  Context Classes  -->
            </div>
        </div>

        <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="onayModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="onayModalLabel" class="baslik">Ürün/Parça İade Bilgileri</h3>
                </div>
                <asp:UpdatePanel ID="upAdd" runat="server">

                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtIadeTutar" CssClass="col-md-4 control-label">İade Tutarı</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtIadeTutar" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="iadeGrup" ControlToValidate="txtIadeTutar" ErrorMessage="Banka adı giriniz."></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtIadeTutar" ValidationGroup="iadeGrup" Type="Currency" MinimumValue="0" MaximumValue="10000000" runat="server" />

                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtIadeAciklama" CssClass="col-md-4 control-label">Açıklama</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtIadeAciklama" CssClass="form-control" />
                                        <asp:HiddenField ID="hdnGarantiID" runat="server" />
                                        <asp:HiddenField ID="hdnCustID" runat="server" />
                                    </div>
                                </div>
                                <div class="btn-group pull-right">

                                    <asp:Button ID="btnIade" runat="server" Text="Tamam"
                                        CssClass="btn btn-success" OnClick="btnIade_Click" />
                                    <button class="btn btn-warning" data-dismiss="modal"
                                        aria-hidden="true">
                                        Kapat</button>

                                </div>

                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdUrun" EventName="RowCommand" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="editModalKredi" class="modal fade">
            tabindex="-1" role="dialog" aria-labelledby="editModalKrediLabel"
            aria-hidden="true">
              <div class="modal-dialog modal-content modal-md">
                  <div class="modal-header modal-header-info">
                      <button type="button" class="close"
                          data-dismiss="modal" aria-hidden="true">
                          ×</button>
                      <h3 id="editModalKrediLabel" class="baslik">Kredi Güncelle</h3>
                  </div>
                  <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                      <ContentTemplate>

                          <div class="modal-body">
                              <div class="form-horizontal">

                                  <div id="paketSecimDuzen" runat="server" class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="drdPaketDuzen" CssClass="col-md-3 control-label">Abonelik Paketleri</asp:Label>
                                      <div class="col-md-9">
                                          <asp:DropDownList ID="drdPaketDuzen" runat="server" CssClass="form-control" ValidationGroup="krediGrup" AutoPostBack="true" OnSelectedIndexChanged="drdPaketDuzen_SelectedIndexChanged">
                                              <asp:ListItem Text="Paket seçiniz" Value="-1" Selected="True"></asp:ListItem>
                                          </asp:DropDownList>

                                          <asp:HiddenField ID="hdnGecerliPaket" runat="server" />
                                      </div>
                                  </div>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtFiyat" CssClass="col-md-3 control-label">Paket Fiyat</asp:Label>
                                      <div class="col-md-9">
                                          <asp:TextBox runat="server" ID="txtFiyat" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="krediGrup" ControlToValidate="txtFiyat" ErrorMessage="Tutar giriniz."></asp:RequiredFieldValidator>
                                          <asp:RangeValidator ErrorMessage="Küsuratlar için virgül kullanınız" ControlToValidate="txtFiyat" ValidationGroup="krediGrup" Type="Currency" MinimumValue="0" MaximumValue="10000000" runat="server" />

                                      </div>

                                  </div>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtGun" CssClass="col-md-3 control-label">Paket Kaç Günlük</asp:Label>
                                      <div class="col-md-9">
                                          <asp:TextBox runat="server" ID="txtGun" TextMode="Number" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="krediGrup" ControlToValidate="txtGun" ErrorMessage="Paketin geçerli olduğu gün sayısını giriniz."></asp:RequiredFieldValidator>
                                          <asp:RangeValidator ErrorMessage="En az 1 gün girebilirsiniz" ControlToValidate="txtGun" ValidationGroup="krediGrup" Type="Integer" MinimumValue="1" MaximumValue="10000000" runat="server" />

                                      </div>


                                  </div>
                                  <div class="form-group">
                                      <asp:Label runat="server" AssociatedControlID="txtPeriyot" CssClass="col-md-3 control-label">Periyot(Adet)</asp:Label>
                                      <div class="col-md-9">
                                          <asp:TextBox runat="server" ID="txtPeriyot" TextMode="Number" Text="1" CssClass="form-control" />
                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="krediGrup" ControlToValidate="txtPeriyot" ErrorMessage="Paketin kaç sefer tekrarlanacağını giriniz."></asp:RequiredFieldValidator>
                                          <asp:RangeValidator ErrorMessage="Tekrar sayısı en az 1 olabilir" ControlToValidate="txtPeriyot" ValidationGroup="krediGrup" Type="Integer" MinimumValue="1" MaximumValue="10000000" runat="server" />

                                      </div>

                                  </div>

                                  <div class="form-group">

                                      <label for="tarih2" class="col-md-3 control-label">İşlem Tarihi</label>
                                      <div class="col-md-9">

                                          <input type='text' id="tarih2" runat="server" class="form-control" />

                                      </div>
                                  </div>

                                  <div class="form-group">
                                      <label class="col-sm-3 control-label">İşlem</label>
                                      <div class="col-md-9">


                                          <div class="checkbox-inline">
                                              <asp:CheckBox ID="chcSms" Text="SMS" runat="server" />
                                          </div>
                                        <%--  <div class="checkbox-inline">
                                              <asp:CheckBox ID="chcMail" Text="Mail" runat="server" />
                                          </div>--%>
                                          <div class="checkbox-inline">
                                              <asp:CheckBox ID="chcPesin" Text="Peşin/Nakit" runat="server" />
                                          </div>


                                      </div>
                                  </div>

                              </div>
                          </div>
                          <div class="modal-footer">
                              <asp:Label ID="lblResultKredi" Visible="false" runat="server"></asp:Label>
                              <asp:Button ID="btnKrediKaydet" runat="server" Text="Kaydet" CssClass="btn btn-info" ValidationGroup="krediGrup" OnClick="btnKrediKaydet_Click" />
                              <asp:Button ID="btnKrediIptal" runat="server" Text="Son yüklemeyi iptal" CssClass="btn btn-info" CausesValidation="false" OnClick="btnKrediIptal_Click" />

                              <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                          </div>
                      </ContentTemplate>
                      <Triggers>

                          <asp:AsyncPostBackTrigger ControlID="btnKredi" EventName="Click" />
                          <asp:AsyncPostBackTrigger ControlID="drdPaketDuzen" EventName="SelectedIndexChanged" />
                      </Triggers>
                  </asp:UpdatePanel>
              </div>
        </div>

        <%-- istihbaratModal kaydetme ve görüntüleme ve silme eklenecek --%>
        <div id="addModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="addModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close" data-dismiss="modal"
                        aria-hidden="true">
                        ×</button>
                    <h3 id="addModalLabel" class="baslik">Not Ekle</h3>
                </div>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">

                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtNot" CssClass="col-md-4 control-label">Notunuz</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtNot" TextMode="MultiLine" Columns="10" Rows="10" CssClass="form-control" />

                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnIstihbarat" runat="server" Text="Kaydet"
                                CssClass="btn btn-info" OnClick="btnIstihbarat_Click" />
                            <asp:Button ID="btnIstihbaratSil" runat="server" Text="Sil"
                                CssClass="btn btn-warning" OnClick="btnIstihbaratSil_Click" />
                            <button class="btn btn-info" data-dismiss="modal"
                                aria-hidden="true">
                                Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnYeniIstihbarat" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
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
