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

    //Add AQI Marker
    addAQIMarker(map,"A503596"); //Salem Chemeketa Community College
    addAQIMarker(map, "@91"); //Silverton, Oregon
    addAQIMarker(map, "@83"); //Lyons, Oregon
    addAQIMarker(map, "@89"); //Salem, Oregon
    addAQIMarker(map, "A503590"); //Dallas, Oregon
    addAQIMarker(map, "@11923"); //Turner Cascade Jr.High, Oregon
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

//Fetches AQI Data from API
async function fetchAQIData(stationId) {
    const apiKey = "152b70cd77ae9f824be07461d6ca46df5ff8b7c3";
    const apiUrl = `https://api.waqi.info/feed/${stationId}/?token=${apiKey}`;

    try {
        const response = await fetch(apiUrl);
        const data = await response.json();

        if (data.status === "ok") {
            return {
                aqi: data.data.aqi,
                lat: data.data.city.geo[0],  // Extract station latitude
                lon: data.data.city.geo[1],  // Extract station longitude
                location: data.data.city.name || `Station ${stationId}`
            };
        } else {
            console.error("Failed to fetch AQI data:", data);
            return null;
        }
    } catch (error) {
        console.error("Error fetching AQI data:", error);
        return null;
    }
}

// Function to determine AQI color
function getAQIColor(aqi) {
    return aqi <= 50 ? "green" :
           aqi <= 100 ? "yellow" :
           aqi <= 150 ? "orange" :
           aqi <= 200 ? "red" :
           aqi <= 300 ? "purple" : "maroon";
}

// Function to add AQI markers to the map
async function addAQIMarker(map, stationId) {
    const aqiData = await fetchAQIData(stationId);

    if (aqiData) {
        L.circleMarker([aqiData.lat, aqiData.lon], {
            radius: 10,
            color: getAQIColor(aqiData.aqi),
            fillColor: getAQIColor(aqiData.aqi),
            fillOpacity: 0.7
        })
        .bindPopup(`<strong>${aqiData.location}</strong><br>AQI: ${aqiData.aqi}`)
        .addTo(map); // <-- Now it has access to map
    }
}








