import { addAQIMarker } from './AQI.js';
import { addFireMarkers } from './fireMarkers.js';
import { getUserId } from '../site.js'; // Import userId
import { initDialogModal } from '../SaveLocationScripts/saveLocationModalHandler.js'; // Import modal handler
import {addLegend } from './addLegend.js';

// Track overridden statuses keyed by shelter ID
const shelterStatusOverrides = {};

const savedLocationMarkers = {}; // Tracks saved location markers
window.savedLocationMarkers = savedLocationMarkers; // Expose to global scope
document.addEventListener("DOMContentLoaded", function () {
    // Initialize Leaflet Map
    const userId = getUserId();
    const map = initializeMap();
    window._leaflet_map = map;

    // Exposes the map to be accessible from BDD tests
    window.webfireMap = map; 

    // Base Layers
    const baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map);

    // Overlay Layers
    createOverlayLayers(map).then(overlayLayers => {
        const fireLayer = overlayLayers["Fire Reports"];
        fireLayer.addTo(map);
        const layerControl = L.control.layers(baseLayers, overlayLayers).addTo(map);

        // 🔥 Admin Fire Creation Tool
        const createFireButton = document.getElementById("createFireBtn");

        if (window.isAdmin && createFireButton) {
            createFireButton.addEventListener("click", () => {
                alert("🖱️ Click on the map to place a simulated fire marker.");
                window.firePlacementMode = true;
            });
        }

        if (userId !== "") {
            const profileElement = document.getElementById("profile");
            const savedLocations = profileElement.dataset.savedLocations;

            if (savedLocations) {
                const locations = JSON.parse(savedLocations);
                for (let location of locations) {
                    const marker = L.marker([location.latitude, location.longitude]).addTo(map);
                    savedLocationMarkers[location.id] = marker;
                    marker.bindPopup(location.title);
                    marker.getElement().id = location.title;
                }
            }
        }

        map.on('click', function (e) {
            if (window.firePlacementMode) {
                const { lat, lng } = e.latlng;
                const simulatedPower = Math.floor(Math.random() * (60 - 5 + 1)) + 5;

                const fireIcon = L.divIcon({
                    className: 'admin-fire-marker',
                    html: '<div style="background: red; border-radius: 50%; width: 16px; height: 16px; border: 2px solid #800000;"></div>',
                    iconSize: [16, 16],
                    iconAnchor: [8, 8]
                });

                const fireMarker = L.marker([lat, lng], {
                    icon: fireIcon
                }).addTo(fireLayer);

                fireMarker.bindPopup(`
                    <strong>🔥 Simulated Fire</strong><br>
                    Latitude: ${lat.toFixed(4)}<br>
                    Longitude: ${lng.toFixed(4)}<br>
                    Radiative Power: ${simulatedPower}<br>
                    <em>Placed by admin</em><br>
                    <em>Saving to database...</em>
                `).openPopup();

                // Save to DB
                fetch('/api/AdminFire/Create', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        latitude: lat,
                        longitude: lng,
                        radiativePower: simulatedPower,
                        isAdminFire: true
                    })
                })
                .then(response => {
                    if (!response.ok) throw new Error("Failed to save fire.");
                    return response.json();
                })
                .then(result => {
                    console.log("✅ Admin fire saved:", result);

                    const fireId = result.fireId; // Use returned ID from DB
                    window.fireMarkerMap?.set(fireId, fireMarker);

                    fireMarker.setPopupContent(`
                        <strong>🔥 Simulated Fire</strong><br>
                        Latitude: ${lat.toFixed(4)}<br>
                        Longitude: ${lng.toFixed(4)}<br>
                        Radiative Power: ${simulatedPower}<br>
                        <em>Placed by admin</em><br>
                        <button class="delete-admin-fire btn btn-sm btn-danger" data-fire-id="${fireId}">
                            🗑️ Delete Fire
                        </button>
                    `);
                })
                .catch(err => {
                    console.error("❌ Error saving admin fire:", err);
                    fireMarker.setPopupContent(`
                        <strong>🔥 Simulated Fire</strong><br>
                        Latitude: ${lat.toFixed(4)}<br>
                        Longitude: ${lng.toFixed(4)}<br>
                        <em>Error saving to DB</em>
                    `);
                });

                window.firePlacementMode = false;
                return;
            }


        // Load today's fire data
        showSpinner();
        fetch("/api/WildfireAPIController/getSavedFires")
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

        // Date filter click handler
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
    //initializeCompass(map);

    //var userId = getUserId(); // Get the user ID from the site.js file
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
                savedLocationMarkers[location.id] = marker; // Store the marker in the savedLocations object
                marker.bindPopup(location.title); // Bind the name to the marker popup
                marker.getElement().id = location.title; // Set the marker ID to the location ID
            }
        }
    } 

    // Fetch wildfire data for today's date automatically
    showSpinner();
  //  fetch(`/api/WildfireAPIController/fetchWildfiresByDate?date=${dateInput.value}`)
    fetch("/api/WildfireAPIController/getSavedFires")
        .then(response => response.json())
        .then(data => {
            fireLayer.clearLayers();
            addFireMarkers(fireLayer, data);
            //
        //
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
                //
                        // Attach event listeners to "Subscribe to Fire" buttons

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

    document.addEventListener('click', function (e) {
        const link = e.target.closest('.fire-jump');
        if (link) {
            e.preventDefault();
            const lat = parseFloat(link.dataset.lat);
            const lng = parseFloat(link.dataset.lng);
            if (!isNaN(lat) && !isNaN(lng)) {
                
            window._leaflet_map.setView([lat, lng],13);
                const fireId = parseInt(link.textContent.trim());
            const marker = window.fireMarkerMap?.get(fireId);
                if (marker) {
                    marker.fire('click');
                }
            const modalElement = document.getElementById('profileModal')
            if(modalElement && bootstrap){
                const modalInstance = bootstrap.Modal.getInstance(modalElement);
                if(modalInstance){
                    modalInstance.hide()
                }
            }
            }
        }
    });

});
   

