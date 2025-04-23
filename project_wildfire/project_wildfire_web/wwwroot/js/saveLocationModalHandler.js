// Handles the dialog modal for saving a location

export function initDialogModal() {
    // Get the button and attributes
    var saveButton = document.getElementById("save-location-popup");
    var lat = saveButton.dataset.lat; // Get latitude from dataset
    var lng = saveButton.dataset.lng; // Get longitude from dataset

    // Get the dialog modal
    var dialogModal = new bootstrap.Modal(document.getElementById('dialogModal'));

    // Set the lat & lng inputs
    var latInput = document.getElementById("latInput");
    var lngInput = document.getElementById("lngInput");
    latInput.value = lat; // Set latitude input value
    lngInput.value = lng; // Set longitude input value

    // Add click event listener to the save button
    if (saveButton) {
        saveButton.addEventListener("click", function (e) {
            e.preventDefault(); // Prevent default link behavior
            dialogModal.show(); // Show the modal
        });
    }
};