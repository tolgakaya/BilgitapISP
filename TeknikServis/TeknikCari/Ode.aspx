<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Ode.aspx.cs" Inherits="TeknikServis.TeknikCari.Ode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="kaydir">

        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 id="baslik" runat="server" class="panel-title baslik">Ödeme Kaydet</h3>
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
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="txtTutar" class="col-sm-2 control-label">Tutar</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtTutar" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" EnableClientScript="true" ControlToValidate="txtTutar" ErrorMessage="Lütfen tutar giriniz" ValidationGroup="valGrup"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Currency" EnableClientScript="true" ValidationGroup="valGrup" ControlToValidate="txtTutar" MaximumValue="10000000" MinimumValue="1" ErrorMessage="En az bir liralık tahsilat yapabilirsiniz. Küsurat için virgül kullanınız"></asp:RangeValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="txtAciklama" class="col-sm-2 control-label">Açıklama</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtAciklama" CausesValidation="true" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="valGrup"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="drdMasrafTip" CssClass="col-sm-2 control-label">Masraf Tipi</asp:Label>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="drdMasrafTip" CssClass="form-control" runat="server">
                                        <%--  <asp:ListItem Text="Masraf tipi seçiniz" Value="-1"></asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">

                                <label for="tarih2" class="col-sm-2 control-label">Ödeme Tarihi</label>
                                <div class="col-sm-10">
                                    <input type='text' id="tarih2" runat="server" class="form-control" />
                                </div>

                            </div>
                            <div class="form-group">

                                <label class="col-sm-2 control-label">İşlem</label>
                                <div class="col-md-10">
                                    <div class="checkbox-inline">
                                        <asp:CheckBox ID="cbYazdir" runat="server" Text="Makbuz" />
                                    </div>
                                    <div class="checkbox-inline">
                                        <asp:CheckBox ID="chcKal" runat="server" Text="Kal" />
                                    </div>
                                    <%--                                    <div class="checkbox-inline">
                                        <asp:CheckBox ID="chcDuzensiz" runat="server" Text="Standart Dışı" />
                                    </div>--%>
                                </div>
                            </div>


                        </div>
                    </ContentTemplate>
                    <Triggers>

                        <asp:AsyncPostBackTrigger ControlID="btnNakit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnKart" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnBanka" EventName="Click" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnCek" EventName="Click" />--%>
                    </Triggers>
                </asp:UpdatePanel>

            </div>
            <div class="panel-footer">
                <div class="btn-group">
                    <asp:LinkButton ID="btnNakit"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnKaydet_Click" ValidationGroup="valGrup"
                        Text="<i class='fa fa-file-excel-o icon-2x'>Nakit</i>" />
                    <asp:LinkButton ID="btnKart"
                        runat="server"
                        CssClass="btn btn-info " OnClick="btnKart_Click" ValidationGroup="valGrup"
                        Text="<i class='fa fa-file-excel-o icon-2x'>Kart</i>" />
                </div>
                <div class="dropup pull-right">
                    <button class="btn btn-info dropdown-toggle" type="button" id="menu1" data-toggle="dropdown">
                        Diğer
  <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu">

                        <li>
                            <asp:LinkButton ID="btnBanka"
                                runat="server"
                                CssClass="btn btn-info" ForeColor="White" OnClick="btnBanka_Click" ValidationGroup="valGrup"
                                Text="Banka Havale" /></li>
                        <%-- <li>
                            <asp:LinkButton ID="btnCek"
                                runat="server"
                                CssClass="btn btn-warning" ForeColor="White" OnClick="btnCek_Click" ValidationGroup="valGrup"
                                Text="Çek/Senet" /></li>--%>
                    </ul>
                </div>


            </div>
        </div>
        <div id="onayModal" class="modal  fade" tabindex="-1" role="dialog"
            aria-labelledby="onayModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-content modal-md">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-12">
                                    <div class="alert alert-info text-center">

                                        <i class="fa fa-2x">Kredi Kartı Bilgileri</i>

                                        <div class="form-horizontal">

                                            <div class="form-group">
                                                <asp:Label runat="server" AssociatedControlID="drdPos" CssClass="col-md-4 control-label">Kredi Kartı</asp:Label>
                                                <div class="col-md-8">
                                                    <asp:DropDownList ID="drdPos" CssClass="form-control" runat="server">
                                                        <asp:ListItem Text="Kredi kartı seçiniz" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <label for="txtTaksit" class="col-md-4 control-label">Taksit Sayısı</label>
                                                <div class="col-md-8">
                                                    <asp:TextBox runat="server" ID="txtTaksit" TextMode="Number" Text="1" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="kartGrup" ControlToValidate="txtTaksit" ErrorMessage="Tahsilatta yapılacak kesintiyi giriniz"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="chcPesin" class="col-md-4 control-label">Karta Transfer</label>
                                                <div class="col-md-8">
                                                    <asp:CheckBox ID="chcPesin" runat="server" Text="Karta Transfer" CssClass="checkbox form-control" />
                                                </div>
                                            </div>
                                            <%--
                                            </div>--%>

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

                                                <label for="tarih" class="col-sm-2 control-label">Vade</label>
                                                <div class="col-sm-10">
                                                    <input type='text' id="tarih" runat="server" class="form-control" />
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
    </div>


    <script type="text/javascript">
        function pageLoad(sender, args) {

            $('#ContentPlaceHolder1_tarih2').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
            $('#ContentPlaceHolder1_tarih').datetimepicker({
                format: 'L',

                locale: 'tr'
            });
        }
    </script>
</asp:Content>
