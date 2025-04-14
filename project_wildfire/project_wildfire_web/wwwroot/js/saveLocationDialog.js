// Add event listener to the save button in the modal
document.getElementById("saveLocationButton").addEventListener("click", async function (e) {
    e.preventDefault(); // Prevent default link behavior

    const userLocationDto = { // Create a DTO object to hold the location data
        Title: document.getElementById('titleInput').value,
        Address: document.getElementById('addressInput').value,
        Latitude: document.getElementById('latInput').value,
        Longitude: document.getElementById('lngInput').value,
        Radius: document.getElementById('radiusInput').value
    };
    console.log(userLocationDto); // Log the DTO to the console for debugging
    // Call the function to save the location with lat, lng, and locationName
    saveLocation(userLocationDto).then(function (response) {
        if (response.ok) {
            // Handle successful location save
            console.log("Location saved successfully!");
            // Optionally, you can close the modal here
            var dialogModal = bootstrap.Modal.getInstance(document.getElementById('dialogModal'));
            dialogModal.hide();
        } else {
            // Handle error in saving location
            console.error("Error saving location:", response.statusText);
        }
    });
});

// Update displayed value while sliding
function updateRadiusValue(val) {
    document.getElementById('radiusValue').textContent = val;
}

// Async function to save the location
async function saveLocation(userLocationDto) {
    try {
        const response = await fetch('/api/Location/SaveLocation', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userLocationDto) // Convert the DTO to JSON
        });
        return response; // Return the response object
    } catch (error) {
        console.error("Error saving location:", error);
    }
}