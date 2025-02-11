document.addEventListener("DOMContentLoaded", function () {
    var map = L.map('map').setView([37.7749, -122.4194], 5); // Default to U.S.

    // Use OpenStreetMap as the base layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    // Example: Add a sample marker
    L.marker([34.0522, -118.2437]).addTo(map) // Los Angeles Example
        .bindPopup("Sample Wildfire Marker")
        .openPopup();
});
