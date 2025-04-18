const L = require('./leaflet-mock');
const { fetchAQIData, addAQIMarker } = require('./AQI-Testing');

describe('AQI Layer Tests', () => {
    let map;
    let aqiLayer;
    let mockMarker;

    beforeEach(() => {
        map = L.map();
        aqiLayer = L.layerGroup();

        // Create a consistent mock marker
        mockMarker = {
            bindPopup: jest.fn().mockReturnThis(),
            addTo: jest.fn()
        };
        L.circleMarker.mockReturnValue(mockMarker);
    });

    test('should create AQI layer and add markers', () => {
        addAQIMarker(aqiLayer, {
            lat: 44.97643,
            lon: -122.97647,
            aqi: 42,
            color: '#00E400',
            location: 'Salem Chemeketa Community College',
            dominantPollutant: 'pm25',
            pm25: 7,
            lastUpdated: '2025-03-18T00:00:00Z',
            attributions: [{ url: 'https://www.oregon.gov/DEQ/', name: 'Oregon DEQ' }]
        });

        expect(aqiLayer.addLayer).toHaveBeenCalledWith(mockMarker);
        expect(L.circleMarker).toHaveBeenCalledTimes(1);
    });

    test('should toggle AQI layer on and off', () => {
        const overlays = { 'AQI Stations': aqiLayer };
        map.addLayer(overlays['AQI Stations']);
        expect(map.addLayer).toHaveBeenCalledWith(overlays['AQI Stations']);

        map.removeLayer(overlays['AQI Stations']);
        expect(map.removeLayer).toHaveBeenCalledWith(overlays['AQI Stations']);
    });

    test('should bind popup to AQI marker', () => {
        addAQIMarker(aqiLayer, {
            lat: 44.97643,
            lon: -122.97647,
            aqi: 42,
            color: '#00E400',
            location: 'Salem Chemeketa Community College',
            dominantPollutant: 'pm25',
            pm25: 7,
            lastUpdated: '2025-03-18T00:00:00Z',
            attributions: [{ url: 'https://www.oregon.gov/DEQ/', name: 'Oregon DEQ' }]
        });

        expect(mockMarker.bindPopup).toHaveBeenCalled();
    });
});

/*Test's above this are for feature F10*/


global.fetch = jest.fn(() =>
    Promise.resolve({
      json: () => Promise.resolve({ data: 'mockData' }), // Mock the successful fetch response
    })
  );
  
  describe('fetchAQIData Tests', () => {
    it('should call the correct API URL with correct parameters', async () => {
      const lat = 44.9429; // Example latitude
      const lon = -123.0351; // Example longitude
  
      const apiUrl = `https://api.airquality.com/data?lat=${lat}&lon=${lon}&key=YOUR_API_KEY`; // Adjust this to your API URL
  
      // Call the function with test parameters
      await fetchAQIData(lat, lon); 
  
      // Ensure fetch was called with the correct URL
      expect(fetch).toHaveBeenCalledWith(apiUrl);
    });
  
    // Additional tests for fetchAQIData can go here (e.g., successful response, failed response)
  });