

var piePoly;

//Display Coordinates below map
function getPolygonCoords() {
    var len = piePoly.getPath().getLength();
    var htmlStr = "";
    for (var i = 0; i < len; i++) {
        htmlStr += "new google.maps.LatLng(" + piePoly.getPath().getAt(i).toUrlValue(5) + "), ";
        //Use this one instead if you want to get rid of the wrap > new google.maps.LatLng(),
        //htmlStr += "" + myPolygon.getPath().getAt(i).toUrlValue(5);
    }
    document.getElementById('info').value = htmlStr;
}


function antenCizBaslangic(merkez, radius, aci) {

    //baslangıç tıklama noktası center
    //yarı çap verilen
    //açı verilen
    // A noktası her zaman 0 açıda
    // B noktası verilen açı
    var pointA = google.maps.geometry.spherical.computeOffset(merkez, radius, 0);
    var pointB = google.maps.geometry.spherical.computeOffset(merkez, radius, aci);

    var arcPts = drawArc(merkez, merkez.Bearing(pointA), merkez.Bearing(pointB), radius);
    // add the start and end lines
    arcPts.push(merkez);
    bounds.extend(merkez);
    arcPts.push(pointA);

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

    orta = google.maps.geometry.spherical.interpolate(pointA, pointB, 0.5);
    ortaCizgi = new google.maps.Polyline({
        path: [
            merkez,
            orta
        ],
        strokeColor: "#F00000",
        strokeOpacity: 1.0,
        strokeWeight: 10,
        map: map
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
        map.setStreetView(panorama);

    });

}

function antenCiz(merkez, radius, aci) {

    piePoly.setMap(null);
    //baslangıç tıklama noktası center
    //yarı çap verilen
    //açı verilen
    // A noktası her zaman 0 açıda
    // B noktası verilen açı
    var pointA = google.maps.geometry.spherical.computeOffset(merkez, radius, 0);
    var pointB = google.maps.geometry.spherical.computeOffset(merkez, radius, aci);

    var arcPts = drawArc(merkez, merkez.Bearing(pointA), merkez.Bearing(pointB), radius);
    // add the start and end lines
    arcPts.push(merkez);
    bounds.extend(merkez);
    arcPts.push(pointA);

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

    orta = google.maps.geometry.spherical.interpolate(pointA, pointB, 0.5);

    cizgi(merkez, orta);
    google.maps.event.addListener(ortaCizgi, 'click', function (event) {

        panorama = new google.maps.StreetViewPanorama(
            document.getElementById('pano'), {
                position: event.latLng,
                pov: {
                    heading: 34,
                    pitch: 10
                }
            });
        map.setStreetView(panorama);


    });

    piePoly.addListener('drag', showArrays);

    //tıklayıncada göstermesi gerekiyor
    //ancak tıklamada önce döndüğü için ters görünüyor
    piePoly.addListener('click', function (e) {
        var donmeAcisi = document.getElementById("txtDondur").value;
        if (donmeAcisi) {
            rotatePolygon(piePoly, donmeAcisi);
        }
        else {
            rotatePolygon(piePoly, 5);
        }


    });
    piePoly.addListener('click', showArrays);
}
var map = null;
var bounds = null;

var ortaCizgi;
var orta;
var panorama;

