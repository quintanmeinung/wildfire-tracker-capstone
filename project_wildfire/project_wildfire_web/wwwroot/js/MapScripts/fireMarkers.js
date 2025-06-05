
export function addFireMarkers(fireLayer, apiData) {
  apiData.forEach(fire => {
    const power = fire.radiativePower;

    // Determine color based on radiative power
    let color = '';
    if (power < 10) {
      color = 'yellow'; // small fires
    } else if (power < 30) {
      color = 'orange'; // moderate fires
    } else {
      color = 'red'; // strong fires
    }

    // Determine radius based on radiative power
    let radius = 5; // default small
    if (power >= 10 && power < 30) {
      radius = 7; // medium
    } else if (power >= 30) {
      radius = 9; // large
    }

    const marker = L.circleMarker([fire.latitude, fire.longitude], {
      radius: radius,
      fillColor: color,
      color: color,
      weight: 1,
      opacity: 1,
      fillOpacity: 0.8,
      className: "wildfire-marker"
  });

  // Build popup content
  let popupContent = `
  <strong> Wildfire!</strong><br>
  <strong>Radiative Power:</strong> ${power}<br>
  <strong>Latitude:</strong> ${fire.latitude.toFixed(5)}<br>
  <strong>Longitude:</strong> ${fire.longitude.toFixed(5)}
`;

// Show delete button *only* if it's an admin fire and the user is an admin
if (fire.isAdminFire && window.isAdmin) {
  popupContent += `
    <br><em>Placed by admin</em><br>
    <button class="delete-admin-fire btn btn-sm btn-danger" data-fire-id="${fire.fireId}">
      Delete Fire
    </button>
  `;
}

// Show subscribe button for *everyone*, for *any fire*
popupContent += `
  <br><button class="subscribe-btn" data-fire-id="${fire.fireId}">
    Subscribe to fire
  </button>
`;


  marker.bindPopup(popupContent);


       
    
    window.fireMarkerMap = window.fireMarkerMap || new Map();
    window.fireMarkerMap.set(fire.fireId, marker);

    marker.addTo(fireLayer);
    bindSubscribeButtonOnPopup(marker, fire.fireId);
  });
}

export function bindSubscribeButtonOnPopup(marker, fireId) {
  marker.on('popupopen', () => {
     const popupEl = marker.getPopup().getElement(); // Get the actual DOM element of the popup
      const btn = popupEl?.querySelector('.subscribe-btn'); // Look inside *that popup only*
    if (btn) {
      btn.addEventListener('click', async () => {
        try {
          const response = await fetch(`/api/fireSub/${fireId}`,{
            method: 'POST'
          });

          if (!response.ok) {
            throw new Error('Subscription failed.');
          }

          const result = await response.json();
          alert(result.message || "Subscribed successfully!");
        } catch (err) {
          alert("Error: " + err.message);
        }
      });
    }
  });
}



