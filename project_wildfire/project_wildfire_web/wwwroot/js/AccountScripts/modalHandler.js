import { initSavedLocationEditing } from "./updateLocation";

document.addEventListener("DOMContentLoaded", function () {
    const profileLink = document.getElementById("manage");
    const modalElement = document.getElementById('profileModal');
    
    if (!modalElement) {
        console.error('Profile modal element not found');
        return;
    }

    const profileModal = new bootstrap.Modal(modalElement);

    if (profileLink) {
        profileLink.addEventListener("click", function (e) {
            e.preventDefault();
            profileModal.show();
            
            // Cleaner event handling with named function
            const handleModalShown = () => {
                try {
                    initSavedLocationEditing();
                    modalElement.removeEventListener('shown.bs.modal', handleModalShown);
                } catch (error) {
                    console.error('Error initializing location editing:', error);
                }
            };
            
            modalElement.addEventListener('shown.bs.modal', handleModalShown);
        });
    }
});