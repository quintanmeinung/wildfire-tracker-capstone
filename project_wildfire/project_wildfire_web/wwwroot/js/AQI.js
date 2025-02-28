// AQI.js

// Fetches AQI Data from API
export async function fetchAQIData(stationId) {
    const apiKey = "152b70cd77ae9f824be07461d6ca46df5ff8b7c3";
    const apiUrl = `https://api.waqi.info/feed/${stationId}/?token=${apiKey}`;

    try {
        const response = await fetch(apiUrl);
        const data = await response.json();

        if (data.status === "ok") {
            return {
                aqi: data.data.aqi,
                lat: data.data.city.geo[0],
                lon: data.data.city.geo[1],
                location: data.data.city.name || `Station ${stationId}`
            };
        } else {
            console.error("Failed to fetch AQI data:", data);
            return null;
        }
    } catch (error) {
        console.error("Error fetching AQI data:", error);
        return null;
    }
}

// Function to determine AQI color
export function getAQIColor(aqi) {
    return aqi <= 50 ? "green" :
           aqi <= 100 ? "yellow" :
           aqi <= 150 ? "orange" :
           aqi <= 200 ? "red" :
           aqi <= 300 ? "purple" : "maroon";
}

// Function to add AQI markers to the map
export async function addAQIMarker(map, stationId) {
    const aqiData = await fetchAQIData(stationId);

    if (aqiData) {
        L.circleMarker([aqiData.lat, aqiData.lon], {
            radius: 10,
            color: getAQIColor(aqiData.aqi),
            fillColor: getAQIColor(aqiData.aqi),
            fillOpacity: 0.7
        })
        .bindPopup(`<strong>${aqiData.location}</strong><br>AQI: ${aqiData.aqi}`)
        .addTo(map);
    }
}
