export function addFireMarkers(fireLayer, apiData) {
  apiData.forEach(fire => {
    const circle = L.circle([fire.latitude, fire.longitude], {
      color: 'red',
      fillColor: '#f03',
      fillOpacity: 0.5,
      radius: 500
    }).bindPopup(`🔥 Radiative Power: ${fire.radiativePower}`);

    circle.addTo(fireLayer);

    // Try polling until the element appears, then modify it
    /*
    const tryAttachAttributes = () => {
      const el = circle.getElement();
      if (el) {
        el.classList.add("wildfire-marker");
        el.setAttribute("data-lat", fire.latitude);
        el.setAttribute("data-lon", fire.longitude);
        console.log(`✅ Attached wildfire-marker to (${fire.latitude}, ${fire.longitude})`);
      } else {
        console.warn("⏳ Waiting for marker DOM element...");
        setTimeout(tryAttachAttributes, 100);
      }
    };

    tryAttachAttributes();*/
  });
}

