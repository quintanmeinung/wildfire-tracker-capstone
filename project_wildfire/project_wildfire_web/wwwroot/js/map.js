document.addEventListener("DOMContentLoaded", function () {
    var map = L.map('map').setView([44.9429, -123.0351], 10); // Salem, Oregon

    // Define base layers
    var osm = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap'
    });

    var topo = L.tileLayer('https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors, SRTM | Map style: © OpenTopoMap (CC-BY-SA)'
    });

    // Alternative satellite layer using ESRI
    var satellite = L.tileLayer('https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
        maxZoom: 19,
        attribution: '© Esri'
    });

    // Set default base layer
    osm.addTo(map);

    // Define overlay layers
    var cities = L.layerGroup([
        L.marker([44.9429, -123.0351]).addTo(map)
        .bindPopup("Salem, Oregon - Default View")
        .openPopup()

    ]);

    // Base map options
    var baseMaps = {
        "Street Map": osm,
        "Satellite": satellite,
        "Topographic Map": topo
    };

    // Overlay options
    var overlayMaps = {
        "Cities": cities
    };

    // Add layer control to the map
    L.control.layers(baseMaps, overlayMaps).addTo(map);

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
});
