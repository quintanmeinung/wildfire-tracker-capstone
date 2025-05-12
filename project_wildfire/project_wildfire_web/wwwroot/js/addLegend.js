/**
 * Adds a collapsible and interactive legend to the map.
 */
function addLegend(map) {
    var legend = L.control({ position: 'bottomright' });

    legend.onAdd = function () {
        var div = L.DomUtil.create('div', 'legend-container');

        // Fetch and insert modal HTML
        fetch('legendModal.cshtml')
            .then(response => response.text())
            .then(modalHtml => {
                document.body.insertAdjacentHTML('beforeend', modalHtml);
            })
            .catch(error => console.error('Error loading legend modal:', error));

        // Generate legend content dynamically
        div.innerHTML = generateLegendHTML();

        // Attach toggle functionality for collapsible legend
        var toggleButton = div.querySelector('.legend-toggle');
        var legendContent = div.querySelector('.legend-content');

        toggleButton.addEventListener('click', function () {
            var isHidden = legendContent.style.display === 'none';
            legendContent.style.display = isHidden ? 'block' : 'none';
            toggleButton.innerHTML = isHidden ? 'Legend ▲' : 'Legend ▼';
        });

        // Attach modal event listeners
        attachLegendEvents();

        return div;
    };

    legend.addTo(map);
}

/**
 * Generates the HTML content for the legend dynamically using an array.
 */
function generateLegendHTML() {
    const legendSections = {
        "Evacuation Zones & Wildfire Risk": [
            { color: "#9ACD32", label: "Safe Zone", info: "Area considered safe.", shape: "square" },
            { color: "#6DAE4F", label: "Low Risk", info: "Currently low risk; stay aware.", shape: "square" },
            { color: "#FFFF33", label: "Stay Alert", info: "Be ready for updates.", shape: "square" },
            { color: "#FFD700", label: "Prepare to Evacuate", info: "Pack essentials.", shape: "square" },
            { color: "#FFA500", label: "Evacuate Now", info: "Leave with key items.", shape: "square" },
            { color: "#FF3333", label: "Evacuate Immediately", info: "Immediate danger.", shape: "square" },
            { color: "#3399FF", label: "Evacuation Zone", info: "Official zone.", shape: "square" }
        ],
        "Fire Activity": [
            { color: "#FFD700", label: "Lesser Fire", info: "Minor fire.", shape: "circle" },
            { color: "#FF8C00", label: "Moderate Fire", info: "Moderate severity.", shape: "circle" },
            { color: "#DC143C", label: "Extreme Fire", info: "Severe wildfire.", shape: "circle" }
        ],
        "Shelter Status": [
            { color: "#32CD32", label: "Open Shelter", info: "Available for evacuees.", shape: "circle" },
            { color: "#3399FF", label: "Closed Shelter", info: "Not accepting evacuees.", shape: "circle" }
        ]
    };

    let legendHTML = `
        <div class="info legend collapsible-legend">
            <button class="legend-toggle">Legend ▼</button>
            <div class="legend-content" style="display: none;">
                <div class="legend-grid">`;

    // Left Column
    legendHTML += `<div class="legend-column"><h4>Evacuation Zones & Wildfire Risk</h4>`;
    legendSections["Evacuation Zones & Wildfire Risk"].forEach(item => {
        const shapeStyle = item.shape === "circle" ? "border-radius: 50%;" : "border-radius: 0;";
        legendHTML += `
            <div class="legend-item" data-info="${item.info}">
                <i style="background: ${item.color}; ${shapeStyle} border: 1px solid #555;"></i> ${item.label}
            </div>`;
    });
    legendHTML += `</div>`;

    // Right Column: Fire Activity + Shelter Status
    legendHTML += `<div class="legend-column"><h4>Fire Activity</h4>`;
    legendSections["Fire Activity"].forEach(item => {
        legendHTML += `
            <div class="legend-item" data-info="${item.info}">
                <i style="background: ${item.color}; border-radius: 50%; border: 1px solid #555;"></i> ${item.label}
            </div>`;
    });

    legendHTML += `<h4 style="margin-top: 15px;">Shelter Status</h4>`;
    legendSections["Shelter Status"].forEach(item => {
        legendHTML += `
            <div class="legend-item" data-info="${item.info}">
                <i style="background: ${item.color}; border-radius: 50%; border: 1px solid #555;"></i> ${item.label}
            </div>`;
    });

    legendHTML += `</div>`; // Close right column
    legendHTML += `</div></div></div>`; // Close grid, content, container

    return legendHTML;
}

/**
 * Attaches event listeners to legend items for modal display.
 */
function attachLegendEvents() {
    document.querySelectorAll('.legend-item').forEach(item => {
        item.addEventListener('click', function () {
            let info = this.dataset.info;
            document.getElementById('legendInfoContent').innerText = info;
            new bootstrap.Modal(document.getElementById('legendInfoModal')).show();
        });
    });
}

/**
 * Function to find the modal content and update it when needed for testing
 */
function updateLegendInfo(content) {
    const legendInfoContent = document.getElementById("legendInfoContent");
  
    if (!legendInfoContent) {
      console.error("Legend modal content element not found.");
      return;
    }
  
    legendInfoContent.innerHTML = content;
  }
  
  module.exports = { updateLegendInfo };
  
