// Custom fire icon
const fireIcon = L.icon({
    iconUrl: 'https://cdn-icons-png.flaticon.com/512/483/483361.png',
    iconSize: [32, 32],
    iconAnchor: [16, 32],
    popupAnchor: [0, -28],
  });
  
  // Placeholder fire locations in Oregon
  const fireLocations = [
    { lat: 45.5051, lng: -122.6750, city: 'Portland' },
    { lat: 44.0521, lng: -123.0868, city: 'Eugene' },
    { lat: 44.9429, lng: -123.0351, city: 'Salem' },
    { lat: 43.8041, lng: -120.5542, city: 'Bend' },
    { lat: 42.3265, lng: -122.8756, city: 'Medford' },
    { lat: 45.6896, lng: -118.8408, city: 'Pendleton' },
  ];
  
    // Function to add fire markers to a given Leaflet layer
    export function addFireMarkers(fireLayer) {
      fireLocations.forEach(location => {
        L.marker([location.lat, location.lng], { icon: fireIcon })
           .bindPopup(`🔥 Fire reported near ${location.city}`)
           .addTo(fireLayer);
    });
  }