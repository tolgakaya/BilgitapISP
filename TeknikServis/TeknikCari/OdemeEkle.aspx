<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="OdemeEkle.aspx.cs" Inherits="TeknikServis.OdemeEkle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 id="baslik" runat="server" class="panel-title baslik">Tahsilat Kaydet</h3>
            </div>
            <div class="panel-body">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(jScript);
                        </script>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="txtTutar" class="col-sm-2 control-label">Tutar</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtTutar" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" ControlToValidate="txtTutar" ErrorMessage="Lütfen tutar giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="valGrup" ControlToValidate="txtTutar" MaximumValue="10000000" MinimumValue="0" ErrorMessage="En az bir liralık tahsilat yapabilirsiniz. Küsurat için virgül kullanınız"></asp:RangeValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="txtAciklama" class="col-sm-2 control-label">Açıklama</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtAciklama" CausesValidation="true" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">

                                <label class="col-sm-2 control-label">İşlem</label>
                                <div class="col-md-10">

<%--                                    <div class="checkbox-inline">

                                        <asp:CheckBox ID="chcSms" Text="SMS" runat="server" />
                                    </div>
                                    <div class="checkbox-inline">

                                        <asp:CheckBox ID="chcMail" Text="Mail" runat="server" />
                                    </div>--%>
                                    <div class="checkbox-inline">

                                        <asp:CheckBox ID="cbYazdir" runat="server" Text="Makbuz" />
                                    </div>

                                </div>
                            </div>
                            <div class="form-group">

                                <label for="tarih2" class="col-sm-2 control-label">Tahsilat Tarihi</label>
                                <div class="col-sm-10">

                                    <input type='text' id="tarih2" runat="server" class="form-control" />

                                </div>

                            </div>

                        </div>


                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnNakit" EventName="Click" />

                        <asp:AsyncPostBackTrigger ControlID="btnKart" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAyni" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAyniK" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBanka" EventName="Click" />

                        <asp:AsyncPostBackTrigger ControlID="btnKartMahsup" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnKartMahsupK" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
            <div class="panel-footer">
                <div class="btn-group">
                    <asp:LinkButton ID="btnNakit"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnKaydet_Click" ValidationGroup="valGrup"
                        Text="Nakit" />
                    <asp:LinkButton ID="btnKart"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnKart_Click" ValidationGroup="valGrup"
                        Text="Kart" />
                    <asp:LinkButton ID="btnBanka"
                        runat="server"
                        CssClass="btn btn-info" ForeColor="White" OnClick="btnBanka_Click" ValidationGroup="valGrup"
                        Text="Banka" />
                </div>
                <div class="dropup pull-right visible-md visible-lg">
                    <button class="btn btn-info dropdown-toggle" type="button" id="menu2" data-toggle="dropdown">
                        Diğer
  <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu">
                        <li>
                            <asp:LinkButton ID="btnAyni"
                                runat="server"
                                CssClass="btn btn-info" ForeColor="White" OnClick="btnAyni_Click" ValidationGroup="valGrup"
                                Text="Ayni" /></li>

                        <li>
                            <asp:LinkButton ID="btnKartMahsup"
                                runat="server"
                                CssClass="btn btn-info" ForeColor="White" OnClick="btnKartMahsup_Click" ValidationGroup="valGrup"
                                Text="Müşteri Kartı" /></li>



                    </ul>
                </div>

                <div class="dropup pull-right visible-xs">
                    <button class="btn btn-primary dropdown-toggle" type="button" id="menu1" data-toggle="dropdown">
                        D
  <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu">
                        <li>
                            <asp:LinkButton ID="btnAyniK"
                                runat="server"
                                CssClass="btn btn-info" ForeColor="White" OnClick="btnAyni_Click" ValidationGroup="valGrup"
                                Text="Ayni" /></li>

                        <li>
                            <asp:LinkButton ID="btnKartMahsupK"
                                runat="server"
                                CssClass="btn btn-info" ForeColor="White" OnClick="btnKartMahsup_Click" ValidationGroup="valGrup"
                                Text="Müşteri Kartı" /></li>



                    </ul>
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
                                                CssClass="btn btn-info" OnClick="btnKartKaydet_Click" />
                                            <button class="btn btn-info" data-dismiss="modal"
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

        <div id="bankaModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="bankaModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-sm">

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">
                                        <i class="fa fa-2x">Banka Havalesi</i>

                                        <asp:DropDownList ID="drdBanka" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Banka seçiniz" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>


                                        <div class="btn-group pull-right">

                                            <asp:Button ID="btnBankaKaydet" runat="server" Text="Tamam"
                                                CssClass="btn btn-info" OnClick="btnBankaKaydet_Click" />
                                            <button class="btn btn-info" data-dismiss="modal"
                                                aria-hidden="true">
                                                Kapat</button>

                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hdnHesapID2" runat="server" />
                                <asp:HiddenField ID="hdnServisIDD2" runat="server" />
                                <asp:HiddenField ID="hdnIslemm2" runat="server" />
                                <asp:HiddenField ID="hdnYekunn2" runat="server" />
                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBankaKaydet" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <%-- <div id="cekModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="cekModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">

                <asp:UpdatePanel ID="UpdatePanel4" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">


                                        <i class="fa fa-2x">Çek/Senet Bilgileri</i>

                                        <div class="form-vertical">
                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtCekBelgeNo" CssClass="col-md-4 control-label">Belge No</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtCekBelgeNo" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="cekGrup" ControlToValidate="txtCekBelgeNo" ErrorMessage="Lütfen belge numarası giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="txtCekMasraf" CssClass="col-md-4 control-label">Masraf</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtCekMasraf" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="cekGrup" ControlToValidate="txtCekMasraf" ErrorMessage="Tahsilatta yapılacak kesintiyi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>

                                            <div class="form-group">

                                                <label for="tarih22" class="col-md-4 control-label">Vade</label>
                                                <div class="col-md-8">

                                                    <input type='text' id="tarih22" runat="server" class="form-control" />

                                                </div>
                                            </div>

                                            <div class="btn-group pull-right">

                                                <asp:Button ID="btnCekKaydet" runat="server" ValidationGroup="cekGrup" Text="Tamam"
                                                    CssClass="btn btn-success" OnClick="btnCekKaydet_Click" />
                                                <button class="btn btn-warning" data-dismiss="modal"
                                                    aria-hidden="true">
                                                    Kapat</button>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCekKaydet" EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>--%>

        <div id="mahsupModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="mahsupModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">
                <div class="modal-header modal-header-info">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        ×</button>
                    <h3 id="mahsupModalLabel">Ödeme Yapacağınız Kişi</h3>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">

                    <ContentTemplate>
                        <asp:HiddenField runat="server" ID="hdnMahsup"></asp:HiddenField>
                        <div class="modal-body">
                            <div class="table-responsive">
                                <div class="input-group custom-search-form">
                                    <input runat="server" type="text" id="txtMusteriSorgu" class="form-control" placeholder="Ara..." />
                                    <span class="input-group-btn">
                                        <button id="btnMusteriAra" runat="server" class="btn btn-default" type="submit" onserverclick="MusteriAra">
                                            <i class="fa fa-search"></i>
                                        </button>
                                    </span>
                                </div>
                                <%-- buradannnnnn --%>

                                <asp:GridView ID="grdMahsup" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="CustID"
                                    EmptyDataText="Kayıt girilmemiş" EnablePersistedSelection="false"
                                    OnSelectedIndexChanged="grdMahsup_SelectedIndexChanged" SelectedIndex="0">
                                    <SelectedRowStyle CssClass="danger" />

                                    <Columns>
                                        <asp:ButtonField CommandName="Select" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Seç">
                                            <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CustID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                            <HeaderStyle CssClass="visible-lg" />
                                            <ItemStyle CssClass="visible-lg" />
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Müşteri Adı" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
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
                                        <asp:BoundField DataField="Tc" HeaderText="Tc" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                            <HeaderStyle CssClass="visible-lg" />
                                            <ItemStyle CssClass="visible-lg" />
                                        </asp:BoundField>
                                    </Columns>

                                </asp:GridView>
                                <%-- burayaaaa --%>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtYansiyan" CssClass="col-md-12 control-label">Firmaya yansıyacak tutar</asp:Label>
                                    <div class="col-md-12">
                                        <asp:TextBox runat="server" ID="txtYansiyan" ValidationGroup="detayGrup" CssClass="form-control" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                            <asp:LinkButton ID="btnKartMahsupKaydet"
                                runat="server"
                                CssClass="btn btn-danger" ForeColor="White" OnClick="btnKartMahsupKaydet_Click" ValidationGroup="valGrup"
                                Text="Mahsuben Kaydet" />
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Kapat</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="btnCekKaydet" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('#ContentPlaceHolder1_tarih22').datetimepicker({
                format: 'L',

                locale: 'tr'
            });

            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>


</asp:Content>
