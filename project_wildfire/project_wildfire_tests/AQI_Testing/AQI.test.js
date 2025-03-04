jest.mock('leaflet');  // This will use __mocks__/leaflet.js automatically
const L = require('leaflet');  // Ensure the mocked version of Leaflet is used


const { fetchAQIData, getAQIColor, addAQIMarker } = require('../../project_wildfire_web/wwwroot/js/AQI.js');

// Mock global fetch for fetchAQIData tests
global.fetch = jest.fn();

// Move this up so it's available to all tests
const mockResponse = {
    status: "ok",
    data: {
        aqi: 42,
        city: {
            geo: [44.9429, -123.0351],
            name: "Salem Chemeketa Community College"
        }
    }
};

describe('fetchAQIData', () => {
    const mockStationId = "A503596";

    afterEach(() => {
        jest.clearAllMocks();
    });

    it('calls correct API URL', async () => {
        fetch.mockResolvedValueOnce({
            json: jest.fn().mockResolvedValueOnce(mockResponse)
        });

        await fetchAQIData(mockStationId);

        expect(fetch).toHaveBeenCalledWith(
            `https://api.waqi.info/feed/${mockStationId}/?token=152b70cd77ae9f824be07461d6ca46df5ff8b7c3`
        );
    });

    it('returns formatted AQI data on success', async () => {
        fetch.mockResolvedValueOnce({
            json: jest.fn().mockResolvedValueOnce(mockResponse)
        });

        const data = await fetchAQIData(mockStationId);

        expect(data).toEqual({
            aqi: 42,
            lat: 44.9429,
            lon: -123.0351,
            location: "Salem Chemeketa Community College"
        });
    });

    it('returns null if API response is not ok', async () => {
        fetch.mockResolvedValueOnce({
            json: jest.fn().mockResolvedValueOnce({ status: "error" })
        });

        const data = await fetchAQIData(mockStationId);
        expect(data).toBeNull();
    });

    it('returns null if fetch fails', async () => {
        fetch.mockRejectedValueOnce(new Error("Network error"));

        const data = await fetchAQIData(mockStationId);
        expect(data).toBeNull();
    });
});

describe('getAQIColor', () => {
    it('returns correct color for different AQI values', () => {
        expect(getAQIColor(30)).toBe('green');
        expect(getAQIColor(75)).toBe('yellow');
        expect(getAQIColor(125)).toBe('orange');
        expect(getAQIColor(180)).toBe('red');
        expect(getAQIColor(250)).toBe('purple');
        expect(getAQIColor(350)).toBe('maroon');
    });
});

describe('addAQIMarker', () => {
    const mockMap = {};  // Placeholder object for map
    const mockStationId = "A503596";

    beforeEach(() => {
        fetch.mockResolvedValueOnce({
            json: jest.fn().mockResolvedValueOnce(mockResponse)
        });
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    it('adds a circle marker to the map', async () => {
        await addAQIMarker(mockMap, mockStationId);

        // Check if circleMarker was called with correct arguments
        expect(L.circleMarker).toHaveBeenCalledWith([44.9429, -123.0351], expect.objectContaining({
            radius: 10,
            color: 'green',
            fillColor: 'green',
            fillOpacity: 0.7
        }));

        // Verify the marker's popup and addTo method were called
        const marker = L.circleMarker.mock.results[0].value;
        expect(marker.bindPopup).toHaveBeenCalledWith(
            `<strong>Salem Chemeketa Community College</strong><br>AQI: 42`
        );
        expect(marker.addTo).toHaveBeenCalledWith(mockMap);
    });
});
