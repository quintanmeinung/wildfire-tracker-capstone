import { initSavedLocationEditing } from './updateLocation.js';

const modalElement = document.getElementById('profileModal');
if (!modalElement) {
    console.error('Profile modal element not found');
} else {
    const profileModal = new bootstrap.Modal(modalElement);
    
    // Handle modal shown event
    const handleModalShown = () => {
        try {
            initSavedLocationEditing();
        } catch (error) {
            console.error('Error initializing location editing:', error);
        }
    };
    
    modalElement.addEventListener('shown.bs.modal', handleModalShown);
    
    // Handle click event
    document.querySelector('#manage')?.addEventListener('click', function(e) {
        e.preventDefault();
        profileModal.show();
        console.log('Profile link clicked');
    });
}