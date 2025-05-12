import { addAQIMarker } from './AQI.js';
import { addFireMarkers } from './fireMarkers.js';
import { getUserId } from './site.js'; // Import userId
import { initDialogModal } from './saveLocationModalHandler.js'; // Import modal handler

document.addEventListener("DOMContentLoaded", function () {
    // Initialize Leaflet Map
    const map = initializeMap();
    window._leaflet_map = map;

    // Base Layers
    const baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map);

    // Overlay Layers
    const overlayLayers = createOverlayLayers(map);
    const layerControl = L.control.layers(baseLayers, overlayLayers).addTo(map);

    // Fire Layer (we'll keep a reference)
    const fireLayer = overlayLayers["Fire Reports"];
    fireLayer.addTo(map);

    //Fire Marker Layer
    //const wildfireMarkersLayer = overlayLayers["Wildfire Markers"];
    //wildfireMarkersLayer.addTo(map);
        
    // Date control setup
    const dateInput = document.getElementById("fire-date");
    const today = new Date();
    const todayStr = formatLocalDate(today);

    const minDateObj = new Date(today);
    minDateObj.setDate(today.getDate() - 30);
    const minDateStr = formatLocalDate(minDateObj);

    dateInput.max = todayStr;
    dateInput.min = minDateStr;
    dateInput.value = todayStr;

    // Handle geolocation
    handleGeolocation(map);

    // Add legend + compass
    addLegend(map);
    initializeCompass(map);

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
            map.on('click', function (e) {
                addMarkerOnClick(e, map)
            });
        }
    }

    // Fetch wildfire data for today's date automatically
    showSpinner();
    fetch(`/api/WildfireAPIController/fetchWildfiresByDate?date=${dateInput.value}`)
        .then(response => response.json())
        .then(data => {
            fireLayer.clearLayers();
            addFireMarkers(fireLayer, data);

            if (data.length === 0) {
                console.warn('No wildfires reported today.');
            }
        })
        .catch(error => {
            console.error('Error loading initial fire markers:', error);
        })
        .finally(() => {
            hideSpinner();
        });

    // Filter button click
    document.getElementById("filter-date-btn").addEventListener("click", () => {
        const selectedDateStr = dateInput.value;
        const minDateStr = dateInput.min;
        const maxDateStr = dateInput.max;

        if (selectedDateStr < minDateStr || selectedDateStr > maxDateStr) {
            alert("Please select a date within the valid range.");
            return;
        }

        showSpinner();
        fetch(`/api/WildfireAPIController/fetchWildfiresByDate?date=${selectedDateStr}`)
            .then(response => response.json())
            .then(data => {
                fireLayer.clearLayers();
                addFireMarkers(fireLayer, data);

                if (data.length === 0) {
                    alert("No wildfires were reported for this date.");
                }
            })
            .catch(error => {
                console.error('Error fetching wildfire data for selected date:', error);
            })
            .finally(() => {
                hideSpinner();
            });
    });
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

// Spinner functions
function showSpinner() {
    document.getElementById("loading-spinner").style.display = "block";
}

function hideSpinner() {
    document.getElementById("loading-spinner").style.display = "none";
}

// Map Setup
function initializeMap() {
    return L.map('map').setView([44.84, -123.23], 10); // Monmouth, OR
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

    //Emergency Shelters Layer 1
    const shelterClusterGroup = L.markerClusterGroup();

    //Wildfire Risk Layer
    window.fireHazardLayer = L.esri.imageMapLayer({
        url: 'https://apps.fs.usda.gov/fsgisx01/rest/services/RDW_Wildfire/RMRS_WRC_WildfireHazardPotential/ImageServer',
        opacity: 0.4 //org 0.6
    });
        
    // Use Esri Leaflet to get shelter features
    const femaShelters = L.esri.featureLayer({
        url: 'https://gis.fema.gov/arcgis/rest/services/NSS/FEMA_NSS/FeatureServer/5',
        pointToLayer: function (geojson, latlng) {
            if (map.getZoom() < 7) {
                return null; // don't draw marker at low zoom
            }

            const status = geojson.properties.shelter_status_code;
            let markerOptions;
    
            if (status === "OPEN") {
                markerOptions = {
                    radius: 6,
                    color: 'green',
                    fillColor: 'lime',
                    fillOpacity: 0.9,
                    weight: 2
                };
            } else {
                markerOptions = {
                    radius: 2,
                    color: '#007bff',
                    fillColor: '#007bff',
                    fillOpacity: 0.6,
                    weight: 1
                };
            }
    
            return L.circleMarker(latlng, markerOptions);
        },
        onEachFeature: function (feature, layer) {
            const props = feature.properties;

            // Attach a test attribute to the SVG element for automation
            layer.on('add', function () {
                const el = layer.getElement();
                if (el) {
                    // ✅ Add 'shelter-marker' class while preserving existing classes
                    const existingClass = el.getAttribute('class') || '';
                    el.setAttribute('class', `${existingClass} shelter-marker`.trim());
                    el.setAttribute('data-type', 'shelter'); // Optional: keep if useful elsewhere
                    el.classList.add('shelter-marker');
                }
            });

            let petAccommodations = "Not listed – call ahead";
            if (props.pet_accommodations_desc === " ") {
                petAccommodations = "Pet Accommodations: Unknown";
            } else if (props.pet_accommodations_desc) {
                petAccommodations = props.pet_accommodations_desc;
            }

            const popup = `
                <strong>${props.shelter_name || "Unnamed Shelter"}</strong><br>
                Address: ${props.address_1 || "Unknown"}<br>
                City: ${props.city || "N/A"}<br>
                Evacuation Capacity: ${props.evacuation_capacity || "N/A"}<br>
                Post-Impact Capacity: ${props.post_impact_capacity || "N/A"}<br>
                ADA Compliant: ${props.ada_compliant === 'Y' ? "Yes" : "No"}<br>
                Wheelchair Accessible: ${props.wheelchair_accessible === 'Y' ? "Yes" : "No"}<br>
                Pet Accommodations: ${petAccommodations}<br>
                Generator Onsite: ${props.generator_onsite === 'Y' ? "Yes" : "No"}<br>
                Status: ${props.shelter_status_code || "N/A"}
            `;

            layer.bindPopup(popup);
            shelterClusterGroup.addLayer(layer);
        }

    });

    femaShelters.on('load', function (e) {
        const features = e.featureCollection.features;
        const hasOpenShelters = features.some(f => f.properties.shelter_status_code === "OPEN");
    
        if (!hasOpenShelters) {
            console.warn("No open shelters found at this time.");
            // Optional: Display an info control or popup
            L.popup()
                .setLatLng([44.9429, -123.0351]) // Default location or center
                .setContent("No open shelters currently reported. All listed locations are closed.")
                .openOn(map);
        }
    });
            
    // Add the layer to the map on startup
    //wildfireRiskLayer.addTo(map);
    //shelterClusterGroup.addTo(map);
    //femaShelters.addTo(map);
    
    return {
        "Emergency Shelters": femaShelters,
        "Evacuation Zone": shelterClusterGroup,
        "Cities": cities,
        "AQI Stations": aqiLayer,
        "Fire Reports": fireLayer,
        "Wildfire Hazard Potential": window.fireHazardLayer
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

    // Format Date to YYYY-MM-DD
function formatLocalDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
}




