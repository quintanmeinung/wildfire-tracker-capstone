import { addAQIMarker } from './AQI.js';
import { addFireMarkers } from './fireMarkers.js';
import { getUserId } from '../site.js'; // Import userId
import { initDialogModal } from '../SaveLocationScripts/saveLocationModalHandler.js'; // Import modal handler
import {addLegend } from './addLegend.js';

const savedLocationMarkers = {}; // Tracks saved location markers
document.addEventListener("DOMContentLoaded", function () {
    // Initialize Leaflet Map
    const map = initializeMap();

    // Base Layers
    const baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map);

    // Overlay Layers
    const overlayLayers = createOverlayLayers(map);
    const layerControl = L.control.layers(baseLayers, overlayLayers).addTo(map);

    // Fire Layer (we'll keep a reference)
    const fireLayer = overlayLayers["Fire Reports"];
    fireLayer.addTo(map);

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

                let marker = L.marker([location.latitude, location.longitude], { locationId: location.id}).addTo(map);
                savedLocationMarkers[location.id] = marker; // Store the marker in the savedLocations object
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

export function removeMarker(id) {
    // Given the location ID it will remove the marker from the map.
    const marker = savedLocationMarkers[id];
    console.log("Removing marker:", marker);
    if (marker) {
        marker.remove(); // Remove the marker from the map
        delete savedLocationMarkers[id]; // Remove from the savedLocations object
    } else {
        console.error(`Marker with title "${id}" not found.`);
    }
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
/*
    function addLegend(map) {
        const legend = L.control({ position: 'bottomright' });

        legend.onAdd = function () {
            const div = L.DomUtil.create('div', 'info legend');
            div.innerHTML += `<h4>Radiative Power</h4>
            <i style="background: green;"></i> Low<br>
            <i style="background: yellow;"></i> Medium<br>
            <i style="background: red;"></i> High<br>`;
            return div;
        };

        legend.addTo(map);
    }
*/
    // Format Date to YYYY-MM-DD
function formatLocalDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    return `${year}-${month}-${day}`;
}




