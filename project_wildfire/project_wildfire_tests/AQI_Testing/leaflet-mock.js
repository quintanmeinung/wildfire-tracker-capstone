const L = {
    map: jest.fn(() => ({
        setView: jest.fn().mockReturnThis(),
        addLayer: jest.fn(),
        removeLayer: jest.fn(),
    })),
    tileLayer: jest.fn(() => ({
        addTo: jest.fn()
    })),
    layerGroup: jest.fn(() => ({
        addLayer: jest.fn(),
        removeLayer: jest.fn()
    })),
    circleMarker: jest.fn(() => {
        const marker = {
            bindPopup: jest.fn().mockReturnThis(),
            addTo: jest.fn()
        };
        return marker;
    }),
    marker: jest.fn(() => ({
        bindPopup: jest.fn().mockReturnThis(),
        addTo: jest.fn()
    }))
};

module.exports = L;
