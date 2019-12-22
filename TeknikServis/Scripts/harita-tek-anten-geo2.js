


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
    var contentString = html;
    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        icon: '/img/musteri.png',
        animation: google.maps.Animation.DROP,
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


var id = getParameterByName('id');
//var id = 2;
var urlBase = "/api/Anten/";
var url = urlBase + "?id=" + id;

var anten;


var piePoly;

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

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
"<a href='MusteriAnten2.aspx?antenid=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Antene Kayıtlı Müşteri Listesi</i> </a>" +
"<a href='TekAnten2?id=" + anten_id + "'><i class='btn btn-xs btn-primary btn-block'>Antene Kayıtlı Müşteri Haritası</i> </a>" +

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
        map: map
    });

};


var map = null;
var bounds = null;


function musteriEkle(musteri) {

    if (piePoly.containsLatLng(musteri.center)) {


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
        //burası müşteri idlerini tutuyor antendekimüşteri listesini göstermek için
        document.getElementById("txtResult").value += musteri.musteri_id + ",";

        //burası müşterilerin koordinatlarını tutuyor,adresten geocode ile alınan koordinatları kaydetmek isterseler diye
        document.getElementById("txtNoktalar").value += musteri.musteri_id + "-" + musteri.lat + "-" + musteri.lng + ",";
    }

}

function mapBaslat() {
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

    var sv = new google.maps.StreetViewService();

    var panorama = new google.maps.StreetViewPanorama(document.getElementById('pano'));
}

function initialize() {
    var musteriler = [];
    var anten = {};

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
        .done(function () {

            $.ajax({
                type: "GET",
                url: "/api/Musteri/",
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
                        var musteri_adres = objData.musteri_adres;

                        if (center_Lat) {
                            musteriler[musteri_id] = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres, lat: center_Lat, lng: center_Long };
                        }

                    });
                },
                complete: function () {
                    mapBaslat();
                    tekli(anten.start, anten.end, anten.center, anten.adi, anten.anten_id);
                    musteriler.map(function (musteri) {
                        return musteriEkle(musteri);
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

