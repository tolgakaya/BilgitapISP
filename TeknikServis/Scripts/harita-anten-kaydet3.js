

var mapCanvas;
var container;
var zoom = 15;
var centerPoint = new google.maps.LatLng(36.077286, 32.832889);

var marker1, marker2, marker3;
var circle, line;
var circle2, line2;
var icons = Array();
var infoWnd = new google.maps.InfoWindow();
var bearingLabelDiv;
var bearingLabelDiv2;
var labelmesafeC;
var labelmesafeB;
var labelAci;
var txtAci;
var labelPozisyonA;
var labelPozisyonC;
var labelPozisyonB;
var metre;
var metreB;
var birinciBearing;
var ikinciBearing;
var bounds = null;

//query stringden anten idsi ile gelecek ve görüntüleme yapılacak.
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

function createIcons() {
    icons = Array();
    var url;
    for (var n = 65; n < 68; n++) {
        var icon = new google.maps.MarkerImage('http://www.google.com/intl/en_ALL/mapfiles/marker_green' + String.fromCharCode(n) + '.png');
        icons.push(icon);
    }
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

function doLoad() {

    var baslama = new google.maps.LatLng(36.077806, 32.826344);
    var myOptions = {
        zoom: zoom,
        center: baslama,
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.satellite
    }
    mapCanvas = new google.maps.Map(document.getElementById("map_canvas"),
                                  myOptions);
    //sadece başlangıçta yüklensin sonra silinsin diye
    //orta = google.maps.geometry.spherical.interpolate(startPoint, endPoint, 0.5);

    ortaCizgi = new google.maps.Polyline({
        path: [
            baslama,
            baslama
        ],
        strokeColor: "#F00000",
        strokeOpacity: 1.0,
        strokeWeight: 10,
        map: mapCanvas
    });
    ucgen = new google.maps.Polyline({
        path: [
            baslama,
            baslama
        ],
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 10,
        map: mapCanvas
    });

    //sadece başlangıçta yüklensin sonra silinsin diye

    bounds = new google.maps.LatLngBounds();

    bearingLabelDiv = document.createElement("div");

    bearingLabelDiv.style.padding = 1;
    bearingLabelDiv.style.backgroundColor = "#EEE";
    bearingLabelDiv.style.borderColor = "#aaa";
    bearingLabelDiv.style.fontSize = "1.5em";

    bearingLabelDiv2 = document.createElement("div");
    bearingLabelDiv2.style.padding = 1;
    bearingLabelDiv2.style.backgroundColor = "#EEE";
    bearingLabelDiv2.style.borderColor = "#aaa";
    bearingLabelDiv2.style.fontSize = "1.5em";

    labelmesafeC = document.createElement("div");
    labelmesafeC.style.padding = 1;
    labelmesafeC.style.backgroundcolor = "#EEE";
    labelmesafeC.style.borderColor = "#aaa";
    labelmesafeC.style.fontSize = "1.5em";

    labelmesafeB = document.createElement("div");
    labelmesafeB.style.padding = 1;
    labelmesafeB.style.backgroundcolor = "#EEE";
    labelmesafeB.style.borderColor = "#aaa";
    labelmesafeB.style.fontSize = "1.5em";

    labelAci = document.createElement("div");
    labelAci.id = "labelAci";
    labelAci.style.padding = 1;
    labelAci.style.backgroundColor = "#EEE";
    labelAci.style.borderColor = "#aaa";
    labelAci.style.fontSize = "1.5em";

    txtAci = document.getElementById("txtAci");
    ////merkez marker
    labelPozisyonA = document.getElementById("labelPozisyonA");

    ////başlangıç marker
    labelPozisyonC = document.getElementById("labelPozisyonC");


    ////bitiş marker
    labelPozisyonB = document.getElementById("labelPozisyonB");


    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(labelmesafeC);
    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(labelmesafeB);
    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(labelAci);

    //mapCanvas.fitBounds(bounds);

}


function calculateBearing() {
    var p1 = marker1.getPosition();
    var p2 = marker2.getPosition();

    var lat1 = p1.lat() * (Math.PI / 180);
    var lon1 = p1.lng() * (Math.PI / 180);
    var lat2 = p2.lat() * (Math.PI / 180);
    var lon2 = p2.lng() * (Math.PI / 180);

    var d = 2 * Math.asin(Math.sqrt(Math.pow((Math.sin((lat1 - lat2) / 2)), 2) + Math.cos(lat1) * Math.cos(lat2) * Math.pow((Math.sin((lon1 - lon2) / 2)), 2)));
    var bearing = Math.atan2(Math.sin(lon1 - lon2) * Math.cos(lat2), Math.cos(lat1) * Math.sin(lat2) - Math.sin(lat1) * Math.cos(lat2) * Math.cos(lon1 - lon2)) / -(Math.PI / 180);
    bearing = bearing < 0 ? 360 + bearing : bearing;

    // 0 degrees at 3 o'clock and counting couterclockwise
    var bearing = 360 - bearing + 90;

    bearingLabelDiv.innerHTML = 'Bearing: ' + bearing % 360;
    birinciBearing = bearing % 360;
    labelAci.innerHTML = "Anten Açısı: " + Math.round(birinciBearing - ikinciBearing);
    txtAci.value = Math.round(birinciBearing - ikinciBearing);
    labelPozisyonB.value = p2;
    labelPozisyonA.value = p1;
    //var sonuc = document.getElementById("txtResult");
    //sonuc.value = p1;
    distance = google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1609;
    //drawCircle(marker1.getPosition(), distance, bearing);
    metreB = 1609.3 * distance;
    labelmesafeB.innerHTML = "Mesafe B :" + Math.round(metreB) + " metre";

    if (!line) {
        line = new google.maps.Polyline({
            strokeColor: '#0000ff',
            strokeWeight: 2,
            strokeOpacity: 1,
            map: mapCanvas
        });
    }
    line.setPath([marker1.getPosition(), marker2.getPosition()]);

}
function calculateBearing2() {
    var p1 = marker1.getPosition();
    var p2 = marker3.getPosition();

    var lat1 = p1.lat() * (Math.PI / 180);
    var lon1 = p1.lng() * (Math.PI / 180);
    var lat2 = p2.lat() * (Math.PI / 180);
    var lon2 = p2.lng() * (Math.PI / 180);

    var d = 2 * Math.asin(Math.sqrt(Math.pow((Math.sin((lat1 - lat2) / 2)), 2) + Math.cos(lat1) * Math.cos(lat2) * Math.pow((Math.sin((lon1 - lon2) / 2)), 2)));
    var bearing = Math.atan2(Math.sin(lon1 - lon2) * Math.cos(lat2), Math.cos(lat1) * Math.sin(lat2) - Math.sin(lat1) * Math.cos(lat2) * Math.cos(lon1 - lon2)) / -(Math.PI / 180);
    bearing = bearing < 0 ? 360 + bearing : bearing;

    // 0 degrees at 3 o'clock and counting couterclockwise
    var bearing = 360 - bearing + 90;

    bearingLabelDiv2.innerHTML = 'Bearing: ' + bearing % 360;

    ikinciBearing = bearing % 360;
    labelAci.innerHTML = "Anten Açısı: " + Math.round(birinciBearing - ikinciBearing);
    txtAci.value = Math.round(birinciBearing - ikinciBearing);
    labelPozisyonC.value = p2;
    labelPozisyonA.value = p1;
    distance = google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1609;

    metre = 1609.3 * distance;
    labelmesafeC.innerHTML = "Mesafe C:" + Math.round(metre) + " metre";
    //drawCircle2(marker1.getPosition(), distance, bearing);

    if (!line2) {
        line2 = new google.maps.Polyline({
            strokeColor: '#0000ff',
            strokeWeight: 2,
            strokeOpacity: 1,
            map: mapCanvas
        });
    }
    line2.setPath([marker1.getPosition(), marker3.getPosition()]);
}

function calculatePoint() {
    var startLat = marker1.getPosition().lat();
    var startLon = marker1.getPosition().lng();

    //in miles.
    var distance = 1;

    // 0 degrees at 3 o'clock and counting couterclockwise, so 90 is due north)
    var bearing = 80;

    var radiansLat = (distance / 3963.189) * (180 / Math.PI);
    var radiansLng = radiansLat / Math.cos(startLat * (Math.PI / 180));
    var radiansBearing = bearing * (Math.PI / 180);

    var destLon = startLon + (radiansLng * Math.cos(radiansBearing));
    var destLat = startLat + (radiansLat * Math.sin(radiansBearing));
    var destPoint = new google.maps.LatLng(parseFloat(destLat, 10), parseFloat(destLon, 10));

    marker2 = setMarker(marker2, destPoint, 'Lat: ' + destPoint.lat() + '<br>Lon: ' + destPoint.lng(), 'Destination', icons[1]);

    calculateBearing();
}
function calculatePointB(nokta) {
    //var startLat = marker1.getPosition().lat();
    //var startLon = marker1.getPosition().lng();

    ////in miles.
    //var distance = 1;

    //// 0 degrees at 3 o'clock and counting couterclockwise, so 90 is due north)
    //var bearing = 80;

    //var radiansLat = (distance / 3963.189) * (180 / Math.PI);
    //var radiansLng = radiansLat / Math.cos(startLat * (Math.PI / 180));
    //var radiansBearing = bearing * (Math.PI / 180);

    //var destLon = startLon + (radiansLng * Math.cos(radiansBearing));
    //var destLat = startLat + (radiansLat * Math.sin(radiansBearing));
    //var destPoint = new google.maps.LatLng(parseFloat(destLat, 10), parseFloat(destLon, 10));

    marker2 = setMarker(marker2, nokta.start, 'Başlangıç', 'Başlangıç', icons[1]);
    calculateBearing();
}

function calculatePoint2() {
    var startLat = marker1.getPosition().lat();
    var startLon = marker1.getPosition().lng();

    //in miles.
    var distance = 1;

    // 0 degrees at 3 o'clock and counting couterclockwise, so 90 is due north)
    var bearing = 30;

    var radiansLat = (distance / 3963.189) * (180 / Math.PI);
    var radiansLng = radiansLat / Math.cos(startLat * (Math.PI / 180));
    var radiansBearing = bearing * (Math.PI / 180);

    var destLon = startLon + (radiansLng * Math.cos(radiansBearing));
    var destLat = startLat + (radiansLat * Math.sin(radiansBearing));
    var destPoint = new google.maps.LatLng(parseFloat(destLat, 10), parseFloat(destLon, 10));

    marker3 = setMarker(marker3, destPoint, 'Lat: ' + destPoint.lat() + '<br>Lon: ' + destPoint.lng(), 'Destination', icons[2]);

    calculateBearing2();

}
function calculatePoint2C(nokta) {
    //var startLat = marker1.getPosition().lat();
    //var startLon = marker1.getPosition().lng();

    ////in miles.
    //var distance = 1;

    //// 0 degrees at 3 o'clock and counting couterclockwise, so 90 is due north)
    //var bearing = 30;

    //var radiansLat = (distance / 3963.189) * (180 / Math.PI);
    //var radiansLng = radiansLat / Math.cos(startLat * (Math.PI / 180));
    //var radiansBearing = bearing * (Math.PI / 180);

    //var destLon = startLon + (radiansLng * Math.cos(radiansBearing));
    //var destLat = startLat + (radiansLat * Math.sin(radiansBearing));
    var destPoint = nokta.end;

    marker3 = setMarker(marker3, destPoint, 'Lat: ' + destPoint.lat() + '<br>Lon: ' + destPoint.lng(), 'Destination', icons[2]);

    calculateBearing2();

    //marker3 = setMarker(marker3, nokta.end, 'Bitiş', 'Bitiş', icons[2]);


}

function setMarker(marker, point, html, title, icon) {
    if (marker) {
        marker.setPosition(point);
        return marker;
    }

    marker = new google.maps.Marker({
        position: point,
        title: title,
        icon: icon,
        draggable: true,
        map: mapCanvas
    });
    google.maps.event.addListener(marker, 'click', function () {
        infoWnd.setContent(html);
        infoWnd.open(mapCanvas, marker);

        panorama = new google.maps.StreetViewPanorama(
               document.getElementById('pano'), {
                   position: point,
                   pov: {
                       heading: 34,
                       pitch: 10
                   }
               });
    });
    google.maps.event.addListener(marker, 'drag', function () {

        calculateBearing();
        calculateBearing2();
        piePoly.setMap(null);

        cekim();

    });
    return marker;
}


//burada başlangıç noktası yerleştiriliyor

var id = getParameterByName('id');
//var id = 2;
var urlBase = "/api/Anten/";
var url = urlBase + "?id=" + id;

var nokta = GetAnten();
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

            nokta = { center: new google.maps.LatLng(center_Lat, center_Long), start: new google.maps.LatLng(start_Lat, start_Long), end: new google.maps.LatLng(end_Lat, end_Long), adi: anten_adi };
        })
    .fail(
        function (jqXHR, textStatus, err) {

        });
    return nokta;
}
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

