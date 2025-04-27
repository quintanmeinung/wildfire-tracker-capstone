const { addAQIMarker } = require('./AQI-Testing');
const L = require('./leaflet-mock');

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

function createOverlayLayers() {
    const cities = L.layerGroup([
        L.marker([44.9429, -123.0351]).bindPopup("Salem, Oregon - Default View")
    ]);

    const aqiLayer = L.layerGroup();

    // Add mock AQI markers
    addAQIMarker(aqiLayer, {
        lat: 44.97643,
        lon: -122.97647,
        aqi: 42,
        color: '#00E400',
        location: 'Salem Chemeketa Community College',
        dominantPollutant: 'pm25',
        pm25: 7,
        lastUpdated: '2025-03-18T00:00:00Z',
        attributions: [
            {
                url: 'https://www.oregon.gov/DEQ/',
                name: 'Oregon Department of Environmental Quality (DEQ)'
            }
        ]
    });

    return {
        "Cities": cities,
        "AQI Stations": aqiLayer
    };
}

function setupMap() {
    const map = initializeMap();

    // Add base layers
    const baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map);

    // Add overlay layers
    const overlayLayers = createOverlayLayers();
    L.control.layers(baseLayers, overlayLayers).addTo(map);

    return map;
}

module.exports = { setupMap, initializeMap, createBaseLayers, createOverlayLayers };

