import { deleteLocation } from './profileFetch.js';
// For javascript associated with the profile page
// specifically the saved locations section

export function initSavedLocations() {
    document.addEventListener('click', function(e) {
        if (e.target.matches('.edit-btn, .save-btn, .cancel-btn, .delete-btn, .view-btn')) {
            const row = e.target.closest('.row.my-1');
            if (e.target.matches('.edit-btn')) {
                editButton(e);
            } else if (e.target.matches('.save-btn')) {
                saveLocationUpdates(row);
            } else if (e.target.matches('.cancel-btn')) {
                cancelEdit(row);
            } else if (e.target.matches('.delete-btn')) {
                deleteButton(e);
            } else if (e.target.matches('.view-btn')) {
                viewLocation(e);
            }
        }
    });
}

function viewLocation(e) {
    console.log('Viewing location...');
    const row = e.target.closest('.row.my-1');
    const location = JSON.parse(row.dataset.location);

    const lat = location.Latitude;
    const lng = location.Longitude;
    const radius = location.Radius;

    const modal = bootstrap.Modal.getInstance(document.getElementById('profileModal'));
    modal.hide();

    window._leaflet_map.setView([lat, lng], 12);
}

function editButton(e) {
    console.log('Editing location...');
    toggleButtons(e);
    toggleEditing(e);
}

function toggleEditing(e) {
    const row = e.target.closest('.row.my-1');
    const titleInput = row.querySelector('input[name="title"]');
    const addressInput = row.querySelector('input[name="address"]');
    const radiusInput = row.querySelector('input[name="radius"]');
    
    // Determine edit mode based on the presence of the 'editing' class on the row
    const isEditMode = !row.classList.contains('editing');
    
    // Toggle readonly and styling
    titleInput.readOnly = !isEditMode;
    addressInput.readOnly = !isEditMode;
    radiusInput.disabled = !isEditMode;
    
    // Toggle input styling
    if (isEditMode) {
        titleInput.classList.replace('form-control-plaintext', 'form-control');
        addressInput.classList.replace('form-control-plaintext', 'form-control');
        radiusInput.classList.replace('form-control-plaintext', 'form-control');
        row.classList.add('editing');
    } else {
        titleInput.classList.replace('form-control', 'form-control-plaintext');
        addressInput.classList.replace('form-control', 'form-control-plaintext');
        radiusInput.classList.replace('form-control', 'form-control-plaintext');
        row.classList.remove('editing');
    }
}

function saveLocationUpdates(row) {

    const location = JSON.parse(row.dataset.location);

    // Get current values
    const title = row.querySelector('input[name="title"]').value;
    const address = row.querySelector('input[name="address"]').value;
    const radius = row.querySelector('input[name="radius"]').value;

    location.Title = title;
    location.Address = address;
    location.Radius = radius;

    // Save changes in the data attribute so if user cancels, it reverts properly
    row.dataset.location = JSON.stringify(location);
    
    // Construct the updated location object
    console.log('Saving:', { title, address, radius });
    // UserLocationDTO
    const updatedDTO = {
        Id: location.Id,        // These don't change;
        UserId: location.UserId,// But, are required for the API
        Title: title,           // Only title, address, and radius change
        Address: address,
        Latitude: location.Latitude,  // Same here
        Longitude: location.Longitude,// And here
        Radius: radius
    };

    // fetch update location endpoint
    fetch('/api/Location/UpdateLocation', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatedDTO)
    }).then(response => {
        if (!response.ok) {
            throw new Error('API UpdateLocation response was not ok: ' + response.statusText);
        } else {
            console.log('Location updated successfully!');
            updateMarker(location.Id, title, radius); // Updates the map marker with the new title and radius
        }
    }).catch(error => {
        console.error('Error updating location:', error);
    });
    
    // Exit edit mode
    toggleEditing({ target: row.querySelector('.save-btn') });
    toggleButtons({ target: row.querySelector('.save-btn') });
}

function updateMarker(locationId, title, radius) {
    console.log('Updating marker for locationId:', locationId);
    const markers = window.savedLocationMarkers;
    var target = markers[locationId]
    if (target) {
        target.remove();
        target.Title = title; // Update the title
        target.Radius = radius; // Update the radius
        target.bindPopup(title);
        target.addTo(window._leaflet_map);
    } else {
        console.error('Marker not found for locationId:', locationId);
    }
}

function cancelEdit(row) {
    // Reset to original values
    const location = JSON.parse(row.dataset.location);
    row.querySelector('input[name="title"]').value = location.Title;
    row.querySelector('input[name="address"]').value = location.Address;
    row.querySelector('input[name="radius"]').value = location.Radius;
    
    // Exit edit mode
    toggleEditing({ target: row.querySelector('.cancel-btn') });
    toggleButtons({ target: row.querySelector('.cancel-btn') });
}

function deleteButton(e) {
    console.log('Deleting location...');
    const row = e.target.closest('.row.my-1');
    const location = JSON.parse(row.dataset.location);

    deleteLocation(location.Id).then(() => {
        row.remove();

        const remainingLocations = document.querySelectorAll('.row.my-1');
        if (remainingLocations.length === 0) {
            // Display "No saved locations" message
            const locationsContainer = document.getElementById('location-container'); // Replace with your container ID
            locationsContainer.innerHTML = '<p class="text-muted">No saved locations</p>';
        }

    }).catch(error => {
        console.error('Error deleting location:', error);
    });
}

function toggleButtons(e) {
    const row = e.target.closest('.row.my-1');
    if (!row) return;

    // Get buttons relative to the row
    const viewBtn = row.querySelector('.view-btn');
    const saveBtn = row.querySelector('.save-btn');
    const editBtn = row.querySelector('.edit-btn');
    const cancelBtn = row.querySelector('.cancel-btn');
    const deleteBtn = row.querySelector('.delete-btn');

    // Null check all buttons
    if (!viewBtn || !saveBtn || !editBtn || !cancelBtn || !deleteBtn) {
        console.error('Missing buttons in row:', row);
        return;
    }

    const isEditMode = e.target.classList.contains('edit-btn');

    // Toggle visibility
    viewBtn.classList.toggle('d-none', isEditMode);
    saveBtn.classList.toggle('d-none', !isEditMode);
    editBtn.classList.toggle('d-none', isEditMode);
    cancelBtn.classList.toggle('d-none', !isEditMode);
    
    // Toggle delete button state
    deleteBtn.classList.toggle('disabled', !isEditMode);

    // Toggle button styles
    cancelBtn.classList.toggle('btn-outline-secondary', !isEditMode);
    cancelBtn.classList.toggle('btn-secondary', isEditMode);
    
    deleteBtn.classList.toggle('btn-outline-danger', !isEditMode);
    deleteBtn.classList.toggle('btn-danger', isEditMode);
}