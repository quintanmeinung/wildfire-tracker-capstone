// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

let userId;

document.addEventListener("DOMContentLoaded", function () {
    userId = document.body.dataset.id;
});

export function getUserId() {
    console.log("Sending  User ID:", userId);
    return userId;
}

// Fetch and add wildfire data as markers
fetch('/api/WildfireAPIController/fetchWildfires')
.then(response => response.json())
.then(data => {
    // Assuming addFireMarkers is updated to handle dynamic data
    addFireMarkers(overlayLayers["Fire Reports"], data);
    layerControl.addOverlay(overlayLayers["Fire Reports"], "Fire Reports");
})
.catch(error => {
    console.error('Error fetching wildfire data:', error);
    alert('Failed to fetch wildfire data.');
});