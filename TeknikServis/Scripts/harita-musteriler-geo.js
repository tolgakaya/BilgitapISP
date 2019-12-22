

 
var map = null;
var bounds = null;
var panorama;




var infowindow = new google.maps.InfoWindow(
  {
      size: new google.maps.Size(150, 50)
  });




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

function musteriEkle(musteri) {
    createMarkerM(musteri.center, "<div class='panel panel-primary'>" +

"<div class='panel-heading'>" +
 musteri.adi +
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
    //burası müşterilerin koordinatlarını tutuyor,adresten geocode ile alınan koordinatları kaydetmek isterseler diye
    document.getElementById("txtNoktalar").value += musteri.musteri_id + "-" + musteri.lat + "-" + musteri.lng + ",";
}
function initializeMusteriler() {

    var musteriler = [];
    $.ajax({
        type: "GET",
        url: "/api/Musteri/",
        contentType: "json",
        dataType: "json",
        async: false,
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

                    musteriler[musteri_id] = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres, lat: center_Lat, lng: center_Long };

                }


            });

        },
        complete: function () {
            mapBaslat();

            musteriler.map(function (musteri) {
                return musteriEkle(musteri);
            });

            map.fitBounds(bounds);
        },
        error: function (xhr) {
            console.log(xhr);
        }
    });



}


