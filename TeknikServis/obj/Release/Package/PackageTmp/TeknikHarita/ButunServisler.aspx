﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ButunServisler.aspx.cs" Inherits="TeknikServis.TeknikHarita.ButunServisler" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>BilgiTAP Müşteri Haritası</title>
    <script src="../Scripts/jquery-2.1.3.min.js"></script>
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDtFQvM0e3RMXvkzOx5S6LJ0UNervLZcEQ&sensor=false&libraries=geometry"></script>
    <%--<script type="text/javascript" src="Scripts/v3_epoly.js"></script>--%>
    <script src="../Scripts/harita-servisler.min.js"></script>

    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap-theme.min.css" rel="stylesheet" />
</head>
<body onload="initializeMusteriler()">
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <!-- Default panel contents -->
            <div class="panel-heading">
                SERVİS HARİTASI
            </div>
            <div class="panel-body">
                <div class="btn-group pull-right hidden-xs hidden-sm">
                    <input id="txtNoktalar" runat="server" type="hidden" />
                    <asp:Button ID="btnHaritayaGore" runat="server" Text="Haritaya Göre Kaydet" CssClass="btn btn-primary" OnClick="btnHaritayaGore_Click" />
                    <asp:Button ID="Button1" runat="server" Text="Giriş" CssClass="btn btn-primary" OnClick="btnAna_Click" />
                    <asp:Button ID="Button3" runat="server" Text="Geri Dön" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                    <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />
                </div>

                <div class="btn-group pull-right visible-xs visible-sm">

                    <asp:Button ID="Button2" runat="server" Text="Giriş" CssClass="btn btn-primary btn-sm" OnClick="btnAna_Click" />
                    <asp:Button ID="Button4" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-sm" OnClick="btnGeri_Click" />
                    <input type="button" value="Yenile" class="btn btn-success btn-sm" onclick="document.location.reload(true)" />
                </div>
            </div>
            <div class="table-responsive">
                  <div class="form-group col-lg-8 pull-left" id="map_canvas" style="width: 70%; height: 900px"></div>
                <div id="pano" class="form-group col-lg-4 pull-left" style="width: 30%; height: 900px"></div>
            </div>

            <div class="panel-footer pull-right">
                <div class="btn-group pull-right hidden-xs hidden-sm">

                    <asp:Button ID="Button5" runat="server" Text="Giriş" CssClass="btn btn-primary" OnClick="btnAna_Click" />
                    <asp:Button ID="Button6" runat="server" Text="Geri Dön" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                    <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />
                </div>

                <div class="btn-group pull-right visible-xs visible-sm">

                    <asp:Button ID="Button7" runat="server" Text="Giriş" CssClass="btn btn-primary btn-sm" OnClick="btnAna_Click" />
                    <asp:Button ID="Button8" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-sm" OnClick="btnGeri_Click" />
                    <input type="button" value="Yenile" class="btn btn-success btn-sm" onclick="document.location.reload(true)" />
                </div>
            </div>

        </div>
    </form>
</body>
</html>
