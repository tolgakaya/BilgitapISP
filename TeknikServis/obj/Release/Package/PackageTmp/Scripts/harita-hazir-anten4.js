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



function drawArc(center, initialBearing, finalBearing, radius) {
    var d2r = Math.PI / 180;   // degrees to radians
    var r2d = 180 / Math.PI;   // radians to degrees

    var points = 32;
    //buradaki 1. 2. ve 30. nokta bizim antenin üç noktası olabilir mi
    // find the raidus in lat/lon
    // find the raidus in lat/lon
    var rlat = (radius / EarthRadiusMeters) * r2d;
    var rlng = rlat / Math.cos(center.lat() * d2r);



    //var pointB = google.maps.geometry.spherical.computeOffset(center, radius, 90);

    //var pointC = google.maps.geometry.spherical.computeOffset(center, radius, 50);
    //markerEkle(pointB);
    //markerEkle(pointC);

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
    document.getElementById('info').innerHTML = htmlStr;
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
    //orta = google.maps.geometry.spherical.interpolate(pointA, pointB, 0.5);

    //cizgi(merkez, orta);
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
        rotatePolygon(piePoly, 5);

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
    document.getElementById('info').innerHTML = contentString;
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

//görüntü eklenecek
//ilk tıklamayla oluşturulan antende merkez noktasının görüntüsü
//ve çizgiye tıklandığı zaman görüntü eklenecek

function initialize() {
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

    antenCizBaslangic(new google.maps.LatLng(36.077286, 32.832889), 100, 90);



    piePoly.addListener('drag', showArrays);

    //tıklayıncada göstermesi gerekiyor
    //ancak tıklamada önce döndüğü için ters görünüyor
    piePoly.addListener('click', function (e) {
        rotatePolygon(piePoly, 5);

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