function load() {
    createIcons();
    doLoad();

    //query stringde id ile gelince düzenleme yapılıyor
    //query string boş ise yeni akayıt yapıyor
    if (nokta != null) {

        marker1 = setMarker(marker1, nokta.center, 'Center', 'Center', icons[0]);
        //marker2 = setMarker(marker2, nokta.start, 'Başlangıç', 'Başlangıç', icons[1]);
        //marker3 = setMarker(marker3, nokta.end, 'Bitiş', 'Bitiş', icons[2]);
        var adimiz = document.getElementById("txtAntenAdi");
        adimiz.value = nokta.adi;
        panorama = new google.maps.StreetViewPanorama(

            document.getElementById('pano'), {
                position: nokta,
                pov: {
                    heading: 34,
                    pitch: 10
                }
            });
        mapCanvas.setStreetView(panorama);
        //calculatePointB(nokta);
        //calculatePoint2C(nokta);
        calculatePointB(nokta);
        calculatePoint2C(nokta);
    }
    else {
        marker1 = setMarker(marker1, new google.maps.LatLng(36.077286, 32.832889), 'Center', 'Center', icons[0]);

        var oylesine = new google.maps.LatLng(36.077286, 32.832889);

        panorama = new google.maps.StreetViewPanorama(
        document.getElementById('pano'), {
           position: oylesine,
           pov: {
               heading: 34,
               pitch: 10
           }
       });
        mapCanvas.setStreetView(panorama);
        calculatePoint();
        calculatePoint2();
    }



    cekim();

    google.maps.event.addListener(ortaCizgi, 'click', function (event) {

        panorama = new google.maps.StreetViewPanorama(
            document.getElementById('pano'), {
                position: event.latLng,
                pov: {
                    heading: 34,
                    pitch: 10
                }
            });

    });

   
}

