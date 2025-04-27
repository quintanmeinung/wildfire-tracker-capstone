import { addAQIMarker } from './AQI.js';
import { addFireMarkers } from './fireMarkers.js';

document.addEventListener("DOMContentLoaded", function () {
    //Initialize the Leaflet Map
    var map = initializeMap();

    //Layer Control Functions
    var baseLayers = createBaseLayers();
    baseLayers["Street Map"].addTo(map);

    var overlayLayers = createOverlayLayers(map);
    overlayLayers["Fire Reports"].addTo(map);

    var layerControl = L.control.layers(baseLayers, overlayLayers);
    layerControl.addTo(map);

     // Date control logic
     const dateInput = document.getElementById("fire-date");

     const today = new Date();
      
     const todayStr = formatLocalDate(today);
     
    const minDateObj = new Date(today);
    minDateObj.setDate(today.getDate() - 30);
    const minDateStr = formatLocalDate(minDateObj);
 
     dateInput.max = todayStr;
     dateInput.min = minDateStr;
     dateInput.value = todayStr;
 
     // 🔁 (we’ll fetch and display markers for the default date next in Step 3)

    handleGeolocation(map);
    addLegend(map);
    initializeCompass(map);

    // 🔥 Fetch wildfire data and display markers
    //This code is in test production
    const fireLayer = overlayLayers["Fire Reports"];

    // 🔁 Fetch markers for the default date (initial load)
    showSpinner();
    fetch(`/api/WildfireAPIController/fetchWildfiresByDate?date=${dateInput.value}`)
        .then(response => response.json())
        .then(data => {
            fireLayer.clearLayers();
            addFireMarkers(fireLayer, data);
        })
        .catch(error => {
            console.error('Error loading default fire markers:', error);
        })  
        .finally(() => {
            hideSpinner();
        });

document.getElementById("filter-date-btn").addEventListener("click", () => {
    const selectedDateStr = dateInput.value;
    const minDateStr = dateInput.min;
    const maxDateStr = dateInput.max;

    if (selectedDateStr < minDateStr || selectedDateStr > maxDateStr) {
        alert("Please select a date within the valid range.");
        return;
    }


    showSpinner();

    // ✅ Use the string version of the input, NOT `selectedDate` (which is a Date object)
    const dateStr = dateInput.value;

    fetch(`/api/WildfireAPIController/fetchWildfiresByDate?date=${dateStr}`)
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
        

    //Spinner Control Functions
    function showSpinner() {
        document.getElementById("loading-spinner").style.display = "block";
    }

    function hideSpinner() {
        document.getElementById("loading-spinner").style.display = "none";
    }

    //Function to load up the leaflet map for application
    function initializeMap() {
        return L.map('map').setView([44.84, -123.23], 10); // Monmouth, Oregon
    }

    //Set up different terrain layers for the viewer to choose from
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

    //Create both AQI and Fire layers to include on the map
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

    //Function to set up the local time in PST Time Zone
    function formatLocalDate(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        return `${year}-${month}-${day}`;
    }
});






