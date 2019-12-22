<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MusteriYolu.aspx.cs" Inherits="TeknikServis.MusteriYolu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Müşteri Yolu</title>
    <script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyDtFQvM0e3RMXvkzOx5S6LJ0UNervLZcEQ&sensor=false&language=tr"></script>
    <script src="../Scripts/jquery-2.1.3.min.js"></script>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap-theme.min.css" rel="stylesheet" />
    <script>
        //var musteri = new google.maps.LatLng(36.11052661014851, 32.99344551822287);
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        };
        var musteri;
        var id = getParameterByName('id');

        var urlBase = "/api/Musteri/";
        var url = urlBase + "?id=" + id;
        function GetMusteri() {


            $.getJSON(url,
                function (data) {
                    var musteri_id = data.musteri_id;
                    var musteri_adi = data.musteri_adi;
                    var center_Lat = data.center_Lat;
                    var center_Long = data.center_Long;

                    musteri = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi };
                })
            .fail(
                function (jqXHR, textStatus, err) {

                });
            return musteri;
        }

        musteri = GetMusteri();

        (function () {
            var directionsService = new google.maps.DirectionsService(),
                directionsDisplay = new google.maps.DirectionsRenderer(),
                createMap = function (start) {
                    var travel = {
                        origin: (start.coords) ? new google.maps.LatLng(start.lat, start.lng) : start.address,
                        destination: musteri.center,//"Alexanderplatz, Berlin",
                        travelMode: google.maps.DirectionsTravelMode.DRIVING
                        // Exchanging DRIVING to WALKING above can prove quite amusing :-)
                    },
                        mapOptions = {
                            zoom: 10,
                            // Default view: downtown Stockholm
                            center: new google.maps.LatLng(59.3325215, 18.0643818),
                            mapTypeId: google.maps.MapTypeId.ROADMAP
                        };

                    map = new google.maps.Map(document.getElementById("map"), mapOptions);
                    directionsDisplay.setMap(map);
                    directionsDisplay.setPanel(document.getElementById("map-directions"));
                    directionsService.route(travel, function (result, status) {
                        if (status === google.maps.DirectionsStatus.OK) {
                            directionsDisplay.setDirections(result);
                        }
                    });
                };

            // Check for geolocation support
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    // Success!
                    createMap({
                        coords: true,
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    });
                },
                    function () {
                        // Gelocation fallback: Defaults to Stockholm, Sweden
                        createMap({
                            coords: false,
                            address: "Anamur, Türkiye"
                        });
                    }
                );
            }
            else {
                // No geolocation fallback: Defaults to Lisbon, Portugal
                createMap({
                    coords: false,
                    address: "BOZYAZI, TÜRKİYE"
                });
            }
        })();
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <header role="banner">
            </header>

            <div class="container-fluid">
                <section id="main-content">
                    <div class="btn-group pull-right hidden-xs hidden-sm">

                        <asp:Button ID="btnAna" runat="server" Text="Giriş" CssClass="btn btn-primary" OnClick="btnAna_Click" />
                        <asp:Button ID="Button1" runat="server" Text="Geri Dön" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                        <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />

                    </div>
                    <div class="btn-group pull-right visible-xs visible-sm">

                        <asp:Button ID="Button4" runat="server" Text="Giriş" CssClass="btn btn-primary btn-sm" OnClick="btnAna_Click" />
                        <asp:Button ID="Button5" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-sm" OnClick="btnGeri_Click" />
                        <input type="button" value="Yenile" class="btn btn-success btn-sm" onclick="document.location.reload(true)" />

                    </div>
                    <div id="map-container">


                        <div id="map-directions"></div>
                        <div id="map" style="width: 100%; height: 600px"></div>

                    </div>
                    <div class="btn-group pull-right hidden-xs hidden-sm">

                        <asp:Button ID="Button2" runat="server" Text="Giriş" CssClass="btn btn-primary" OnClick="btnAna_Click" />
                        <asp:Button ID="Button3" runat="server" Text="Geri Dön" CssClass="btn btn-danger" OnClick="btnGeri_Click" />
                        <input type="button" value="Yenile" class="btn btn-success" onclick="document.location.reload(true)" />

                    </div>
                    <div class="btn-group pull-right visible-xs visible-sm">

                        <asp:Button ID="Button6" runat="server" Text="Giriş" CssClass="btn btn-primary btn-sm" OnClick="btnAna_Click" />
                        <asp:Button ID="Button7" runat="server" Text="Geri Dön" CssClass="btn btn-danger btn-sm" OnClick="btnGeri_Click" />
                        <input type="button" value="Yenile" class="btn btn-success btn-sm" onclick="document.location.reload(true)" />

                    </div>
                </section>
            </div>

        </div>
    </form>
</body>
</html>
