<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustQ.aspx.cs" ViewStateMode="Disabled" Inherits="TeknikServis.CustQ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/main-style.min.css" rel="stylesheet" />
    <link href="Content/alertify-bootstrap-3.css" rel="stylesheet" />

    <link href="Content/style.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.3.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/alertify.min.js"></script>
    <link href="Content/font-awesome.min.css" rel="stylesheet" />

</head>
<body>

    <form id="form1" enableviewstate="false" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="kaydir">

            <div class="panel panel-info">
                <div class="panel-heading">
                    SERVİS DURUM SORGUSU
                </div>
                <div class="panel-body">


                    <div class="input-group custom-search-form">
                        <input runat="server" type="text" id="txtAra" class="form-control" placeholder="Servis numarası yada TC giriniz..." />
                        <span class="input-group-btn">
                            <button id="btnARA" runat="server" class="btn btn-default" type="submit" onserverclick="ServisAra">
                                <i class="fa fa-search"></i>
                            </button>
                        </span>
                    </div>
                </div>
            </div>

            <div class="panel-group">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">

                    <ContentTemplate>
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>

                                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                </div>

                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <div id="grdKararlar" runat="server" visible="false" class="panel panel-default">
                            <div class="panel-heading">
                                <h4 class="panel-title">Onayınız Beklenen Kararlar
                                </h4>
                            </div>

                            <%-- Servis Durum başlıyor --%>

                            <div class="panel-body">

                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                                    DataKeyNames="hesapID"
                                    EmptyDataText="Bütün servis kararları onaylanmış">
                                    <PagerStyle CssClass="pagination-ys" />

                                    <Columns>

                                        <asp:BoundField DataField="hesapID" HeaderText="ID" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg">
                                            <HeaderStyle CssClass="visible-lg" />
                                            <ItemStyle CssClass="visible-lg" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="islemParca" HeaderText="İşlem" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="urun_cinsi" HeaderText="Cihaz" HeaderStyle-CssClass="visible-lg visible-xs visible-sm" ItemStyle-CssClass="visible-lg visible-xs visible-sm">
                                            <HeaderStyle CssClass="visible-lg visible-xs visible-sm" />
                                            <ItemStyle CssClass="visible-lg visible-xs visible-sm" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="aciklama" HeaderText="Açıklama" HeaderStyle-CssClass="visible-lg " ItemStyle-CssClass="visible-lg "></asp:BoundField>
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
                                    </Columns>

                                </asp:GridView>
                                <asp:Button ID="btnOnay" runat="server" Text="Onayla" CssClass="btn btn-danger btn-block"
                                    OnClick="btnOnay_Click" />


                            </div>

                            <%-- servis durum bitiyor--%>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />
                        <asp:AsyncPostBackTrigger ControlID="btnOnay" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>

                            <label runat="server" id="musteri"></label>

                        </h4>
                    </div>
                    <%-- servis tipleri başlıyor --%>

                    <div class="panel-body">
                        <asp:UpdatePanel ID="upCrudGrid"  runat="server">
                            <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>

                                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999;">
                                            <asp:Image ID="imgUpdateProgresss" runat="server" ImageUrl="~/img/ajax_loader_blue_64.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                                        </div>

                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <div id="tablolar" runat="server" visible="false">
                                    <div id="tablo" runat="server" class="row">


                                        <div class="col-lg-4">
                                            <div class="alert alert-info text-center">
                                                <i class="fa fa-calendar fa-3x" id="internet" runat="server"></i>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="alert alert-success text-center">
                                                <i class="fa  fa-thumbs-up fa-3x" id="gecerlilikTarih" runat="server"></i>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="alert alert-info text-center">
                                                <i class="fa fa-rss fa-3x" id="gecerlilik" runat="server"></i>&nbsp; Gün Kaldı

                                            </div>
                                        </div>

                                    </div>
                                    <div id="tablo2" runat="server" class="row">

                                        <div class="col-lg-4">
                                            <div class="panel panel-primary text-center no-boder">
                                                <div class="panel-body yellow">
                                                    <i class="fa fa-bar-chart-o fa-3x"></i>
                                                    <h3 id="borc" runat="server"></h3>
                                                </div>
                                                <div class="panel-footer">
                                                    <span class="panel-eyecandy-title">Günü Gelen Ödeme
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="panel panel-primary text-center no-boder">
                                                <div class="panel-body blue">
                                                    <i class="fa fa-pencil-square-o fa-3x"></i>
                                                    <h3 id="durum" runat="server"></h3>
                                                </div>
                                                <div class="panel-footer">
                                                    <span class="panel-eyecandy-title">Servis Durumu
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="panel panel-primary text-center no-boder">
                                                <div class="panel-body green">
                                                    <i class="fa fa fa-floppy-o fa-3x"></i>
                                                    <h3 id="aciklama" runat="server"></h3>
                                                </div>
                                                <div class="panel-footer">
                                                    <span class="panel-eyecandy-title">Servis Tipi
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div id="tablo3" runat="server" visible="false" class="row">

                                        <div class="col-lg-4">
                                            <div class="panel panel-primary text-center no-boder">
                                                <div class="panel-body yellow">
                                                    <i class="fa fa-bar-chart-o fa-3x"></i>
                                                    <h3 id="sonrakiTutar" runat="server"></h3>
                                                </div>
                                                <div class="panel-footer">
                                                    <span class="panel-eyecandy-title">Bir Sonraki Ödeme
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="panel panel-primary text-center no-boder">
                                                <div class="panel-body blue">
                                                    <i class="fa fa-pencil-square-o fa-3x"></i>
                                                    <h3 id="sonrakiTarih" runat="server"></h3>
                                                </div>
                                                <div class="panel-footer">
                                                    <span class="panel-eyecandy-title">Sonraki Ödeme Tarihi
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="panel panel-primary text-center no-boder">
                                                <div class="panel-body green">
                                                    <i class="fa fa fa-floppy-o fa-3x"></i>
                                                    <h3 id="toplamBorc" runat="server"></h3>
                                                </div>
                                                <div class="panel-footer">
                                                    <span class="panel-eyecandy-title">Toplam Borç
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnARA" EventName="ServerClick" />

                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <%-- servis tipleri bitiyor --%>
                </div>

            </div>


        </div>
    </form>
</body>
</html>
