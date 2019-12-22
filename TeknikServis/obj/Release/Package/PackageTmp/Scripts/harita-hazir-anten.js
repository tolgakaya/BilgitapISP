﻿//query stringden anten idsi ile gelecek ve görüntüleme yapılacak.
//asp sayfasında tıklama yapıldığı zaman müşteri listesine gönderilecek

var EarthRadiusMeters = 6378137.0; // meters
/* Based the on the Latitude/longitude spherical geodesy formulae & scripts
   at http://www.movable-type.co.uk/scripts/latlong.html
   (c) Chris Veness 2002-2010
*/
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

// === A function which returns the bearing between two LatLng in radians ===
// === If v1 is null, it returns the bearing between the first and last vertex ===
// === If v1 is present but v2 is null, returns the bearing from v1 to the next vertex ===
// === If either vertex is out of range, returns void ===
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


/**
 * Extend the Number object to convert degrees to radians
 *
 * @return {Number} Bearing in radians
 * @ignore
 */
Number.prototype.toRad = function () {
    return this * Math.PI / 180;
};

/**
 * Extend the Number object to convert radians to degrees
 *
 * @return {Number} Bearing in degrees
 * @ignore
 */
Number.prototype.toDeg = function () {
    return this * 180 / Math.PI;
};

/**
 * Normalize a heading in degrees to between 0 and +360
 *
 * @return {Number} Return
 * @ignore
 */
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
    //buradaki 1. 2. ve 30. nokta bizim antenin üç noktası olabilir mi
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



var id = getParameterByName('id');
//var id = 2;
var urlBase = "/api/Anten/";
var url = urlBase + "?id=" + id;
var tekAn;
var anten;

function GetAnten() {

    //var id = $('#<%=txtSearch.ClientID %>').val();
    $.getJSON(url,
        function (data) {
            var anten_adi = data.anten_adi;
            var center_Lat = data.center_Lat;
            var center_Long = data.center_Long;
            var start_Lat = data.start_Lat;
            var start_Long = data.start_Long;
            var end_Lat = data.end_Lat;
            var end_Long = data.end_Long;

            var anten_id = data.anten_id;
            anten = { center: new google.maps.LatLng(center_Lat, center_Long), start: new google.maps.LatLng(start_Lat, start_Long), end: new google.maps.LatLng(end_Lat, end_Long), adi: anten_adi, anten_id: anten_id };


        })
    .fail(
        function (jqXHR, textStatus, err) {

        });
    return anten;
}
var piePoly;

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

//Display Coordinates below map
function getPolygonCoords() {
    var len = piePoly.getPath().getLength();
    var htmlStr = "";
    for (var i = 0; i < len; i++) {
        htmlStr += "new google.maps.LatLng(" + piePoly.getPath().getAt(i).toUrlValue(5) + "), ";
        //Use this one instead if you want to get rid of the wrap > new google.maps.LatLng(),
        //htmlStr += "" + myPolygon.getPath().getAt(i).toUrlValue(5);
    }
    document.getElementById('info').innerHTML = htmlStr;
}
function tekli(startPoint, endPoint, centerPoint, adi, anten_id) {
    //   createMarker(centerPoint, "Anten: " + adi + " Yönetici: " + bayi +
    //    "<br><a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint.toUrlValue(6) + "));map.setZoom(20);'>Yaklaş</a> - <a href='javascript:map.fitBounds(bounds);'>Uzaklaş</a>" +
    //"<br><a href='TekAnten.aspx?id=" + anten_id + "'>Anten müşterilerine git </a>");

    createMarker(centerPoint, "<div class='panel panel-primary'>" +

"<div class='panel-heading'>" +
adi + "/" +
"</div>" +
"<div class='panel-body'>" +
"<div class='table-responsive'>" +
"<div class='btn btn-xs-group'>" +
"<a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint.toUrlValue(6) + "));map.setZoom(20);'><i class='btn btn-xs btn-success '>Yaklaş</i></a> <a href='javascript:map.fitBounds(bounds);'><i class='btn btn-xs btn-danger'>Uzaklaş</i></a>" +
"<a href='TekAnten.aspx?id=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Kapsam İçerisindeki Müşteri Haritası</i> </a>" +
"<a href='MusteriAnten.aspx?antenid=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Antene Kayıtlı Müşteri Listesi</i> </a>" +
"</div>" +
"</div>" +

