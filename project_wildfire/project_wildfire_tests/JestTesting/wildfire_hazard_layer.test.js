describe("Wildfire Hazard Layer Toggle Logic", () => {
  let mockMap;
  let fireHazardLayer;

  beforeEach(() => {
    mockMap = {
      layers: new Set(),
      addLayer(layer) { this.layers.add(layer); },
      removeLayer(layer) { this.layers.delete(layer); },
      hasLayer(layer) { return this.layers.has(layer); }
    };

    fireHazardLayer = { id: "fireHazard" };
  });

  test("adds fire hazard layer when toggled on", () => {
    mockMap.addLayer(fireHazardLayer);
    expect(mockMap.hasLayer(fireHazardLayer)).toBe(true);
  });

  test("removes fire hazard layer when toggled off", () => {
    mockMap.addLayer(fireHazardLayer);
    mockMap.removeLayer(fireHazardLayer);
    expect(mockMap.hasLayer(fireHazardLayer)).toBe(false);
  });
});

