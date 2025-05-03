/**
 * Adds a collapsible and interactive legend to the map.
 */
export function addLegend(map) {
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
    const legendItems = [
        { color: "rgb(19, 188, 72)", label: "Evacuation Zone", info: "Designated Zone for Evacuation." },
        { color: "#ffffb2", label: "Stay Alert", info: "Stay on constant alert for fire updates." },
        { color: "#fecc5c", label: "Prepare to Evacuate", info: "Prepare to bring all essentials and other items of interest." },
        { color: "#fd8d3c", label: "Evacuate Now", info: "Only bring essential items." },
        { color: "#f03b20", label: "Evacuate Immediately", info: "Leave all materials behind." },
        { color: "#bd0026", label: "Fire Perimeter", info: "Perimeter of the nearby fire." }
    ];

    let legendHTML = `
        <div class="info legend collapsible-legend">
            <button class="legend-toggle">Legend ▼</button>
            <div class="legend-content" style="display: none;">`;

    legendItems.forEach(item => {
        legendHTML += `
            <div class="legend-item" data-info="${item.info}">
                <i style="background: ${item.color}"></i> ${item.label}
            </div>`;
    });

    legendHTML += `</div></div>`;
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
export function updateLegendInfo(content) {
    const legendInfoContent = document.getElementById("legendInfoContent");
  
    if (!legendInfoContent) {
      console.error("Legend modal content element not found.");
      return;
    }
  
    legendInfoContent.innerHTML = content;
}
