"use strict";
$(document).ready(function(){
    map = new GMaps({
        el: '#gmap-simple',
        lat: 21.192572,
        lng: 72.799736,
        zoom : 0,
        panControl : false,
        streetViewControl : false,
        mapTypeControl: false,
        overviewMapControl: false
    });
    var mapCanvas = document.getElementById("gmap-marker");
    var myCenter = new google.maps.LatLng(21.192572,72.799736);
    var mapOptions = {center: myCenter, zoom: 5};
    var map = new google.maps.Map(mapCanvas,mapOptions);
    var marker = new google.maps.Marker({
        position: myCenter
    });
    marker.setMap(map);
    var mapCanvas = document.getElementById("gmap-marker-custom");
    var myCenter = new google.maps.LatLng(21.192572,72.799736);
    var mapOptions = {center: myCenter, zoom: 5};
    var map = new google.maps.Map(mapCanvas,mapOptions);
    var marker = new google.maps.Marker({
        position: myCenter,
        icon: "../../images/location.png",
        animation: google.maps.Animation.BOUNCE
    });
    marker.setMap(map);
    var mapCanvas = document.getElementById("gmap-marker-animation");
    var myCenter = new google.maps.LatLng(21.192572,72.799736);
    var mapOptions = {center: myCenter, zoom: 5};
    var map = new google.maps.Map(mapCanvas,mapOptions);
    var marker = new google.maps.Marker({
        position: myCenter,
        animation: google.maps.Animation.BOUNCE
    });
    marker.setMap(map);
    $(document).ready(function(){
        map = new GMaps({
            el: '#overlayermap',
            lat: 21.192572,
            lng: 72.799736
        });
        map.drawOverlay({
            lat: map.getCenter().lat(),
            lng: map.getCenter().lng(),
            layer: 'overlayLayer',
            verticalAlign: 'center',
            horizontalAlign: 'center'
        });
    });
    $(document).ready(function(){
        map = new GMaps({
            el: '#Polygons',
            lat: -12.040397656836609,
            lng: -77.03373871559225,
            click: function(e){
                console.log(e);
            }
        });
        var paths;
        paths = [
            [
                [
                    [-12.040397656836609,-77.03373871559225]

                ],[
                [-12.040248585302038,-77.03993927003302]
            ]
            ],[
                [
                    [-12.050047116528843,-77.02448169303511]
                ]
            ]
        ];
        var path;
        path = [[-12.040397656836609,-77.03373871559225], [-12.040248585302038,-77.03993927003302], [-12.050047116528843,-77.02448169303511], [-12.044804866577001,-77.02154422636042]];
        map.drawPolygon({
            paths: paths,
            useGeoJSON: true,
            strokeColor: '#263238',
            strokeWeight: 1
        });
        map.drawPolygon({
            paths: path,
            strokeColor: '#263238',
            strokeOpacity: 0.6,
            strokeWeight: 6
        });
    });
});
var mapCanvas = document.getElementById("gmap-marker-animation");
var myCenter = new google.maps.LatLng(21.192572,72.799736);
var mapOptions = {center: myCenter, zoom: 5};
var map = new google.maps.Map(mapCanvas,mapOptions);
var marker = new google.maps.Marker({
    position: myCenter,
    animation: google.maps.Animation.BOUNCE
});
marker.setMap(map);
var mapGeo = new GMaps({
    div: '#geocoding',
    lat:21.2334329,
    lng:72.866472
});
$('#search-form').submit(function(e){
    e.preventDefault();
    GMaps.geocode({
        address: $('#address').val().trim(),
        callback: function(results, status){
            if(status=='OK'){
                var latlng = results[0].geometry.location;
                mapGeo.setCenter(latlng.lat(), latlng.lng());
                mapGeo.addMarker({
                    lat: latlng.lat(),
                    lng: latlng.lng()
                });
            }
        }
    });
});