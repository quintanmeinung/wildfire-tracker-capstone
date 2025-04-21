import { addAQIMarker } from './AQI.js';
import { getUserId } from './site.js'; // Import userId
import { initDialogModal } from './saveLocationModalHandler.js'; // Import modal handler


document.addEventListener("DOMContentLoaded", function () {
    var map = initializeMap();

    var baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map);

    var overlayLayers = createOverlayLayers(map);

    var layerControl = L.control.layers(baseLayers, overlayLayers);
    layerControl.addTo(map);

    handleGeolocation(map);
    addLegend(map);
    initializeCompass(map);

    // Add dynamic markers for logged-in users
    var userId = getUserId(); // Get the user ID from the site.js file
    if (userId !== "") {
        var profileElement = document.getElementById("profile");

        // Get saved locations from the profile element data attribute(Index.cshtml)
        var savedLocations = profileElement.dataset.savedLocations;

        console.log("Saved locations:", savedLocations);
        if (savedLocations) {
            
            // Parse the JSON string to an object
             savedLocations = JSON.parse(savedLocations); 

            for (let location of savedLocations) {
                console.log(location);
                let marker = L.marker([location.latitude, location.longitude]).addTo(map);
                marker.bindPopup(location.title); // Bind the name to the marker popup
            } 
        }

        map.on('click', function (e) {
            addMarkerOnClick(e, map)
        });
    }
});

let activeMarker = null; // Variable to store user's most recent marker
function addMarkerOnClick(e, map) {
    if (activeMarker) {
        map.removeLayer(activeMarker); // Remove the previous marker if it exists
    }
    // Create a new marker at the clicked location
    activeMarker = L.marker(e.latlng).addTo(map);

    // Create a popup with a button to save the location
    var popup = document.createElement('div');
    popup.id = 'save-location-popup';
    popup.className = 'btn btn-primary';
    popup.innerHTML = 'Save Location';
    popup.dataset.lat = e.latlng.lat.toFixed(5); // Store latitude in dataset
    popup.dataset.lng = e.latlng.lng.toFixed(5); // Store longitude in dataset
    activeMarker.bindPopup(popup);
    activeMarker.openPopup(); // Open the popup immediately
    initDialogModal(); // Initialize the modal handler
}



/**
 * Initializes the Leaflet map.
 */
function initializeMap() {
    return L.map('map').setView([44.84, -123.23], 10); // Monmouth, Oregon
}

function createBaseLayers() {
    return {
        "Street Map": L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '© OpenStreetMap'
        }),
        "Satellite": L.tileLayer('https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
            maxZoom: 19,
            attribution: '© Esri'
        }),
        "Topographic Map": L.tileLayer('https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '© OpenStreetMap contributors, SRTM | Map style: © OpenTopoMap (CC-BY-SA)'
        })
    };
}

function createOverlayLayers(map) {
    const cities = L.layerGroup().addLayer(
        L.marker([44.9429, -123.0351]).bindPopup("Salem, Oregon - Default View")
    );

    const aqiLayer = initializeAqiLayer();
    const fireLayer = L.layerGroup();

    return {
        "Cities": cities,
        "AQI Stations": aqiLayer,
        "Fire Reports": fireLayer
    };
}

function initializeAqiLayer() {
    const aqiLayer = L.layerGroup();
    addAQIMarker(aqiLayer, "A503596"); // Salem Chemeketa
    addAQIMarker(aqiLayer, "@91");     // Silverton
    addAQIMarker(aqiLayer, "@83");     // Lyons
    addAQIMarker(aqiLayer, "@89");     // Salem
    addAQIMarker(aqiLayer, "A503590"); // Dallas
    addAQIMarker(aqiLayer, "@11923");  // Turner
    return aqiLayer;
}

function handleGeolocation(map) {
    if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition(
            position => onGeolocationSuccess(position, map),
            onGeolocationError
        );
    } else {
        console.log("Geolocation not supported by this browser");
    }
}

function onGeolocationSuccess(position, map) {
    const { latitude, longitude } = position.coords;
    map.panTo([latitude, longitude]);
    L.marker([latitude, longitude])
        .bindPopup("Your current location")
        .openPopup()
        .addTo(map);

    addGeolet(map);
}

function onGeolocationError(error) {
    addGeolet(map);
    const errorMessages = {
        1: "Permission Denied",
        2: "Location information unavailable",
        3: "The request timed out",
        0: "An unknown error occurred"
    };
    console.log(errorMessages[error.code] || "An error occurred");
}

function addGeolet(map) {
    if (typeof L.geolet !== "undefined") {
        L.geolet({ position: 'bottomleft', title: 'Find Current Location' }).addTo(map);
        console.log("Geolet added to map");
    } else {
        console.error("Geolet plugin failed to load.");
    }
}

function initializeCompass(map) {
    if (typeof L.control.compass !== "undefined") {
        L.control.compass({
            position: 'topright',
            autoActive: true,
            showDigit: true
        }).addTo(map);
    } else {
        console.error("Leaflet Compass plugin failed to load.");
    }
}