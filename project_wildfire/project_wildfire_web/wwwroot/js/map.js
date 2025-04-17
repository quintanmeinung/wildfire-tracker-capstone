import { addAQIMarker } from './AQI.js'; //imports AQI.js file
import { addFireMarkers } from './fireMarkers.js';
import { getUserId } from './site.js'; // Import userId
import { initDialogModal } from './saveLocationModalHandler.js'; // Import modal handler


document.addEventListener("DOMContentLoaded", function () {
    // Initialize the map
    var map = initializeMap();

    // Get saved locations from the map element
    var savedLocations = map.dataset.savedLocations;
    if (savedLocations) {

        // Parse the JSON string to an object
        //savedLocations = JSON.parse(savedLocations); 

        for (let location of savedLocations) {
            let marker = L.marker([location.latitude, location.longitude]).addTo(map);
            marker.bindPopup(location.title); // Bind the name to the marker popup
        }
    }

    // Initialize base layers
    var baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map); // Default layer

    // Initialize overlays
    var overlayLayers = createOverlayLayers(map, false);

    // Add layer control to the map
    var layerControl = L.control.layers(baseLayers, overlayLayers);
    layerControl.addTo(map);

    // Handle geolocation
    handleGeolocation(map);

    // Add legend
    addLegend(map);

    // Initialize compass
    initializeCompass(map);

    // Add dynamic markers for logged-in users
    var userId = getUserId(); // Get the user ID from the site.js file
    if (userId !== "") {
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

/**
 * Creates and returns base layers for the map.
 */
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

/**
 * Creates and returns overlay layers.
 */
function createOverlayLayers(map) {
    // Create a layer group for cities
    var cities = L.layerGroup().addLayer(L.marker([44.9429, -123.0351]).bindPopup("Salem, Oregon - Default View"));

    // Initialize AQI layer with any predefined markers
    const aqiLayer = initializeAqiLayer();

    // Fire layer initialized but not populated yet
    const fireLayer = L.layerGroup();

    return {
        "Cities": cities,
        "AQI Stations": aqiLayer,
        "Fire Reports": fireLayer,
        "Saved Locations": savedLocations
    };
}

function initializeAqiLayer() {
    const aqiLayer = L.layerGroup();
    // Example: Add AQI markers (this function needs to be defined or adjusted as per existing AQI code)
    addAQIMarker(aqiLayer, "A503596"); // Salem Chemeketa Community College
    addAQIMarker(aqiLayer, "@91"); // Silverton, Oregon
    addAQIMarker(aqiLayer, "@83"); // Lyons, Oregon
    addAQIMarker(aqiLayer, "@89"); // Salem, Oregon
    addAQIMarker(aqiLayer, "A503590"); // Dallas, Oregon
    addAQIMarker(aqiLayer, "@11923"); // Turner Cascade Jr.High, Oregon
    return aqiLayer;
}


/**
 * Handles geolocation logic, including user location retrieval and error handling.
 */
function handleGeolocation(map) {
    if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition(
            (position) => onGeolocationSuccess(position, map),
            onGeolocationError
        );
    } else {
        console.log("Geolocation not supported by this browser");
    }
}

/**
 * Called on successful retrieval of geolocation data.
 */
function onGeolocationSuccess(position, map) {
    var { latitude, longitude } = position.coords;
    map.panTo([latitude, longitude]);

    // Add a marker for the user's current location
    L.marker([latitude, longitude])
        .bindPopup("Your current location")
        .openPopup()
        .addTo(map);

    addGeolet(map);
}

/**
 * Handles errors from the geolocation API.
 */
function onGeolocationError(error) {
    addGeolet(map);

    var errorMessages = {
        1: "Permission Denied",
        2: "Location information unavailable",
        3: "The request timed out",
        0: "An unknown error occurred"
    };

    console.log(errorMessages[error.code] || "An error occurred");
}

/**
 * Adds the Geolet geolocation plugin if available.
 */
function addGeolet(map) {
    if (typeof L.geolet !== "undefined") {
        L.geolet({ position: 'bottomleft', title: 'Find Current Location' }).addTo(map);
        console.log("Geolet added to map");
    } else {
        console.error("Geolet plugin failed to load.");
    }
}

/**
 * Initializes the Leaflet compass control if available.
 */
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


/**
 * Test logic for no markers.
 */
/*
const testParam = new URLSearchParams(window.location.search).get("test");

if (testParam === "no-data") {
  console.log("🧪 Test Mode: no-data triggered");
  // skip loading markers
} else {
  fetch("/api/WildfireAPIController/fetchWildfires").then(...)
}*/