function addMarkerOnClick(e, map) {
    savedLocationMarkers['temp-marker']?.remove(); // Remove existing temp marker if any

    let newMarker = L.marker(e.latlng).addTo(map); // Create a new marker at the clicked location
    newMarker.getElement().id = 'temp-marker'; // Set the ID to 'temp-marker'
    
    const popup = document.createElement('div');
    popup.className = 'btn btn-primary';
    popup.innerHTML = `Save Location`;
    popup.id = 'save-location-popup';

    newMarker.bindPopup(popup);
    newMarker.openPopup(); // Open the popup immediately

    savedLocationMarkers['temp-marker'] = newMarker; // Store the marker in the savedLocations object
    initDialogModal(); // Initialize the modal handler
}

//export function removeMarker(id) {
function removeMarker(id) {
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

//export function addMarker(userLocationDto) {
function addMarker(userLocationDto) {
    // Given the location DTO it will add the marker to the map.
    const { id, title, latitude, longitude } = userLocationDto;
    console.log("Adding marker:", userLocationDto);

    if (savedLocationMarkers[id]) {
        console.error(`Marker with title "${title}" already exists.`);
        return;
    }

    const marker = L.marker([latitude, longitude])
    marker.addTo(window._leaflet_map);
    savedLocationMarkers[id] = marker; // Store the marker in the savedLocations object
    marker.bindPopup(title); // Bind the name to the marker popup
    marker.getElement().id = title; // Set the marker ID to the location ID
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
    const map = L.map('map').setView([44.84, -123.23], 10); // Monmouth, OR
    var compass = L.control.compass({ autoActive: true }).addTo(map);
    return map;
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

async function createOverlayLayers(map) {
    const cities = L.layerGroup().addLayer(
        L.marker([44.9429, -123.0351]).bindPopup("Salem, Oregon - Default View")
    );
    

    const aqiLayer = await initializeAqiLayer();
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

            const shelterId = geojson.properties.shelter_id || geojson.properties.OBJECTID;
            let status = geojson.properties.shelter_status_code;

            // Check for override
            if (shelterStatusOverrides[shelterId]) {
                status = shelterStatusOverrides[shelterId];
            }

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

            //Extract shelter ID:
            const shelterId = props.shelter_id || props.OBJECTID;

            // Check for override status (fallback to original)
            let status = props.shelter_status_code;
            if (shelterStatusOverrides[shelterId]) {
                status = shelterStatusOverrides[shelterId];
            }

            // Build popup content
            let petAccommodations = "Not listed – call ahead";
            if (props.pet_accommodations_desc === " ") {
                petAccommodations = "Pet Accommodations: Unknown";
            } else if (props.pet_accommodations_desc) {
                petAccommodations = props.pet_accommodations_desc;
            }

            let popup = `
                <strong>${props.shelter_name || "Unnamed Shelter"}</strong><br>
                Address: ${props.address_1 || "Unknown"}<br>
                City: ${props.city || "N/A"}<br>
                Evacuation Capacity: ${props.evacuation_capacity || "N/A"}<br>
                Post-Impact Capacity: ${props.post_impact_capacity || "N/A"}<br>
                ADA Compliant: ${props.ada_compliant === 'Y' ? "Yes" : "No"}<br>
                Wheelchair Accessible: ${props.wheelchair_accessible === 'Y' ? "Yes" : "No"}<br>
                Pet Accommodations: ${petAccommodations}<br>
                Generator Onsite: ${props.generator_onsite === 'Y' ? "Yes" : "No"}<br>
                Status: <strong>${status}</strong><br>
            `;

            // 👮 Add toggle button only if user is an admin
            if (window.isAdmin) {
                popup += `
                    <button class="toggle-shelter-status-btn" data-id="${shelterId}">
                        ${status === "OPEN" ? "Mark as CLOSED" : "Mark as OPEN"}
                    </button>
                `;
            }

            // Bind popup
            layer.bindPopup(popup);

            // Tag marker in DOM for automation/testing (optional)
            layer.on('add', function () {
                const el = layer.getElement();
                if (el) {
                    const existingClass = el.getAttribute('class') || '';
                    el.setAttribute('class', `${existingClass} shelter-marker`.trim());
                    el.setAttribute('data-type', 'shelter');
                }
            });

            // Add to cluster group
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

document.addEventListener('click', function (e) {
    const button = e.target.closest('.toggle-shelter-status-btn');
    if (button) {
        const shelterId = button.dataset.id;
        const current = shelterStatusOverrides[shelterId] || "CLOSED";
        const newStatus = current === "OPEN" ? "CLOSED" : "OPEN";

        // Update override
        shelterStatusOverrides[shelterId] = newStatus;

        // ✅ Refresh popup and marker
        femaShelters.eachFeature(function (layer) {
            const props = layer.feature.properties;
            const id = props.shelter_id || props.OBJECTID;

            if (id == shelterId) {
                // Rebuild the popup content
                let status = newStatus;

                let petAccommodations = "Not listed – call ahead";
                if (props.pet_accommodations_desc === " ") {
                    petAccommodations = "Pet Accommodations: Unknown";
                } else if (props.pet_accommodations_desc) {
                    petAccommodations = props.pet_accommodations_desc;
                }

                let popup = `
                    <strong>${props.shelter_name || "Unnamed Shelter"}</strong><br>
                    Address: ${props.address_1 || "Unknown"}<br>
                    City: ${props.city || "N/A"}<br>
                    Evacuation Capacity: ${props.evacuation_capacity || "N/A"}<br>
                    Post-Impact Capacity: ${props.post_impact_capacity || "N/A"}<br>
                    ADA Compliant: ${props.ada_compliant === 'Y' ? "Yes" : "No"}<br>
                    Wheelchair Accessible: ${props.wheelchair_accessible === 'Y' ? "Yes" : "No"}<br>
                    Pet Accommodations: ${petAccommodations}<br>
                    Generator Onsite: ${props.generator_onsite === 'Y' ? "Yes" : "No"}<br>
                    Status: <strong>${status}</strong><br>
                `;

                if (window.isAdmin) {
                    popup += `
                        <button class="toggle-shelter-status-btn" data-id="${id}">
                            ${status === "OPEN" ? "Mark as CLOSED" : "Mark as OPEN"}
                        </button>
                    `;
                }

                // Re-bind the popup
                layer.bindPopup(popup).openPopup();

                // ✅ Update the marker style
                const style = (status === "OPEN") ?
                    {
                        radius: 6,
                        color: 'green',
                        fillColor: 'lime',
                        fillOpacity: 0.9,
                        weight: 2
                    } :
                    {
                        radius: 2,
                        color: '#007bff',
                        fillColor: '#007bff',
                        fillOpacity: 0.6,
                        weight: 1
                    };

                layer.setStyle(style);
            }
        });

        console.log(`🔄 Shelter ${shelterId} status changed to ${newStatus}`);
    }
});

    
    return {
        "Emergency Shelters": femaShelters,
        "Evacuation Zone": shelterClusterGroup,
        "Cities": cities,
        "AQI Stations": aqiLayer,
        "Fire Reports": fireLayer,
        "Wildfire Hazard Potential": window.fireHazardLayer
    };
}

//Global JS Listener for Admin Fire Deletion
let fireBeingDeleted = new Set();

document.addEventListener('click', function (e) {
    const deleteBtn = e.target.closest('.delete-admin-fire');
    if (deleteBtn) {
        const fireId = deleteBtn.dataset.fireId;

        if (fireBeingDeleted.has(fireId)) return; // 🛡️ Already deleting this fire

        if (confirm("Are you sure you want to delete this admin fire?")) {
            fireBeingDeleted.add(fireId); // ⏳ Mark fire as being deleted

            fetch(`/api/AdminFire/Delete/${fireId}`, {
                method: 'DELETE'
            })
            .then(response => {
                if (!response.ok) throw new Error("Delete failed.");
                
                const marker = window.fireMarkerMap?.get(parseInt(fireId));
                if (marker) {
                    marker.setStyle({ opacity: 0.3, fillOpacity: 0.3 });
                    setTimeout(() => {
                        marker.remove();
                        window.fireMarkerMap.delete(parseInt(fireId));
                    }, 300);
                }

                alert("🔥 Admin fire deleted.");
            })
            .catch(err => {
                console.error("❌ Fire delete failed:", err);
                alert("Failed to delete fire. Check console for details.");
            })
            .finally(() => {
                fireBeingDeleted.delete(fireId); 
            });
        }
    }
});

async function initializeAqiLayer() {
    const aqiLayer = L.layerGroup();

    try {
        const response = await fetch('/api/aqi/stations');
        const stations = await response.json();

        stations.forEach(station => {
            addAQIMarker(aqiLayer, station.stationId); 
        });
    } catch (error) {
        console.error('Error loading AQI stations:', error);
    }
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
});
