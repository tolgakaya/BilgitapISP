﻿
var map = null;
var bounds = null;

var antenler = {};
var poligonlar = {};
var hepsi = {};
var EarthRadiusMeters = 6378137.0; // meters

google.maps.LatLng.prototype.DestinationPoint = function (brng, dist) {
    var R = EarthRadiusMeters; // earth's mean radius in meters
    var brng = brng.toRad();
    var lat1 = this.lat().toRad(), lon1 = this.lng().toRad();
    var lat2 = Math.asin(Math.sin(lat1) * Math.cos(dist / R) +
                          Math.cos(lat1) * Math.sin(dist / R) * Math.cos(brng));
    var lon2 = lon1 + Math.atan2(Math.sin(brng) * Math.sin(dist / R) * Math.cos(lat1),
                                 Math.cos(dist / R) - Math.sin(lat1) * Math.sin(lat2));

    return new google.maps.LatLng(lat2.toDeg(), lon2.toDeg());
}


google.maps.LatLng.prototype.Bearing = function (otherLatLng) {
    var from = this;
    var to = otherLatLng;
    if (from.equals(to)) {
        return 0;
    }
    var lat1 = from.latRadians();
    var lon1 = from.lngRadians();
    var lat2 = to.latRadians();
    var lon2 = to.lngRadians();
    var angle = -Math.atan2(Math.sin(lon1 - lon2) * Math.cos(lat2), Math.cos(lat1) * Math.sin(lat2) - Math.sin(lat1) * Math.cos(lat2) * Math.cos(lon1 - lon2));
    if (angle < 0.0) angle += Math.PI * 2.0;
    if (angle > Math.PI) angle -= Math.PI * 2.0;
    return parseFloat(angle.toDeg());
}


Number.prototype.toRad = function () {
    return this * Math.PI / 180;
};

Number.prototype.toDeg = function () {
    return this * 180 / Math.PI;
};


Number.prototype.toBrng = function () {
    return (this.toDeg() + 360) % 360;
};

var infowindow = new google.maps.InfoWindow(
  {
      size: new google.maps.Size(150, 50)
  });



function createMarker(latlng, html) {
    var contentString = html;
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        icon: '/img/anten2.png',
        animation: google.maps.Animation.DROP,
        zIndex: Math.round(latlng.lat() * -100000) << 5
    });
    bounds.extend(latlng);
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.setContent(contentString);
        infowindow.open(map, marker);
    });
}

function createMarkerM(latlng, html) {
    var contentString = html;
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        icon: '/img/musteri.png',
        animation: google.maps.Animation.BOUNCE,
        zIndex: Math.round(latlng.lat() * -100000) << 5
    });
    bounds.extend(latlng);
    google.maps.event.addListener(marker, 'click', function () {
        infowindow.setContent(contentString);
        infowindow.open(map, marker);
    });
}


function drawArc(center, initialBearing, finalBearing, radius) {
    var d2r = Math.PI / 180;   // degrees to radians
    var r2d = 180 / Math.PI;   // radians to degrees

    var points = 32;

    // find the raidus in lat/lon
    var rlat = (radius / EarthRadiusMeters) * r2d;
    var rlng = rlat / Math.cos(center.lat() * d2r);

    var extp = new Array();

    if (initialBearing > finalBearing) finalBearing += 360;
    var deltaBearing = finalBearing - initialBearing;
    deltaBearing = deltaBearing / points;
    for (var i = 0; (i < points + 1) ; i++) {
        extp.push(center.DestinationPoint(initialBearing + i * deltaBearing, radius));
        bounds.extend(extp[extp.length - 1]);
    }
    return extp;
}


function drawCircle(point, radius) {
    var d2r = Math.PI / 180;   // degrees to radians
    var r2d = 180 / Math.PI;   // radians to degrees
    var EarthRadiusMeters = 6378137.0; // meters
    var earthsradius = 3963; // 3963 is the radius of the earth in miles

    var points = 32;

    // find the raidus in lat/lon
    var rlat = (radius / EarthRadiusMeters) * r2d;
    var rlng = rlat / Math.cos(point.lat() * d2r);


    var extp = new Array();
    for (var i = 0; i < points + 1; i++) // one extra here makes sure we connect the
    {
        var theta = Math.PI * (i / (points / 2));
        ey = point.lng() + (rlng * Math.cos(theta)); // center a + radius x * cos(theta)
        ex = point.lat() + (rlat * Math.sin(theta)); // center b + radius y * sin(theta)
        extp.push(new google.maps.LatLng(ex, ey));
        bounds.extend(extp[extp.length - 1]);
    }
    // alert(extp.length);
    return extp;
}
function GetAllAntens() {

    $.ajax({
        type: "GET",
        url: "/api/Anten/",
        contentType: "json",
        dataType: "json",
        success: function (data) {

            $.each(data, function (key, value) {
                //stringify
                var jsonData = JSON.stringify(value);
                //Parse JSON
                var objData = $.parseJSON(jsonData);
                var anten_adi = objData.anten_adi;
                var center_Lat = objData.center_Lat;
                var center_Long = objData.center_Long;
                var start_Lat = objData.start_Lat;
                var start_Long = objData.start_Long;
                var end_Lat = objData.end_Lat;
                var end_Long = objData.end_Long;
                var bayi = objData.bayi;
                var anten_id = objData.anten_id;
                hepsi[anten_adi] = { center: new google.maps.LatLng(center_Lat, center_Long), start: new google.maps.LatLng(start_Lat, start_Long), end: new google.maps.LatLng(end_Lat, end_Long), adi: anten_adi, bayi: bayi, anten_id: anten_id };


            });
        },
        error: function (xhr) {
            console.log(xhr);
        }
    });

    return hepsi;
}
antenler = GetAllAntens();

