export function addWildfireMarkers(layerGroup, flameIcon) {
  fetch('/api/wildfires')
    .then(response => response.json())
    .then(data => {
      data.forEach(fire => {
        const popupContent = `
           <strong>${fire.name}</strong><br>
             ID: ${fire.uniqueFireIdentifier}<br>
             Start: ${new Date(fire.startDate).toLocaleString()}<br>
             Acres burned: ${fire.acreageBurned.toLocaleString()}<br>
             Contained: ${fire.percentageContained}%<br>
             County/State: ${fire.pooCounty}, ${fire.pooState}<br>
             Cause: ${fire.fireCause}
        `;
        const marker = L.marker([fire.latitude, fire.longitude], { icon: flameIcon });
        marker.bindPopup(popupContent);
        layerGroup.addLayer(marker);
      });
    })
    .catch(error => console.error('Error loading wildfire data:', error));
}