function showArrays(event) {
    // Since this polygon has only one path, we can call getPath() to return the
    // MVCArray of LatLngs.
    var vertices = this.getPath();

    var contentString = "";

    //'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
    //'<br>';
    //var m = vertices.getAt(33);
    //var a = vertices.getAt(32);
    //var b = vertices.getAt(1);
    var m, a, b;
    //contenString = m.lat() + ',' + m.lng() + '-' + a.lat() + ',' + a.lng() + '-' + b.lat() + ',' + b.lng();
    // Iterate over the vertices.
    ortaCizgi.setMap(null);
    for (var i = 0; i < vertices.getLength() ; i++) {
        if (i === 0 || i === 32 || i === 33) {
            var xy = vertices.getAt(i);
            contentString += xy.lat() + ',' + xy.lng() + '-';

            //var pos = new google.maps.LatLng(xy.lat(), xy.lng());
            //addMarker(pos);
        }
        if (i === 0) {
            var a = vertices.getAt(i);
        }
        else if (i === 32) {
            var b = vertices.getAt(i);
        }
        else if (i === 33) {
            var m = vertices.getAt(i);

        }

    }
    var o = google.maps.geometry.spherical.interpolate(a, b, 0.5);
    cizgi(m, o);
    google.maps.event.addListener(ortaCizgi, 'click', function (event) {

        panorama = new google.maps.StreetViewPanorama(
            document.getElementById('pano'), {
                position: event.latLng,
                pov: {
                    heading: 34,
                    pitch: 10
                }
            });

        map.setStreetView(panorama);

    });
    // Replace the info window's content and position.
    //infowindow.setContent(contentString);
    //infowindow.setPosition(event.latLng);

    //infowindow.open(map);
    document.getElementById('info').value = contentString;
}
function rotatePolygon(polygon, angle) {
    var map = polygon.getMap();
    var prj = map.getProjection();

    var origin = prj.fromLatLngToPoint(polygon.getPath().getAt(33)); //rotate around first point


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
function cizgi(merkez, sinir) {
    ortaCizgi.setMap(null);
    ortaCizgi = new google.maps.Polyline({
        path: [
            merkez,
            sinir
        ],
        strokeColor: "#F00000",
        strokeOpacity: 1.0,
        strokeWeight: 10,
        map: map
    });
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

function tekli(startPoint, endPoint, centerPoint, adi, anten_id) {

    var arcPts = drawArc(centerPoint, centerPoint.Bearing(startPoint), centerPoint.Bearing(endPoint), centerPoint.distanceFrom(startPoint));

    arcPts.push(centerPoint);
    bounds.extend(centerPoint);
    arcPts.push(startPoint);
    var txtAdi = document.getElementById("txtAntenAdi");
    txtAdi.value = adi;
    piePoly = new google.maps.Polygon({
        paths: [arcPts],
        strokeColor: "#00FF00",
        strokeOpacity: 0.5,
        strokeWeight: 2,
        fillColor: "#FF0000",
        fillOpacity: 0.35,
        draggable: true,
        map: map
    });

    orta = google.maps.geometry.spherical.interpolate(startPoint, endPoint, 0.5);
    ortaCizgi = new google.maps.Polyline({
        path: [
            centerPoint,
            orta
        ],
        strokeColor: "#F00000",
        strokeOpacity: 1.0,
        strokeWeight: 10,
        map: map
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
        map.setStreetView(panorama);

    });
};
function initializeEski() {
    var myOptions = {
        zoom: 10,
        center: new google.maps.LatLng(36.077286, 32.832889),
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map = new google.maps.Map(document.getElementById("map_canvas"),
                                  myOptions);

    bounds = new google.maps.LatLngBounds();

    panorama = new google.maps.StreetViewPanorama(
       document.getElementById('pano'), {
           position: new google.maps.LatLng(36.077286, 32.832889),
           pov: {
               heading: 34,
               pitch: 10
           }
       });

    map.setStreetView(panorama);

    if (anten) {
        tekli(anten.start, anten.end, anten.center, anten.adi, anten.anten_id);
    }
    else {
        antenCizBaslangic(new google.maps.LatLng(36.077286, 32.832889), 100, 90);
    }



    piePoly.addListener('drag', showArrays);

    //tıklayıncada göstermesi gerekiyor
    //ancak tıklamada önce döndüğü için ters görünüyor
    piePoly.addListener('click', function (e) {


        var donmeAcisi = document.getElementById("txtDondur").value;
        if (donmeAcisi) {
            rotatePolygon(piePoly, donmeAcisi);
        }
        else {
            rotatePolygon(piePoly, 5);
        }


    });
    piePoly.addListener('click', showArrays);

    google.maps.event.addListener(map, 'click', function (event) {
        var latitude = event.latLng.lat();
        var longitude = event.latLng.lng();

        var baslangic = new google.maps.LatLng(latitude, longitude);

        var r = document.getElementById("txtR").value;


        var ang = document.getElementById("txtAci").value;


        antenCiz(baslangic, r, ang);
    });
    //google.maps.event.addListener(ortaCizgi, 'click', function () {
    //    infowindow.close();
    //});


    map.fitBounds(bounds);

}

function mapBaslat() {
    var myOptions = {
        zoom: 10,
        center: new google.maps.LatLng(36.077286, 32.832889),
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
        navigationControl: true,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map = new google.maps.Map(document.getElementById("map_canvas"),
                                  myOptions);

    bounds = new google.maps.LatLngBounds();

    panorama = new google.maps.StreetViewPanorama(
       document.getElementById('pano'), {
           position: new google.maps.LatLng(36.077286, 32.832889),
           pov: {
               heading: 34,
               pitch: 10
           }
       });

    map.setStreetView(panorama);
}
function mapTamamla() {
    piePoly.addListener('drag', showArrays);

    piePoly.addListener('click', function (e) {


        var donmeAcisi = document.getElementById("txtDondur").value;
        if (donmeAcisi) {
            rotatePolygon(piePoly, donmeAcisi);
        }
        else {
            rotatePolygon(piePoly, 5);
        }


    });
    piePoly.addListener('click', showArrays);

    google.maps.event.addListener(map, 'click', function (event) {
        var latitude = event.latLng.lat();
        var longitude = event.latLng.lng();

        var baslangic = new google.maps.LatLng(latitude, longitude);

        var r = document.getElementById("txtR").value;


        var ang = document.getElementById("txtAci").value;


        antenCiz(baslangic, r, ang);
    });

    map.fitBounds(bounds);
}
function initialize() {
    //parametre id varsa kayıtlı anteni gösterecek
    //yoksa yeni aten çizilecek
    var id = getParameterByName('id');
    var urlBase = "/api/Anten/";
    var url = urlBase + "?id=" + id;
    if (id) {

        $.getJSON(url,
      function (data) {
          mapBaslat();
          var anten_adi = data.anten_adi;
          var center_Lat = data.center_Lat;
          var center_Long = data.center_Long;
          var start_Lat = data.start_Lat;
          var start_Long = data.start_Long;
          var end_Lat = data.end_Lat;
          var end_Long = data.end_Long;

          anten = { center: new google.maps.LatLng(center_Lat, center_Long), start: new google.maps.LatLng(start_Lat, start_Long), end: new google.maps.LatLng(end_Lat, end_Long), adi: anten_adi };

          //blat,blng-
          var antenString = start_Lat + ',' + start_Long + '-' + end_Lat + ',' + end_Long + '-' + center_Lat + ',' + center_Long;
          document.getElementById('info').value = antenString;
          tekli(anten.start, anten.end, anten.center, anten.adi, anten.anten_id);

          mapTamamla();
      })
  .fail(
      function (jqXHR, textStatus, err) {

      });
    }
    else {

        mapBaslat();
        //baş tarafını unutma
        antenCizBaslangic(new google.maps.LatLng(36.077286, 32.832889), 100, 90);
        mapTamamla();
    }


}

