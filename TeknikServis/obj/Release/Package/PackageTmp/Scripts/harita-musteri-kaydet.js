
//query stringle müşteri ID gelecek ve  burada müşteri ID'ye göre latlong müşteri kaydı yapılacak.
var map;
var markers = [];
var bounds = new google.maps.LatLngBounds();
//var baslangic;
//var lat = baslangic.coords.lat;
//var lng = baslangic.coords.lng;

function initialize(lat, lng, address) {

    var mapOptions = {
        center: new google.maps.LatLng(36, 45),
        zoom: 4,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("myMap"),
     mapOptions);

    google.maps.event.addListener(map, 'click', function (event) {
        addMarker(event.latLng);
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            // Success!
            var baslangic = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            addMarker(baslangic);
            var infotext = '<hr>' +
                  'Latitude: ' + position.coords.latitude + '<br>Longitude: ' + position.coords.longitude;
            var infowindow = new google.maps.InfoWindow();
            infowindow.setContent(infotext);
            infowindow.setPosition(new google.maps.LatLng(position.coords.latitude, position.coords.longitude));
            infowindow.open(map);
        });
    }
    else {
        var pos = new google.maps.LatLng(36.077286, 32.832889);
        addMarker(pos);
    }

    //var pos = new google.maps.LatLng(baslangic.lat, baslangic.lng);
    //addMarker(pos);

    map.setZoom(20);

}

function poziyon() {

    // Check for geolocation support
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            // Success!
            baslangic = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
        });
    }
    else {
        baslangic = new google.maps.LatLng(36, 42);
    }
    return baslangic;
};

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
            initialize(results[0].geometry.location.lat(), results[0].geometry.location.lng(), longaddress);
        } else {
            alert('Geocode error: ' + status);
        }
    });
}
