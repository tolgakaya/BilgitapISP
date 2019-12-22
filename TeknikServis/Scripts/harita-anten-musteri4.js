


var map = null;
var bounds = null;
var panorama;





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

function createMarkerM(latlng, html) {
    var info = new google.maps.InfoWindow(
 {
     size: new google.maps.Size(60, 10)
 });

    var contentString = html;
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        icon: '/img/musteri.png',
        animation: google.maps.Animation.DROP,
        zIndex: Math.round(latlng.lat() * -100000) << 5
    });
    bounds.extend(latlng);
    info.setContent(contentString);
    info.open(map, marker);
    //google.maps.event.addListener(marker, 'click', function () {
    //    infowindow.setContent(contentString);
    //    infowindow.open(map, marker);

    //    panorama = new google.maps.StreetViewPanorama(
    //    document.getElementById('pano'), {
    //        position: latlng,
    //        pov: {
    //            heading: 34,
    //            pitch: 10
    //        }
    //    });
    //    map.setStreetView(panorama);
    //});
}


function tekli(startPoint, endPoint, centerPoint, adi, anten_id) {

    createMarker(centerPoint, "<div class='panel panel-primary'>" +

"<div class='panel-heading'>" +
adi + "/" +
"</div>" +
"<div class='panel-body'>" +
"<div class='table-responsive'>" +
"<div class='btn btn-xs-group'>" +
"<a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint.toUrlValue(6) + "));map.setZoom(20);'><i class='btn btn-xs btn-success '>Yaklaş</i></a> <a href='javascript:map.fitBounds(bounds);'><i class='btn btn-xs btn-danger'>Uzaklaş</i></a>" +
"<a href='TekAnten.aspx?id=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Kapsam İçindeki Müşteri Haritası</i> </a>" +
"<a href='MusteriAnten2.aspx?antenid=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Antene Kayıtlı Müşteri Listesi</i> </a>" +
"<a href='TekAnten2?id=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Antene Kayıtlı Müşteri Haritası</i> </a>" +

"</div>" +
"</div>" +

"</div>" +

" </div>");

    var arcPts = drawArc(centerPoint, centerPoint.Bearing(startPoint), centerPoint.Bearing(endPoint), centerPoint.distanceFrom(startPoint));
    // add the start and end lines
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
};




function mapBaslat() {
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
    //antenler = GetAllAntens();
    google.maps.event.addListener(map, 'click', function () {
        infowindow.close();
    });
}

function initializeAntenMusteri() {
    var antenler = [];
    var musteriler = [];

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

                var anten_id = objData.anten_id;
                antenler[anten_adi] = { center: new google.maps.LatLng(center_Lat, center_Long), start: new google.maps.LatLng(start_Lat, start_Long), end: new google.maps.LatLng(end_Lat, end_Long), adi: anten_adi, anten_id: anten_id };


            });

        },
        complete: function () {

            $.ajax({
                type: "GET",
                url: "/api/Musterianten/",
                contentType: "json",
                dataType: "json",
                success: function (data) {

                    $.each(data, function (key, value) {
                        //stringify
                        var jsonData = JSON.stringify(value);
                        //Parse JSON
                        var objData = $.parseJSON(jsonData);
                        var musteri_id = objData.musteri_id;
                        var musteri_adi = objData.musteri_adi;
                        var center_Lat = objData.center_Lat;
                        var center_Long = objData.center_Long;
                        var musteri_id = objData.musteri_id;
                        var musteri_adres = objData.musteri_adres;

                        if (center_Lat) {

                            musteriler[musteri_id] = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres };
                        }


                    });
                },
                complete: function () {
                    mapBaslat();

                    for (var musteri in musteriler) {

                        createMarkerM(musteriler[musteri].center, musteriler[musteri].adi);

                    };

                    for (var city in antenler) {
                        tekli(antenler[city].start, antenler[city].end, antenler[city].center, antenler[city].adi, antenler[city].anten_id);
                    };

                    map.fitBounds(bounds);
                },
                error: function (xhr) {
                    console.log(xhr);
                }
            });
        },

        error: function (xhr) {
            console.log(xhr);
        }
    });

}
