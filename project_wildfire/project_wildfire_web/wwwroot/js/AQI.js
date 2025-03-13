// Function to add AQI markers to the map
async function addAQIMarker(map, stationId) {
    const apiUrl = `/api/aqi/get-aqi-data?stationId=${stationId}`;

    try {
        const response = await fetch(apiUrl);
        const data = await response.json();

        if (response.ok) {
            L.circleMarker([data.lat, data.lon], {
                radius: 10,
                color: data.color,
                fillColor: data.color,
                fillOpacity: 0.7
            })
            .bindPopup(`<strong>${data.location}</strong><br>AQI: ${data.aqi}`)
            .addTo(map);
        } else {
            console.error("Failed to fetch AQI data:", data);
        }
    } catch (error) {
        console.error("Error fetching AQI data:", error);
    }
}
export {addAQIMarker};