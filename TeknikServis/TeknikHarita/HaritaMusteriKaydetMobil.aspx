<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HaritaMusteriKaydetMobil.aspx.cs" Inherits="TeknikServis.HaritaMusteriKaydetMobil" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>BilgiTAP Anten Kaydetme İşlemleri</title>
    <script src="../Scripts/jquery-2.1.3.min.js"></script>
    <%--<script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDtFQvM0e3RMXvkzOx5S6LJ0UNervLZcEQ&sensor=false"></script>--%>
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDtFQvM0e3RMXvkzOx5S6LJ0UNervLZcEQ&sensor=false&libraries=geometry"></script>
      <script src="../Scripts/harita-musteri-kaydet-mobil3.min.js"></script>
    <%--<script src="../Scripts/harita-musteri-kaydet-mobil4.js"></script>--%>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap-theme.min.css" rel="stylesheet" />
</head>
<body onload="initialize()">
    <form id="form1" runat="server">


        <div class="panel panel-primary">
            <!-- Default panel contents -->
            <div class="panel-heading">
                MÜŞTERİ ADRES KAYDI
            </div>
            <div class="panel-body">
                <div class="table-responsive">
                    <div class="form-group">
                        <label for="txtAddress" class="label">Address</label>
                        <input id="txtAddress" runat="server" type="text" class="form-control" />
                        <input id="btnGetLatLng" type="button" class="btn btn-danger btn-block" value="Ara" onclick="getLatLng();" />
                    </div>
                    <%-- burada textboxı hiddena çevirdim   <div class="form-group">
                        <label for="txtLatitude" class="label">Latitude</label>--%>
                    <%--       <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control"></asp:TextBox>-- %>--%>
                    <input id="txtLatitude" runat="server" type="hidden" />
                </div>
                <div class="btn-group pull-right">
                    <asp:Button ID="btnMusteriKaydet" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnMusteriKaydet_Click" />
                    <asp:Button ID="Button1" runat="server" Text="Giriş" CssClass="btn btn-info" OnClick="btnAna_Click" />
                    <asp:Button ID="Button3" runat="server" Text="Geri" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                    <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />
                </div>

                <div class="form-group col-lg-8 pull-left" id="myMap" style="width: 70%; height: 900px"></div>
                <div id="pano" class="form-group col-lg-4 pull-left" style="width: 30%; height: 900px"></div>
            </div>
        </div>

        <div class="panel-footer pull-right">
            <div class="btn-group">
                <asp:Button ID="Button2" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnMusteriKaydet_Click" />
                <asp:Button ID="Button4" runat="server" Text="Giriş" CssClass="btn btn-info" OnClick="btnAna_Click" />
                <asp:Button ID="Button5" runat="server" Text="Geri" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />
            </div>
        </div>

    </form>
</body>
</html>
