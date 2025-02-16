document.addEventListener("DOMContentLoaded", function () {
    // Initialize the map
    var map = L.map('map').setView([44.9429, -123.0351], 10);

    // Define base layers
    var osm = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap'
    });

    var topo = L.tileLayer('https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors, SRTM | Map style: © OpenTopoMap (CC-BY-SA)'
    });

    var satellite = L.tileLayer('https://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
        maxZoom: 19,
        attribution: '© Esri'
    });

    // Set default base layer
    osm.addTo(map);

    // Define overlay layers
    var cities = L.layerGroup([
        L.marker([44.9429, -123.0351]).bindPopup("Salem, Oregon - Default View")
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

    // Initialize compass
    if (typeof L.control.compass !== "undefined") {
        L.control.compass({
            position: 'topright',
            autoActive: true,
            showDigit: true
        }).addTo(map);
    } else {
        console.error("Leaflet Compass plugin failed to load.");
    }

// Create a legend control
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
        const content = div.querySelector('.legend-content');
        const button = div.querySelector('.legend-toggle');
        if (content.style.display === 'none') {
            content.style.display = 'block';
            button.innerHTML = 'Legend ▲';
        } else {
            content.style.display = 'none';
            button.innerHTML = 'Legend ▼';
        }
    });

    // Add click event to each legend item to open the modal
    div.querySelectorAll('.legend-item').forEach(function (item) {
        item.addEventListener('click', function () {
            const info = item.dataset.info;
            document.getElementById('legendInfoContent').innerText = info;
            const legendModal = new bootstrap.Modal(document.getElementById('legendInfoModal'));
            legendModal.show();
        });
    });

    return div;
};

// Add the legend to the map
legend.addTo(map);
})
