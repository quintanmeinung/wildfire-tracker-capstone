export function initSavedLocationEditing() {
    // Edit button click handler
    handleEditButton();
    
    // Save button click handler
    handleSaveButton();
    
    // Delete button click handler
    handleDeleteButton();
};

function handleEditButton() {
    document.querySelectorAll('.edit-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            const row = this.closest('tr');
            
            // Enable editing mode
            row.classList.add('editing');
            row.querySelectorAll('.sl-input input').forEach(input => {
                input.readOnly = false;
                input.classList.add('editing');
            });
            
            // Toggle buttons
            this.classList.add('d-none');
            row.querySelector('.save-btn').classList.remove('d-none');
            row.querySelector('.delete-btn').classList.remove('d-none');
        });
    });
}

function handleSaveButton() {
    document.querySelectorAll('.save-btn').forEach(btn => {
        btn.addEventListener('click', async function() {
            const row = this.closest('tr');
            const locationId = row.dataset.id;
            
            // Collect updated data
            const updatedData = {
                Id: locationId,
                Title: document.getElementById(`sl-title-input-${locationId}`).value,
                Address: document.getElementById(`sl-address-input-${locationId}`).value,
                Latitude: document.getElementById(`sl-latitude-input-${locationId}`).value,
                Longitude: document.getElementById(`sl-longitude-input-${locationId}`).value
            };
            
            try {
                const response = await fetch('/Profile/UpdateLocation', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(updatedData)
                });
                
                if (response.ok) {
                    // Exit editing mode
                    row.classList.remove('editing');
                    row.querySelectorAll('.sl-input input').forEach(input => {
                        input.readOnly = true;
                        input.classList.remove('editing');
                    });
                    
                    // Toggle buttons
                    row.querySelector('.edit-btn').classList.remove('d-none');
                    this.classList.add('d-none');
                    row.querySelector('.delete-btn').classList.add('d-none');
                    
                    showToast('Location updated successfully!');
                } else {
                    throw new Error('Failed to update');
                }
            } catch (error) {
                console.error('Error:', error);
                showToast('Error updating location', 'error');
            }
        });
    });
}

function handleDeleteButton() {
    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', async function() {
            if (confirm('Are you sure you want to delete this location?')) {
                const row = this.closest('tr');
                const locationId = row.dataset.id;
                
                try {
                    const response = await fetch(`/Profile/DeleteLocation/${locationId}`, {
                        method: 'DELETE'
                    });
                    
                    if (response.ok) {
                        row.remove();
                        showToast('Location deleted successfully!');
                    } else {
                        throw new Error('Failed to delete');
                    }
                } catch (error) {
                    console.error('Error:', error);
                    showToast('Error deleting location', 'error');
                }
            }
        });
    });
}
