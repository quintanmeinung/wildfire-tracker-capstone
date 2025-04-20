import { addAQIMarker } from './AQI.js'; //imports AQI.js file
import { addFireMarkers } from './fireMarkers.js';


document.addEventListener("DOMContentLoaded", function () {
    var map = initializeMap();
    var baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map);

    var overlayLayers = createOverlayLayers(map, false);
    var layerControl = L.control.layers(baseLayers, overlayLayers);
    layerControl.addTo(map);

    const testParam = new URLSearchParams(window.location.search).get("test");

    if (!testParam) {
        handleGeolocation(map);
    } else {
        console.log("🧪 Test mode → skipping geolocation");
    }

    addLegend(map);
    initializeCompass(map);

    // 🧪 Test data logic
    if (testParam === "no-data") {
        console.log("🧪 Test Mode: no-data → Skipping fire markers");
    }
    else if (testParam === "single") {
        console.log("🧪 Test Mode: single → Adding one fire marker");
        const testFires = [
            { latitude: 45.0, longitude: -120.5, radiativePower: 40.2 }
        ];
        addFireMarkers(overlayLayers["Fire Reports"], testFires);
        layerControl.addOverlay(overlayLayers["Fire Reports"], "Fire Reports");
    }
    else if (testParam === "multiple") {
        console.log("🧪 Test Mode: multiple → Adding two fire markers");
        const testFires = [
            { latitude: 45.0, longitude: -120.5, radiativePower: 45.7 },
            { latitude: 46.0, longitude: -121.5, radiativePower: 50.1 }
        ];
        addFireMarkers(overlayLayers["Fire Reports"], testFires);
        layerControl.addOverlay(overlayLayers["Fire Reports"], "Fire Reports");
    }
    else {
        // 🌍 Normal mode
        console.log("🌍 Normal mode → Fetching wildfire data from API");
        fetch('/api/WildfireAPIController/fetchWildfires' + window.location.search)
            .then(response => response.json())
            .then(data => {
                addFireMarkers(overlayLayers["Fire Reports"], data);
                layerControl.addOverlay(overlayLayers["Fire Reports"], "Fire Reports");
            })
            .catch(error => {
                console.error('Error fetching wildfire data:', error);
                alert('Failed to fetch wildfire data.');
            });
    }
});



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
        "Fire Reports": fireLayer
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




