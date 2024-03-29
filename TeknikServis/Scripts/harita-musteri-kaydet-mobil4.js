﻿
//query stringle müşteri ID gelecek ve  burada müşteri ID'ye göre latlong müşteri kaydı yapılacak.
var map;
var markers = [];
var bounds = new google.maps.LatLngBounds();
var panorama;
//var baslangic;
//var lat = baslangic.coords.lat;
//var lng = baslangic.coords.lng;

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



var panaroma;

function initialize() {

    var musteri2;



    $.getJSON(url,
       function (data) {

           var musteri_id = data.musteri_id;
           var musteri_adi = data.musteri_adi;
           var center_Lat = data.center_Lat;
           var center_Long = data.center_Long;
           var musteri_adres = data.musteri_adres;
           musteri2 = { center_Lat: center_Lat, center_Long: center_Long, adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres };


           var lat = 36.066256248568465;
           var lng = 32.83886153222488;
           var mapOptions = {
               center: new google.maps.LatLng(lat, lng),
               zoom: 10,
               mapTypeId: google.maps.MapTypeId.ROADMAP
           };

           map = new google.maps.Map(document.getElementById("myMap"),
            mapOptions);

           google.maps.event.addListener(map, 'click', function (event) {
               addMarker(event.latLng);
           });

           var sv = new google.maps.StreetViewService();

           panorama = new google.maps.StreetViewPanorama(document.getElementById('pano'));

           if (center_Lat) {
               lat = center_Lat;
               lng = center_Long;
               var pos = new google.maps.LatLng(lat, lng);
               addMarker(pos);
           }
           else {

               var address = musteri_adres;
               if (!address) {
                   address = 'Anamur Mersin';
               }
               var geocoder = new google.maps.Geocoder();

               geocoder.geocode({ 'address': address }, function (results, status) {
                   if (status == google.maps.GeocoderStatus.OK) {
                       var longaddress = results[0].address_components[0].long_name;
                       var latitude = results[0].geometry.location.lat();

                       txtLatitude.value = latitude;

                       goster(results[0].geometry.location.lat(), results[0].geometry.location.lng(), longaddress);
                       console.log('Adresimiz: ' + address);
                   } else {
                       console.log('Geocode error: ' + status);

                   }
               });
           }
       })
   .fail(
       function (jqXHR, textStatus, err) {
           console.log('api hatasi');
       });

    //console.log('koordinatlar '+lat);



    map.setZoom(15);


}

function goster(lat, lng, address) {

    google.maps.event.addListener(map, 'click', function (event) {
        addMarker(event.latLng);
    });

    var pos = new google.maps.LatLng(lat, lng);
    addMarker(pos);


    var infotext = address + '<hr>' +
                   'Latitude: ' + lat + '<br>Longitude: ' + lng;
    var infowindow = new google.maps.InfoWindow();
    infowindow.setContent(infotext);
    infowindow.setPosition(new google.maps.LatLng(lat, lng));
    infowindow.open(map);

    //var pos = new google.maps.LatLng(baslangic.lat, baslangic.lng);
    //addMarker(pos);



}


function addMarker(location) {
    deleteMarkers();
    var marker = new google.maps.Marker({
        position: location,
        map: map
    });

    panorama = new google.maps.StreetViewPanorama(
         document.getElementById('pano'), {
             position: location,
             pov: {
                 heading: 34,
                 pitch: 10
             }
         });
    map.setStreetView(panorama);
    txtLatitude.value = location;

    bounds.extend(location);
    markers.push(marker);

}
function clearMarkers() {
    setAllMap(null);
}


function showMarkers() {
    setAllMap(map);
}


function deleteMarkers() {
    clearMarkers();
    markers = [];
}
function setAllMap(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}
function getLatLng() {
    var address = document.getElementById("txtAddress").value;
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var longaddress = results[0].address_components[0].long_name;
            var latitude = results[0].geometry.location.lat();
            //var longitude = results[0].geometry.location.lng();
            txtLatitude.value = latitude;
            //txtLongitude.value = longitude;
            goster(results[0].geometry.location.lat(), results[0].geometry.location.lng(), longaddress);
        } else {
            alert('Geocode error: ' + status);
        }
    });
}

