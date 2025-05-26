// Handles the dialog modal for saving a location

export function initDialogModal() {
    // Grab the hidden modal
    var dialogModal = new bootstrap.Modal(document.getElementById('dialogModal'));

    var latlng = window.savedLocationMarkers['temp-marker'].getLatLng();
    console.log(latlng); // Log the latlng object to the console for debugging

    // Set the lat & lng inputs
    var latInput = document.getElementById("latInput");
    var lngInput = document.getElementById("lngInput");
    latInput.value = latlng.lat.toFixed(5); // Set latitude input value truncated to 5 decimals
    lngInput.value = latlng.lng.toFixed(5); // Set longitude input value truncated to 5 decimals

    document.getElementById("save-location-popup").addEventListener("click", function (e) {
        e.preventDefault(); // Prevent default link behavior
        dialogModal.show(); // Show the modal
    });
};