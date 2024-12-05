
class KTGoogleMaps {
    constructor() {

        return function() {
            var map = new GMaps({
                div: '#kt_gmap_3',
                lat: -51.38739,
                lng: -6.187181,
            });
            map.addMarker({
                lat: -51.38739,
                lng: -6.187181,
                title: 'Lima',
                details: {
                    database_id: 42,
                    author: 'HPNeo'
                },
                click: function(e) {
                    if (console.log) console.log(e);
                    alert('You clicked in this marker');
                }
            });
            map.addMarker({
                lat: -12.042,
                lng: -77.028333,
                title: 'Marker with InfoWindow',
                infoWindow: {
                    content: '<span style="color:#000">HTML Content!</span>'
                }
            });
            map.setZoom(5);
        };
    } 
}
 