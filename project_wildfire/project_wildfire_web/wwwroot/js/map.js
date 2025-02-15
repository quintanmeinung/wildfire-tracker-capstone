document.addEventListener("DOMContentLoaded", function () {
    var map = L.map('map').setView([44.84, -123.23], 10); // Monmouth, Oregon
    
    // Use OpenStreetMap as the base layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    // Default marker at Monmouth, OR
    var defaultMarker = L.marker([44.84,-123.23])
    defaultMarker.bindPopup('Monmouth Oregon')
    defaultMarker.openPopup();
    defaultMarker.addTo(map);

    // Add Geolocation Button
    if (typeof L.geolet !== "undefined") {
        L.geolet({ position: 'bottomleft', title: 'Find Current Location' }).addTo(map);
    } else {
        console.error("Geolet plugin failed to load.");
    }

    // Ensure Leaflet Compass is available before initializing
    if (L.control.compass) {
        L.control.compass({
            position: 'top-left',  // Change position as needed
            autoActive: true,      // Automatically activates on page load
            showDigit: true        // Shows numeric heading values
        }).addTo(map);
    } 
    else {
        console.error("Leaflet Compass plugin failed to load.");
    }


/*********** HTML5 USER LOCATION REQUEST *****************/

//function for successful retrieval of browser geolocation -- success callback 
    
//Check if geolocation is available in browser
    if("geolocation" in navigator){
                    
        navigator.geolocation.getCurrentPosition(success,handleError);
    }
    else {
        console.log("Geolocation not supported by this browser");
    }

    //success callback function
    function success(position){
        //Delete default marker from map
        map.removeLayer(defaultMarker);
           
        const crds = position.coords;
         //Set the map view to users location
        map.panTo([crds.latitude, crds.longitude]);

        //Add marker to map showing users browser location
        var userMarker = L.marker([crds.latitude, crds.longitude])
            userMarker.bindPopup("Your current location");
            userMarker.openPopup();
            userMarker.addTo(map);
    }

    //Handle errors from geolocation
    function handleError(error) {
        switch(error.code) {
            case error.PERMISSION_DENIED:
                console.log("Permission Denied");
                break;
        }
    }



});