"</div>" +

" </div>");

    //createMarker(centerPoint, "Anten: " + adi + "<br><a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint.toUrlValue(6) + "));map.setZoom(20);'>Yaklaş</a> - <a href='javascript:map.fitBounds(bounds);'>Uzaklaş</a>");
    var arcPts = drawArc(centerPoint, centerPoint.Bearing(startPoint), centerPoint.Bearing(endPoint), centerPoint.distanceFrom(startPoint));
    // add the start and end lines
    arcPts.push(centerPoint);
    bounds.extend(centerPoint);
    arcPts.push(startPoint);

    piePoly = new google.maps.Polygon({
        paths: [arcPts],
        strokeColor: "#00FF00",
        strokeOpacity: 0.5,
        strokeWeight: 2,
        fillColor: "#FF0000",
        fillOpacity: 0.35,
        draggable: true,
        //editable: true,
        map: map
    });

  


};


var map = null;
var bounds = null;



anten = GetAnten();

function rotatePolygon(polygon, angle) {
    var map = polygon.getMap();
    var prj = map.getProjection();
    var origin = prj.fromLatLngToPoint(polygon.getPath().getAt(0)); //rotate around first point

    var coords = polygon.getPath().getArray().map(function (latLng) {
        var point = prj.fromLatLngToPoint(latLng);
        var rotatedLatLng = prj.fromPointToLatLng(rotatePoint(point, origin, angle));
        return { lat: rotatedLatLng.lat(), lng: rotatedLatLng.lng() };
    });
    polygon.setPath(coords);
}
function rotatePolygon(polygon, angle,origin) {
    var map = polygon.getMap();
    var prj = map.getProjection();
    var origin = prj.fromLatLngToPoint(polygon.getPath().getAt(0)); //rotate around first point

    var coords = polygon.getPath().getArray().map(function (latLng) {
        var point = prj.fromLatLngToPoint(latLng);
        var rotatedLatLng = prj.fromPointToLatLng(rotatePoint(point, origin, angle));
        return { lat: rotatedLatLng.lat(), lng: rotatedLatLng.lng() };
    });
    polygon.setPath(coords);
}
function rotatePoint(point, origin, angle) {
    var angleRad = angle * Math.PI / 180.0;
    return {
        x: Math.cos(angleRad) * (point.x - origin.x) - Math.sin(angleRad) * (point.y - origin.y) + origin.x,
        y: Math.sin(angleRad) * (point.x - origin.x) + Math.cos(angleRad) * (point.y - origin.y) + origin.y
    };
}
function initialize() {
    var myOptions = {
        zoom: 10,
        center: new google.maps.LatLng(36.077806, 32.826344),
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

   

    tekli(anten.start, anten.end, anten.center, anten.adi, anten.anten_id);

    piePoly.addListener('click', function (e) {
        rotatePolygon(piePoly, 5,anten.center);
    });

    //google.maps.event.addListener(myPolygon, "dragend", getPolygonCoords);
    google.maps.event.addListener(piePoly.getPath(), "insert_at", getPolygonCoords);
    //google.maps.event.addListener(myPolygon.getPath(), "remove_at", getPolygonCoords);
    google.maps.event.addListener(piePoly.getPath(), "set_at", getPolygonCoords);
    //verilen bir anten kapsamında hangi müşterilerin olduğunu bulacağız

    map.fitBounds(bounds);

}

