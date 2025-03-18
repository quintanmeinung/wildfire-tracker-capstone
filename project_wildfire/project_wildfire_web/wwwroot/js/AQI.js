async function addAQIMarker(map, stationId) {
    const apiUrl = `/api/aqi/get-aqi-data?stationId=${stationId}`;

    try {
        const response = await fetch(apiUrl);
        const data = await response.json();

        if (response.ok) {
            const marker = L.circleMarker([data.lat, data.lon], {
                radius: 10,
                color: data.color,
                fillColor: data.color,
                fillOpacity: 0.7
            });

            // Format attributions as clickable links
            let attributions = "";
            if (data.attributions && data.attributions.length) {
                attributions = data.attributions.map(attr =>
                    `<a href="${attr.url}" target="_blank" rel="noopener noreferrer">${attr.name}</a>`
                ).join(', ');
            } else {
                attributions = "N/A";
            }

            // Create the popup content
            const popupContent = `
                <strong>${data.location}</strong><br>
                AQI: ${data.aqi} (${data.dominantPollutant || "N/A"})<br>
                PM2.5: ${data.pm25 !== null ? `${data.pm25} µg/m³` : "N/A"}<br>
                Dominant Pollutant: ${data.dominantPollutant || "N/A"}<br>
                Last Updated: ${data.lastUpdated ? new Date(data.lastUpdated).toLocaleString() : "N/A"}<br>
                Attributions: ${attributions}<br>
            `;

            // Bind the popup with the widget and additional details
            marker.bindPopup(popupContent);

            // Add the marker to the map
            marker.addTo(map);
        } else {
            console.error("Failed to fetch AQI data:", data);
        }
    } catch (error) {
        console.error("Error fetching AQI data:", error);
    }
}

export { addAQIMarker };
