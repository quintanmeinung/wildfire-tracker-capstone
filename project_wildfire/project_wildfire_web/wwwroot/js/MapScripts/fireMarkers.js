export function addFireMarkers(fireLayer, apiData) {
  apiData.forEach(fire => {
    const power = fire.radiativePower;

    // Determine color based on radiative power
    let color = '';
    if (power < 10) {
      color = 'yellow'; // small fires
    } else if (power < 30) {
      color = 'orange'; // moderate fires
    } else {
      color = 'red'; // strong fires
    }

    // Determine radius based on radiative power
    let radius = 5; // default small
    if (power >= 10 && power < 30) {
      radius = 7; // medium
    } else if (power >= 30) {
      radius = 9; // large
    }

    const marker = L.circleMarker([fire.latitude, fire.longitude], {
      radius: radius,
      fillColor: color,
      color: color,
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8,
      className: "wildfire-marker"
    }).bindPopup(`
      <strong>🔥 Wildfire Detected!</strong><br>
      <strong>Radiative Power:</strong> ${power}<br>
      <strong>Latitude:</strong> ${fire.latitude.toFixed(5)}<br>
      <strong>Longitude:</strong> ${fire.longitude.toFixed(5)}
    `);

    marker.addTo(fireLayer);
  });
}



