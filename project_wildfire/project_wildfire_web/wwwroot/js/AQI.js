/* const L = require('leaflet'); //helps pass the test when mocking

 */
// Fetches AQI Data 
async function fetchAQIData(stationId) {
    const apiUrl = `/api/aqi/get-aqi-data?stationId=${stationId}`;

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
function getAQIColor(aqi) {
    return aqi <= 50 ? "green" :
           aqi <= 100 ? "yellow" :
           aqi <= 150 ? "orange" :
           aqi <= 200 ? "red" :
           aqi <= 300 ? "purple" : "maroon";
}

// Function to add AQI markers to the map
async function addAQIMarker(map, stationId) {
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

// This is the key change - CommonJS export
/* module.exports = {
    fetchAQIData,
    getAQIColor,
    addAQIMarker
}; */

export {fetchAQIData, getAQIColor, addAQIMarker};