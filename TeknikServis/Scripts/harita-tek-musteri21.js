
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



//querystringden okunacak
var id = getParameterByName('id');
//var id = 74;
var urlBase = "/api/Musteri/";
var url = urlBase + "?id=" + id;



function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

function tekli(startPoint, endPoint, centerPoint, adi, anten_id) {

    //createMarker(centerPoint, "Anten: " + adi + " Yönetici: " + bayi +
    //          "<br><a href='javascript:map.setCenter(new google.maps.LatLng(" + centerPoint.toUrlValue(6) + "));map.setZoom(20);'>Yaklaş</a> - <a href='javascript:map.fitBounds(bounds);'>Uzaklaş</a>" +
    //      "<br><a href='TekAnten.aspx?id=" + anten_id + "'>Anten müşterilerine git </a>");

    createMarker(centerPoint, "<div class='panel panel-primary'>" +

"<div class='panel-heading'>" +
adi + "/" +
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
    //poligonlar[centerPoint] = { center: centerPoint, poly: piePoly };
};

function musteriEkle(musteri) {

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

    //}
}



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

    google.maps.event.addListener(map, 'click', function () {
        infowindow.close();
    });
}

function initialize2() {


    //var id = $('#<%=txtSearch.ClientID %>').val();
    $.getJSON(url,
        function (data) {
            mapBaslat();
            var geocoder = new google.maps.Geocoder();
            var musteri_id = data.musteri_id;
            var musteri_adi = data.musteri_adi;
            var center_Lat = data.center_Lat;
            var center_Long = data.center_Long;
            var musteri_adres = data.musteri_adres;

            if (!center_Lat) {

                geocoder.geocode({ 'address': musteri_adres }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {

                        musteri = { center: new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng()), adi: musteri_adi, musteri_id: musteri_id };
                    } else {
                        console.log('Geocode error: ' + status);
                        //alert('Geocode error: ' + status);
                    }
                });


            }
            else {
                musteri = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id };

            }
            if (musteri) {
                musteriEkle(musteri);
            }


        })
        .done(function () {
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
                        anten = { center: new google.maps.LatLng(center_Lat, center_Long), start: new google.maps.LatLng(start_Lat, start_Long), end: new google.maps.LatLng(end_Lat, end_Long), adi: anten_adi, anten_id: anten_id };
                        tekli(anten.start, anten.end, anten.center, anten.adi, anten.anten_id);

                    });
                    map.fitBounds(bounds);

                },
                error: function (xhr) {
                    console.log(xhr);
                }
            });

        })
    .fail(
        function (jqXHR, textStatus, err) {

        });


}
function initialize() {

    var antenler = [];
    var musteri = {};
    //var id = $('#<%=txtSearch.ClientID %>').val();
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
            $.getJSON(url, function (data) {

                mapBaslat();
                var geocoder = new google.maps.Geocoder();
                var musteri_id = data.musteri_id;
                var musteri_adi = data.musteri_adi;
                var center_Lat = data.center_Lat;
                var center_Long = data.center_Long;
                var musteri_adres = data.musteri_adres;

                if (!center_Lat) {

                    geocoder.geocode({ 'address': musteri_adres }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {

                            musteri = { center: new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng()), adi: musteri_adi, musteri_id: musteri_id };
                        } else {
                            console.log('Geocode error: ' + status);
                            //alert('Geocode error: ' + status);
                        }
                    });


                }
                else {
                    musteri = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id };

                }



            })
      .done(function () {
          if (musteri) {
              musteriEkle(musteri);
          }
      

          for (var city in antenler) {
              tekli(antenler[city].start, antenler[city].end, antenler[city].center, antenler[city].adi, antenler[city].anten_id);
          };

          map.fitBounds(bounds);

      })
  .fail(
      function (jqXHR, textStatus, err) {

      });

        },
        error: function (xhr) {
            console.log(xhr);
        }
    });


}
