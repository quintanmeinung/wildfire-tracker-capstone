document.addEventListener("DOMContentLoaded", function () {
    // Initialize the map
    var map = initializeMap();

    // Initialize base layers
    var baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map); // Default layer

    // Initialize overlays
    var overlayLayers = createOverlayLayers();

    // Add layer control to the map
    L.control.layers(baseLayers, overlayLayers).addTo(map);

    // Handle geolocation
    handleGeolocation(map);

    // Add legend
    addLegend(map);

    // Initialize compass
    initializeCompass(map);
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
function createOverlayLayers() {
    var cities = L.layerGroup([
        L.marker([44.9429, -123.0351]).bindPopup("Salem, Oregon - Default View")
    ]);

    return {
        "Cities": cities
    };
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
 * Initializes and adds a collapsible legend to the map.
 */
function addLegend(map) {
    var legend = L.control({ position: 'bottomright' });

    legend.onAdd = function () {
        var div = L.DomUtil.create('div', 'info legend collapsible-legend');
        div.innerHTML = `
            <button class="legend-toggle">Legend ▼</button>
            <div class="legend-content" style="display: none;">
                <div class="legend-item" data-info="Designated Zone for Evacuation.">
                    <i style="background:rgb(19, 188, 72)"></i> Evacuation Zone
                </div>
                <div class="legend-item" data-info="Stay on constant alert for fire updates.">
                    <i style="background: #ffffb2"></i> Stay Alert
                </div>
                <div class="legend-item" data-info="Prepare to bring all essentials and other items of interest.">
                    <i style="background: #fecc5c"></i> Prepare to Evacuate
                </div>
                <div class="legend-item" data-info="Only bring essential items.">
                    <i style="background: #fd8d3c"></i> Evacuate Now
                </div>
                <div class="legend-item" data-info="Leave all materials behind.">
                    <i style="background: #f03b20"></i> Evacuate Immediately
                </div>
                <div class="legend-item" data-info="Perimeter of the nearby fire.">
                    <i style="background: #bd0026"></i> Fire Perimeter
                </div>
            </div>
        `;

        // Toggle legend visibility
        div.querySelector('.legend-toggle').addEventListener('click', function () {
            var content = div.querySelector('.legend-content');
            var button = div.querySelector('.legend-toggle');
            content.style.display = content.style.display === 'none' ? 'block' : 'none';
            button.innerHTML = content.style.display === 'none' ? 'Legend ▼' : 'Legend ▲';
        });

        // Add click event to legend items for modal display
        div.querySelectorAll('.legend-item').forEach(item => {
            item.addEventListener('click', function () {
                document.getElementById('legendInfoContent').innerText = item.dataset.info;
                new bootstrap.Modal(document.getElementById('legendInfoModal')).show();
            });
        });

        return div;
    };

    legend.addTo(map);
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
