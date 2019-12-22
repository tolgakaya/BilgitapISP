<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HaritaAntenKaydet.aspx.cs" Inherits="BilgitapServis.HaritaAntenKaydet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>BilgiTAP Anten Kaydetme İşlemleri</title>
    <script src="../Scripts/jquery-2.1.3.min.js"></script>
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDtFQvM0e3RMXvkzOx5S6LJ0UNervLZcEQ&sensor=false&libraries=geometry"></script>

    <script type="text/javascript" src="../Scripts/v3_epoly.min.js"></script>
    <script src="../Scripts/maps.google.polygon.containsLatLng.min.js"></script>
    <script src="../Scripts/harita-anten-kaydet3.min.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap-theme.min.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">

        <div class="panel panel-primary">
            <!-- Default panel contents -->
            <div class="panel-heading">
                ANTEN HARİTA KAYDI
            </div>
            <div class="panel-body">
                <div class="table-responsive">
                    <div class="form-group col-lg-6">
                        <label for="txtAntenAdi">Anten Adı</label>

                        <input id="txtAntenAdi" runat="server" type="text" class="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="text-danger" ControlToValidate="txtAntenAdi" ValidationGroup="grup" ErrorMessage="Lütfen anten adını giriniz"></asp:RequiredFieldValidator>

                    </div>
                    <div class="form-group col-lg-6">
                        <label for="labelPozisyonA">Anten Açısı</label>

                        <input id="txtAci" runat="server" type="text" disabled="disabled" class="form-control" />
                        <%--<input type="button" id="btnGoster" value="Göster" class="btn btn-success" onclick="//goster()"/>--%>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtAci" Type="Integer" MinimumValue="0" EnableClientScript="true" MaximumValue="360" ValidationGroup="grup" CssClass="text-danger" ErrorMessage="Negatif açı girdiniz. Lütfen B ve C ikonlarının yerini değiştiriniz."></asp:RangeValidator>
                        <div class="btn-group pull-right hidden-xs hidden-sm">

                            <asp:Button ID="btnAna" runat="server" Text="Giriş" CssClass="btn btn-primary" OnClick="btnAna_Click" />
                            <asp:Button ID="btnAntenKaydet" runat="server" Text="Anten Kaydet" CssClass="btn btn-info" OnClick="btnAntenKaydet_Click" ValidationGroup="grup" CausesValidation="true" />
                            <asp:Button ID="btnGeri" runat="server" Text="Geri Dön" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                            <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />

                        </div>
                        <div class="btn-group pull-right visible-xs visible-sm">

                            <asp:Button ID="Button4" runat="server" Text="Giriş" CssClass="btn btn-primary btn-sm" OnClick="btnAna_Click" />
                            <asp:Button ID="Button5" runat="server" Text="Anten Kaydet" CssClass="btn btn-info btn-sm" OnClick="btnAntenKaydet_Click" ValidationGroup="grup" CausesValidation="true" />
                            <asp:Button ID="Button6" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-sm" OnClick="btnGeri_Click" />
                            <input type="button" value="Yenile" class="btn btn-success btn-sm" onclick="document.location.reload(true)" />

                        </div>
                    </div>
                    <input id="labelPozisyonA" runat="server" type="hidden" />

                    <input id="labelPozisyonB" runat="server" type="hidden" />

                    <input id="labelPozisyonC" runat="server" type="hidden" />



                    <div class="form-group col-lg-8 pull-left" id="map_canvas" style="width: 70%; height: 900px"></div>
                    <div id="pano" class="col-lg-4 pull-left" style="width: 30%; height: 900px"></div>
                </div>
            </div>


            <div class="panel-footer pull-right">
                <div class="btn-group pull-right hidden-xs hidden-sm">

                    <asp:Button ID="Button1" runat="server" Text="Giriş" CssClass="btn btn-primary" OnClick="btnAna_Click" />
                    <asp:Button ID="Button2" runat="server" Text="Anten Kaydet" CssClass="btn btn-info" OnClick="btnAntenKaydet_Click" ValidationGroup="grup" CausesValidation="true" />
                    <asp:Button ID="Button3" runat="server" Text="Geri Dön" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                    <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />

                </div>
                <div class="btn-group pull-right visible-xs visible-sm">

                    <asp:Button ID="Button7" runat="server" Text="Giriş" CssClass="btn btn-primary btn-sm" OnClick="btnAna_Click" />
                    <asp:Button ID="Button8" runat="server" Text="Anten Kaydet" CssClass="btn btn-info btn-sm" OnClick="btnAntenKaydet_Click" ValidationGroup="grup" CausesValidation="true" />
                    <asp:Button ID="Button9" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-sm" OnClick="btnGeri_Click" />
                    <input type="button" value="Yenile" class="btn btn-success btn-sm" onclick="document.location.reload(true)" />

                </div>

            </div>

        </div>



    </form>
</body>
</html>
