

var musteriler = {};
var map = null;
var bounds = null;
var panorama;



function GetAllMusteris() {
    var geocoder = new google.maps.Geocoder();

    $.ajax({
        type: "GET",
        url: "/api/Musteriservis/",
        contentType: "json",
        dataType: "json",
        success: function (data) {

            $.each(data, function (key, value) {
                //stringify
                var jsonData = JSON.stringify(value);
                //Parse JSON
                var objData = $.parseJSON(jsonData);
                var servis_id = objData.servis_id;
                var musteri_id = objData.musteri_id;
                var musteri_adi = objData.musteri_adi;
                var center_Lat = objData.center_Lat;
                var center_Long = objData.center_Long;

                var musteri_adres = objData.musteri_adres;
                var kimlik = objData.kimlik;
                if (!center_Lat) {

                    geocoder.geocode({ 'address': musteri_adres }, function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {

                            musteriler[musteri_id] = { center: new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng()), adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres, lat: results[0].geometry.location.lat(), lng: results[0].geometry.location.lng(), servis_id: servis_id, kimlik: kimlik };

                        } else {
                            console.log('Geocode error: ' + status);
                            //alert('Geocode error: ' + status);
                        }
                    });


                }
                else {
                    musteriler[musteri_id] = { center: new google.maps.LatLng(center_Lat, center_Long), adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres, lat: center_Lat, lng: center_Long, servis_id: servis_id, kimlik: kimlik };

                }

            });
        },
        error: function (xhr) {
            console.log(xhr);
        }
    });

    return musteriler;
}


//var infowindow = new google.maps.InfoWindow(
//  {
//      size: new google.maps.Size(150, 50)
//  });




function createMarkerM(latlng, html) {
    var contentString = html;
    var infowindow = new google.maps.InfoWindow(
  {
      size: new google.maps.Size(150, 50)
  });

    var marker = new google.maps.Marker({
        position: latlng,
        map: map,
        icon: '/img/musteri.png',
        animation: google.maps.Animation.DROP,
        zIndex: Math.round(latlng.lat() * -100000) << 5
    });

    infowindow.setContent(contentString);
    infowindow.open(map, marker);

    bounds.extend(latlng);
    google.maps.event.addListener(marker, 'click', function () {


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



var musteriListesi = GetAllMusteris();

function initializeMusteriler() {
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

    //google.maps.event.addListener(map, 'click', function () {
    //    infowindow.close();
    //});


    for (var musteri in musteriler) {

        createMarkerM(musteriListesi[musteri].center, "<div class='panel panel-primary'>" +

       "<div class='panel-heading'>" +
         musteriListesi[musteri].adi +
       "</div>" +
       "<div class='panel-body'>" +
           "<div class='table-responsive'>" +
                      "<div class='btn btn-xs-group'>" +
    "<a href='javascript:map.setCenter(new google.maps.LatLng(" + musteriListesi[musteri].center.toUrlValue(6) + "));map.setZoom(20);'><i class='btn btn-xs btn-success '>Yaklaş</i></a> <a href='javascript:map.fitBounds(bounds);'><i class='btn btn-xs btn-danger'>Uzaklaş</i></a>" +
    "<a href='../MusteriDetayBilgileri.aspx?custID=" + musteriListesi[musteri].musteri_id + "'><i class='btn btn-xs btn-primary btn-block'>Müşteri Detay</i> </a>" +
    "<a href='MusteriYolu.aspx?id=" + musteriListesi[musteri].musteri_id + "'><i class='btn btn-xs btn-info btn-block'>Navigasyon</i> </a>" +
    "<a href='/TeknikTeknik/Servis.aspx?servisid=" + musteriListesi[musteri].servis_id + "&custid=" + musteriListesi[musteri].musteri_id + "&kimlik=" + musteriListesi[musteri].kimlik + "'><i class='btn btn-xs btn-info btn-block'>Servis Detayı</i> </a>" +
           "</div>" +
       "</div>" +

       "</div>" +

  " </div>");
        //burası müşterilerin koordinatlarını tutuyor,adresten geocode ile alınan koordinatları kaydetmek isterseler diye
        document.getElementById("txtNoktalar").value += musteriListesi[musteri].musteri_id + "-" + musteriListesi[musteri].lat + "-" + musteriListesi[musteri].lng + ",";
    };









    map.fitBounds(bounds);

}


