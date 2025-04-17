// Handles the dialog modal for saving a location

export function initDialogModal() {
    // Get the profile link
    var saveButton = document.getElementById("save-location-popup");

    // Get the dialog modal
    var dialogModal = new bootstrap.Modal(document.getElementById('dialogModal'));

    // Add click event listener to the save button
    if (saveButton) {
        saveButton.addEventListener("click", function (e) {
            e.preventDefault(); // Prevent default link behavior
            dialogModal.show(); // Show the modal
        });
    }
};

