document.addEventListener("DOMContentLoaded", function () {
    var map = L.map('map').setView([44.9429, -123.0351], 10); // Salem, Oregon

    // Use OpenStreetMap as the base layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    // Example: Add a marker at Salem, OR
    L.marker([44.9429, -123.0351]).addTo(map)
        .bindPopup("Salem, Oregon - Default View")
        .openPopup();

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
    } else {
        console.error("Leaflet Compass plugin failed to load.");
    }


/*********** HTML5 USER LOCATION REQUEST *****************/

//function for successful retrieval of browser geolocation -- success callback 
function success(position){
    const crds = position.cords;

    //Add marker to map showing users browser location
    L.marker([crds.latitude, crds.longitude]).addTo(map)
        .bindPopup("Your current location")
        .openPopup();
}

//Handle errors from geolocation
function handleError(error) {
    switch(error.code) {
        case error.PERMISSION_DENIED:
            console.log("Permission Denied");
            break;
    }
}

//Check if the browser as the navigator.geolocation object
if ("geolocation" in navigator) {
    navigator.geolocation.getCurrentPosition(success,handleError);
}

});
