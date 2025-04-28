document.getElementById('update-phone-form').addEventListener('submit', async (e) => {
    e.preventDefault();

    const phoneInput = document.getElementById('PhoneNumber');
    const newPhoneNumber = phoneInput.value.trim();

    if (!validatePhoneNumber(newPhoneNumber)) {
        alert('Invalid phone number.');
        return;
    }

    try {
        const response = await fetch('/api/User/UpdatePhoneNumber', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newPhoneNumber),
        });

        if (response.ok) {
            alert('Phone number updated successfully!');
        } else {
            const error = await response.json();
            alert('Failed to update phone number: ' + error.message);
        }
    } catch (error) {
        console.error('Error updating phone number:', error);
        alert('An error occurred while updating the phone number. Please try again.');
    }
});

function validatePhoneNumber(phoneNumber) {
    // Basic validation for phone number format
    return /^[0-9]{10}$/.test(phoneNumber);
}