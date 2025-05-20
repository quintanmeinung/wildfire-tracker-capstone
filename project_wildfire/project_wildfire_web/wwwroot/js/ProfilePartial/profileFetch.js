export async function deleteLocation(locationId) {
    try {
        const response = await fetch('/api/Location/DeleteLocation', {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(locationId)
        });

        if (!response.ok) {
            console.error('Delete Location Response:', response);
            throw new Error('Network response was not ok' + response.statusText);
        }

        const result = await response.json();
        window.savedLocationMarkers[locationId].remove(); // Remove the marker from the map
        window.savedLocationMarkers[locationId] = null; // Remove the marker from the saved markers
        console.log('Location deleted successfully:', result);
    } catch (error) {
        console.error('Error deleting location:', error);
    }
}