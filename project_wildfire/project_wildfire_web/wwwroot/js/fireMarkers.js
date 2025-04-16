// Function to add fire markers to a given Leaflet layer
export function addFireMarkers(fireLayer, apiData) {
  apiData.forEach(fire => {
      // Create a red circle marker at the latitude and longitude from the API data
      L.circle([fire.latitude, fire.longitude], {
          color: 'red',      // Color of the circle
          fillColor: '#f03', // Fill color of the circle
          fillOpacity: 0.5,  // Transparency of the circle
          radius: 500        // Radius of the circle in meters
      }).bindPopup(`🔥 Radiative Power: ${fire.radiativePower}`)
        .addTo(fireLayer);
  });
}
