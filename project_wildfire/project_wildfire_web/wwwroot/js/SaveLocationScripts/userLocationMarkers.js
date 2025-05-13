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
        activeMarker.closePopup();
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
    
    activeMarker.bindPopup(popup).openPopup();
    initDialogModal();
}