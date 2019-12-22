

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
var labelMesafe;
var labelAci;
var labelPozisyonA;
var labelPozisyonC;
var labelPozisyonB;
var metre;
var birinciBearing;
var ikinciBearing;

function createIcons() {
    icons = Array();
    var url;
    for (var n = 65; n < 68; n++) {
        var icon = new google.maps.MarkerImage('http://www.google.com/intl/en_ALL/mapfiles/marker_green' + String.fromCharCode(n) + '.png');
        icons.push(icon);
    }
}

function doLoad() {
    mapCanvas = new google.maps.Map(document.getElementById("map_canvas"), {
        center: centerPoint,
        zoom: zoom,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

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

    labelMesafe = document.createElement("div");
    labelMesafe.style.padding = 1;
    labelMesafe.style.backgroundColor = "#EEE";
    labelMesafe.style.borderColor = "#aaa";
    labelMesafe.style.fontSize = "1.5em";

    labelAci = document.createElement("div");
    labelAci.id = "labelAci";
    labelAci.style.padding = 1;
    labelAci.style.backgroundColor = "#EEE";
    labelAci.style.borderColor = "#aaa";
    labelAci.style.fontSize = "1.5em";

    //merkez marker
    labelPozisyonA = document.createElement("div");
    labelPozisyonA.id = "labelPozisyonA";
    labelPozisyonA.style.padding = 1;
    labelPozisyonA.style.backgroundColor = "#EEE";
    labelPozisyonA.style.borderColor = "#aaa";
    labelPozisyonA.style.fontSize = "1.5em";

    //başlangıç marker
    labelPozisyonC = document.createElement("div");
    labelPozisyonC.id = "labelPozisyonC";
    labelPozisyonC.style.padding = 1;
    labelPozisyonC.style.backgroundColor = "#EEE";
    labelPozisyonC.style.borderColor = "#aaa";
    labelPozisyonC.style.fontSize = "1.5em";

    //bitiş marker
    labelPozisyonB = document.createElement("div");
    labelPozisyonB.id = "labelPozisyonB";
    labelPozisyonB.style.padding = 1;
    labelPozisyonB.style.backgroundColor = "#EEE";
    labelPozisyonB.style.borderColor = "#aaa";
    labelPozisyonB.style.fontSize = "1.5em";

    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(bearingLabelDiv);
    mapCanvas.controls[google.maps.ControlPosition.LEFT_BOTTOM].push(bearingLabelDiv2);
    mapCanvas.controls[google.maps.ControlPosition.LEFT_BOTTOM].push(labelMesafe);
    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(labelAci);

    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(labelPozisyonA);
    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(labelPozisyonC);
    mapCanvas.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(labelPozisyonB);
}

function drawCircle(center, radius, bearing) {
    //iki tane çember çizerek aradaki açıyı belirteceksin :)
    if (!circle) {
        circle = new google.maps.Polygon({
            strokeColor: '#000000',
            strokeWeight: 2,
            strokeOpacity: 1,
            fillColor: '#000000',
            fillColor: 0.15,
            map: mapCanvas
        });
    }

    var circlePoints = Array();

    // radians
    var d = radius / 3963.189;

    // radians
    var lat1 = (Math.PI / 180) * center.lat();
    // radians
    var lng1 = (Math.PI / 180) * center.lng();

    for (var a = bearing - 180; a < bearing + 1; a++) {
        var tc = (Math.PI / 180) * a;
        var y = Math.asin(Math.sin(lat1) * Math.cos(d) + Math.cos(lat1) * Math.sin(d) * Math.cos(tc));
        var dlng = Math.atan2(Math.sin(tc) * Math.sin(d) * Math.cos(lat1), Math.cos(d) - Math.sin(lat1) * Math.sin(y));
        var x = ((lng1 - dlng + Math.PI) % (2 * Math.PI)) - Math.PI;
        // MOD function
        var point = new google.maps.LatLng(parseFloat(y * (180 / Math.PI), 10), parseFloat(x * (180 / Math.PI), 10));
        circlePoints.push(point);
    }

    circlePoints.push(circlePoints[0]);
    if (d < 1.5678565720686044) {
        circle.setPath(circlePoints);
    } else {
        circle.setPath(circlePoints);
    }
}
function drawCircle2(center, radius, bearing) {
    //iki tane çember çizerek aradaki açıyı belirteceksin :)
    if (!circle2) {
        circle2 = new google.maps.Polygon({
            strokeColor: '#000000',
            strokeWeight: 2,
            strokeOpacity: 1,
            fillColor: '#000000',
            fillColor: 0.15,
            map: mapCanvas
        });
    }

    var circlePoints = Array();

    // radians
    var d = radius / 3963.189;

    // radians
    var lat1 = (Math.PI / 180) * center.lat();
    // radians
    var lng1 = (Math.PI / 180) * center.lng();

    for (var a = bearing - 180; a < bearing + 1; a++) {
        var tc = (Math.PI / 180) * a;
        var y = Math.asin(Math.sin(lat1) * Math.cos(d) + Math.cos(lat1) * Math.sin(d) * Math.cos(tc));
        var dlng = Math.atan2(Math.sin(tc) * Math.sin(d) * Math.cos(lat1), Math.cos(d) - Math.sin(lat1) * Math.sin(y));
        var x = ((lng1 - dlng + Math.PI) % (2 * Math.PI)) - Math.PI;
        // MOD function
        var point = new google.maps.LatLng(parseFloat(y * (180 / Math.PI), 10), parseFloat(x * (180 / Math.PI), 10));
        circlePoints.push(point);
    }

    circlePoints.push(circlePoints[0]);
    if (d < 1.5678565720686044) {
        circle2.setPath(circlePoints);
    } else {
        circle2.setPath(circlePoints);
    }
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
    labelAci.innerHTML = (birinciBearing - ikinciBearing);
    labelPozisyonB.innerHTML = p2;
    labelPozisyonA.innerHTML = p1;
    distance = google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1609;
    //drawCircle(marker1.getPosition(), distance, bearing);
    metre = 1609.3 * distance;
    labelMesafe.innerHTML = metre;

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
    labelAci.innerHTML = (birinciBearing - ikinciBearing);
    labelPozisyonC.innerHTML = p2;
    labelPozisyonA.innerHTML = p1;
    distance = google.maps.geometry.spherical.computeDistanceBetween(p1, p2) / 1609;
    
    metre = 1609.3 * distance;
    labelMesafe.innerHTML = metre;
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
        infoWnd.open(map, marker);
    });
    google.maps.event.addListener(marker, 'drag', function () {

        calculateBearing();
        calculateBearing2();

    });
    return marker;
}


//burada başlangıç noktası yerleştiriliyor
function load() {
    createIcons();
    doLoad();
    marker1 = setMarker(marker1, new google.maps.LatLng(36.077286, 32.832889), 'Center', 'Center', icons[0]);
    calculatePoint();
    calculatePoint2();
    var buton = document.getElementById('btnGoster');
    google.maps.event.addListener(buton, 'click', function () {

        goster();
    });
}
function goster() {
    var acimiz = labelAci.innerHTML; //document.getElementById("labelAci").innerHTML;
    var pozisyonA = labelPozisyonA.innerHTML;
    var pozisyonB = labelPozisyonB.innerHTML;
    var pozisyonC = labelPozisyonC.innerHTML;
    if (acimiz < 0) {
        alert("Negatif açı seçtiniz. Pozitif açı için B ve C ikonlarını değiştiriniz.");
    }
    else {
        alert(pozisyonA + pozisyonB + pozisyonC + acimiz);
    }
    var sonuc = document.getElementById("txtResult");
    sonuc.value = pozisyonA;
}

google.maps.event.addDomListener(window, "load", load);

 