const L = require('./leaflet-mock');


function addAQIMarker(layer, stationData) {
    const marker = L.circleMarker([stationData.lat, stationData.lon], {
        radius: 10,
        color: stationData.color,
        fillColor: stationData.color,
        fillOpacity: 0.7
    });

    // Format attributions as clickable links
    let attributions = stationData.attributions && stationData.attributions.length
        ? stationData.attributions.map(attr =>
            `<a href="${attr.url}" target="_blank" rel="noopener noreferrer">${attr.name}</a>`
        ).join(', ')
        : "N/A";

    // Create the popup content
    const popupContent = `
        <strong>${stationData.location}</strong><br>
        AQI: ${stationData.aqi} (${stationData.dominantPollutant || "N/A"})<br>
        PM2.5: ${stationData.pm25 !== null ? `${stationData.pm25} µg/m³` : "N/A"}<br>
        Dominant Pollutant: ${stationData.dominantPollutant || "N/A"}<br>
        Last Updated: ${stationData.lastUpdated ? new Date(stationData.lastUpdated).toLocaleString() : "N/A"}<br>
        Attributions: ${attributions}<br>
    `;

    // Bind the popup with the widget and additional details
    marker.bindPopup(popupContent);

    //Add the marker to the AQI layer instead of the map directly
    layer.addLayer(marker);
}

async function fetchAQIData(lat, lon) {
    const apiUrl = `https://api.airquality.com/data?lat=${lat}&lon=${lon}&key=YOUR_API_KEY`; // Your API URL here
  
    try {
      const response = await fetch(apiUrl);
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching AQI data:', error);
      return null; // Return null in case of an error
    }
  }

module.exports = { addAQIMarker, fetchAQIData };
