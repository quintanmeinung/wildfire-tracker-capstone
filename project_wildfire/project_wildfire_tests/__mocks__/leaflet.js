module.exports = {
    circleMarker: jest.fn(() => ({
        bindPopup: jest.fn().mockReturnThis(),
        addTo: jest.fn().mockReturnThis(),
    })),
};