var piePoly;
var ortaCizgi;
var panorama;
var orta;
var ucgen;
function cekim() {

    var centerPoint = marker1.getPosition();
    var startPoint = marker2.getPosition();
    var endPoint = marker3.getPosition();

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
        map: mapCanvas
    });

    ortaCizgi.setMap(null);
    ucgen.setMap(null);
    //panorama.setMap(null);
    orta = google.maps.geometry.spherical.interpolate(startPoint, endPoint, 0.5);

    ortaCizgi = new google.maps.Polyline({
        path: [
            centerPoint,
            orta
        ],
        strokeColor: "#F00000",
        strokeOpacity: 1.0,
        strokeWeight: 10,
        map: mapCanvas
    });
    google.maps.event.addListener(ortaCizgi, 'click', function (event) {

        panorama = new google.maps.StreetViewPanorama(
            document.getElementById('pano'), {
                position: event.latLng,
                pov: {
                    heading: 34,
                    pitch: 10
                }
            });

    });
    ucgen = new google.maps.Polyline({
        path: [
            startPoint,
            endPoint
        ],
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 10,
        map: mapCanvas
    });
    //panorama = new google.maps.StreetViewPanorama(
    //       document.getElementById('pano'), {
    //           position: centerPoint,
    //           pov: {
    //               heading: 34,
    //               pitch: 10
    //           }
    //       });
    //mapCanvas.setStreetView(panorama);

}
function goster() {
    var acimiz = labelAci.innerHTML; //document.getElementById("labelAci").innerHTML;
    var pozisyonA = labelPozisyonA.value;
    var pozisyonB = labelPozisyonB.value;
    var pozisyonC = labelPozisyonC.value;
    if (acimiz < 0) {
        alert("Negatif açı seçtiniz. Pozitif açı için B ve C ikonlarını değiştiriniz.");
    }
    else {
        alert(pozisyonA + pozisyonB + pozisyonC + acimiz);
    }

}

google.maps.event.addDomListener(window, "load", load);

