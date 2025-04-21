import { addAQIMarker } from './AQI.js';
import { addFireMarkers } from './fireMarkers.js';

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

    // 🔥 Fetch wildfire data and display markers
    fetch('/api/WildfireAPIController/fetchWildfires')
        .then(response => response.json())
        .then(data => {
            addFireMarkers(overlayLayers["Fire Reports"], data);
        })
        .catch(error => {
            console.error('Error fetching wildfire data:', error);
        });
});

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









