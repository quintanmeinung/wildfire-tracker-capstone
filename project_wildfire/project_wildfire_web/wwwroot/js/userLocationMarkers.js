import { initDialogModal } from './saveLocationModalHandler.js';

export function initializeSavedLocations(map) {
    // Handle map click event to add dynamic markers
    map.on('click', function (e) {
        addMarkerOnClick(e, map)
    });

}

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
    popup.addEventListener('click', function (e) {
        var lat = e.latlng.lat.toFixed(5); // Get the latitude from the event
        var lng = e.latlng.lng.toFixed(5); // Get the longitude from the event
        initDialogModal();
    });

    activeMarker.bindPopup(popup).openPopup();
}


function saveLocation(dto) {
    // Fetch POST request to save the location
    fetch('/api/Location/SaveLocation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(dto)
    })
    .then(response => {
        if (response.ok) {
            return response.json();
        } else {
            throw new Error('Failed to save location');
        }
    })

    // Close the dialog box
    var dialogBox = document.getElementById('save-location-dialog');
    if (dialogBox) {
        dialogBox.remove();
    }
}