//querystringden okunacak
var id = getParameterByName('id');
//var id = 74;
var urlBase = "/api/Musteri/";
var url = urlBase + "?id=" + id;

var musteri;

function GetMusteri() {

    //var id = $('#<%=txtSearch.ClientID %>').val();
    $.getJSON(url,
        function (data) {
            var musteri_id = data.musteri_id;
            var musteri_adi = data.musteri_adi;
            var center_Lat = data.center_Lat;
            var center_Long = data.center_Long;
          
            var bayi = data.bayi;
            musteri = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id, bayi: bayi };
        })
    .fail(
        function (jqXHR, textStatus, err) {

        });
    return musteri;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

function tekli(startPoint, endPoint, centerPoint, adi, bayi, anten_id) {

    //createMarker(centerPoint, "Anten: " + adi + " Yönetici: " + bayi +
    //          "<br><a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint.toUrlValue(6) + "));map.setZoom(20);'>Yaklaş</a> - <a href='javascript:map.fitBounds(bounds);'>Uzaklaş</a>" +
    //      "<br><a href='TekAnten.aspx?id=" + anten_id + "'>Anten müşterilerine git </a>");

    createMarker(centerPoint, "<div class='panel panel-primary'>" +

"<div class='panel-heading'>" +
adi + "/" + bayi +
"</div>" +
"<div class='panel-body'>" +
"<div class='table-responsive'>" +
"<div class='btn btn-xs-group'>" +
"<a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint.toUrlValue(6) + "));map.setZoom(20);'><i class='btn btn-xs btn-success '>Yaklaş</i></a> <a href='javascript:map.fitBounds(bounds);'><i class='btn btn-xs btn-danger'>Uzaklaş</i></a>" +
"<a href='TekAnten.aspx?id=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Anten Müşterilerine Git</i> </a>" +

"</div>" +
"</div>" +

"</div>" +

" </div>");

    var arcPts = drawArc(centerPoint, centerPoint.Bearing(startPoint), centerPoint.Bearing(endPoint), centerPoint.distanceFrom(startPoint));

    arcPts.push(centerPoint);
    bounds.extend(centerPoint);
    arcPts.push(startPoint);

    var piePoly = new google.maps.Polygon({
        paths: [arcPts],
        strokeColor: "#00FF00",
        strokeOpacity: 0.5,
        strokeWeight: 2,
        fillColor: "#FF0000",
        fillOpacity: 0.35,
        map: map
    });
    poligonlar[centerPoint] = { center: centerPoint, poly: piePoly };
};

musteri = GetMusteri();

function initialize() {
    var myOptions = {
        zoom: 10,
        center: new google.maps.LatLng(-33.9, 151.2),
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map = new google.maps.Map(document.getElementById("map_canvas"),
                                  myOptions);

    bounds = new google.maps.LatLngBounds();

    google.maps.event.addListener(map, 'click', function () {
        infowindow.close();
    });



    for (var city in antenler) {
        tekli(antenler[city].start, antenler[city].end, antenler[city].center, antenler[city].adi, antenler[city].bayi, antenler[city].anten_id);
    };
    //verilen bir noktanın hangi anten kapsamında olduğunu buluyoruz

    for (var poli in poligonlar) {
        if (poligonlar[poli].poly.containsLatLng(musteri.center)) {


            createMarkerM(musteri.center, "<div class='panel panel-primary'>" +

"<div class='panel-heading'>" +
  musteri.adi + "/" +
"</div>" +
"<div class='panel-body'>" +
    "<div class='table-responsive'>" +
               "<div class='btn btn-xs-group'>" +
"<a href='javascript:map.setCenter(new google.maps.LatLng(" + musteri.center.toUrlValue(6) + "));map.setZoom(20);'><i class='btn btn-xs btn-success '>Yaklaş</i></a> <a href='javascript:map.fitBounds(bounds);'><i class='btn btn-xs btn-danger'>Uzaklaş</i></a>" +
"<a href='../MusteriDetayBilgileri.aspx?custID=" + musteri.musteri_id + "'><i class='btn btn-xs btn-primary btn-block'>Müşteri Detay</i> </a>" +
"<a href='MusteriYolu.aspx?id=" + musteri.musteri_id + "'><i class='btn btn-xs btn-info btn-block'>Navigasyon</i> </a>" +
    "</div>" +
"</div>" +

"</div>" +

" </div>");
            document.getElementById("txtResult").value += musteri.musteri_id + ",";

        }
    };

    map.fitBounds(bounds);

}
