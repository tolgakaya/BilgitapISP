﻿
var map = null;
var bounds = null;


var EarthRadiusMeters = 6378137.0; // meters
var panorama;

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

var musteri;
var anten;

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};
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

        panorama = new google.maps.StreetViewPanorama(
        document.getElementById('pano'), {
            position: latlng,
            pov: {
                heading: 34,
                pitch: 10
            }
        });
        map.setStreetView(panorama);
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

var must = {};
function GetMusteri() {
   
    //querystringden okunacak
    var id = getParameterByName('id');
    //var id = 74;
    var urlBase = "/api/Musteri/";
    var url = urlBase + "?id=" + id;

    var geocoder = new google.maps.Geocoder();
    //var id = $('#<%=txtSearch.ClientID %>').val();
    $.getJSON(url,
        function (data) {
            var musteri_id = data.musteri_id;
            var musteri_adi = data.musteri_adi;
            var center_Lat = data.center_Lat;
            var center_Long = data.center_Long;
            var musteri_adres = data.musteri_adres;
            var antenid = data.antenid;
            if (!center_Lat) {

                geocoder.geocode({ 'address': musteri_adres }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {

                        must = { center: new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng()), adi: musteri_adi, musteri_id: musteri_id, antenid: antenid };
                    } else {
                        console.log('Geocode error: ' + status);
                        //alert('Geocode error: ' + status);
                    }
                });


            }
            else {
                must = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id, antenid: antenid };

            }
        })
    .fail(
        function (jqXHR, textStatus, err) {

        });
    return must;
}

var ant = {};
function GetAnten() {

    var id = getParameterByName('antenid');
    //var id = 74;
    var urlBase = "/api/Anten/";
    var url = urlBase + "?id=" + id;
  
    $.ajax({
        type: "GET",
        url: "/api/Anten/" + id,
        contentType: "json",
        dataType: "json",
        success: function (data) {

            var anten_adi = data.anten_adi;
            var center_Lat = data.center_Lat;
            var center_Long = data.center_Long;
            var start_Lat = data.start_Lat;
            var start_Long = data.start_Long;
            var end_Lat = data.end_Lat;
            var end_Long = data.end_Long;

            var anten_id = data.anten_id;
            ant = { center: new google.maps.LatLng(center_Lat, center_Long), start: new google.maps.LatLng(start_Lat, start_Long), end: new google.maps.LatLng(end_Lat, end_Long), adi: anten_adi, anten_id: anten_id };


        },
        error: function (xhr) {
            console.log(xhr);
        }
    });

    return ant;
}



function initialize() {

    //anten = GetAnten();
    musteri = GetMusteri();


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

//    var centerPoint = anten.center;
//    var endPoint = anten.end;
//    var startPoint = anten.start;
//    var anten_id = anten.anten_id;


//    createMarker(centerPoint, "<div class='panel panel-primary'>" +

//"<div class='panel-heading'>" +
////adi + "/" +
//"</div>" +
//"<div class='panel-body'>" +
//"<div class='table-responsive'>" +
//"<div class='btn btn-xs-group'>" +
//"<a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint+ "));map.setZoom(20);'><i class='btn btn-xs btn-success '>Yaklaş</i></a> <a href='javascript:map.fitBounds(bounds);'><i class='btn btn-xs btn-danger'>Uzaklaş</i></a>" +
//"<a href='TekAnten.aspx?id=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Anten Müşterilerine Git</i> </a>" +

//"</div>" +
//"</div>" +

//"</div>" +

//" </div>");

//    var arcPts = drawArc(centerPoint, centerPoint.Bearing(startPoint), centerPoint.Bearing(endPoint), centerPoint.distanceFrom(startPoint));

//    arcPts.push(centerPoint);
//    bounds.extend(centerPoint);
//    arcPts.push(startPoint);

//    var piePoly = new google.maps.Polygon({
//        paths: [arcPts],
//        strokeColor: "#00FF00",
//        strokeOpacity: 0.5,
//        strokeWeight: 2,
//        fillColor: "#FF0000",
//        fillOpacity: 0.35,
//        map: map
//    });
    //poligonlar[centerPoint] = { center: centerPoint, poly: piePoly };
    //verilen bir noktanın hangi anten kapsamında olduğunu buluyoruz

    //for (var poli in poligonlar) {
    //if (poligonlar[poli].poly.containsLatLng(musteri.center)) {


    createMarkerM(musteri.center, "<div class='panel panel-primary'>" +

"<div class='panel-heading'>" +
musteri.adi + "/" +
"</div>" +
"<div class='panel-body'>" +
"<div class='table-responsive'>" +
       "<div class='btn btn-xs-group'>" +
"<a href='javascript:map.setCenter(new google.maps.LatLng(" + (musteri.center) + "));map.setZoom(20);'><i class='btn btn-xs btn-success '>Yaklaş</i></a> <a href='javascript:map.fitBounds(bounds);'><i class='btn btn-xs btn-danger'>Uzaklaş</i></a>" +
"<a href='../MusteriDetayBilgileri.aspx?custID=" + musteri.musteri_id + "'><i class='btn btn-xs btn-primary btn-block'>Müşteri Detay</i> </a>" +
"<a href='MusteriYolu.aspx?id=" + musteri.musteri_id + "'><i class='btn btn-xs btn-info btn-block'>Navigasyon</i> </a>" +
"</div>" +
"</div>" +

"</div>" +

" </div>");
    document.getElementById("txtResult").value += musteri.musteri_id + ",";

    //}
    //};

    map.fitBounds(bounds);

}
