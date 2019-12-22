
//query stringle müşteri ID gelecek ve  burada müşteri ID'ye göre latlong müşteri kaydı yapılacak.
var map;
var markers = [];
var bounds = new google.maps.LatLngBounds();
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

function GetMusteri() {

    //var id = $('#<%=txtSearch.ClientID %>').val();
    var musteri;
    $.getJSON(url,
        function (data) {

            var musteri_id = data.musteri_id;
            var musteri_adi = data.musteri_adi;
            var center_Lat = data.center_Lat;
            var center_Long = data.center_Long;
            var musteri_adres = data.musteri_adres;
            musteri = { center_Lat: center_Lat, center_Long: center_Long, adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres };
            console.log(data);
        })
    .fail(
        function (jqXHR, textStatus, err) {
            console.log('api hatasi');
        });
    return musteri;
}


function initialize() {

    var musteri2;
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

    $.getJSON(url,
       function (data) {

           var musteri_id = data.musteri_id;
           var musteri_adi = data.musteri_adi;
           var center_Lat = data.center_Lat;
           var center_Long = data.center_Long;
           var musteri_adres = data.musteri_adres;
           musteri2 = { center_Lat: center_Lat, center_Long: center_Long, adi: musteri_adi, musteri_id: musteri_id, musteri_adres: musteri_adres };

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
                       //var longitude = results[0].geometry.location.lng();
                       txtLatitude.value = latitude;
                       //txtLongitude.value = longitude;
                       //lat = results[0].geometry.location.lat();
                       //lng = results[0].geometry.location.lng();
                       goster(results[0].geometry.location.lat(), results[0].geometry.location.lng(), longaddress);
                       console.log('Adresimiz: ' + address);
                   } else {
                       console.log('Geocode error: ' + status);
                       //alert('Geocode error: ' + status);
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
