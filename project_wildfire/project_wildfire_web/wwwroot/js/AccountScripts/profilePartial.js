// Edit button calls this function
document.getElementById('editButton').addEventListener('click', () => toggleEdit());

async function toggleEdit() {
    // Get the profile data from the HTML element
    const profileElement = document.getElementById('profile-data');
    if (!profileElement) {
        console.error('Profile data element not found.');
        return;
    }
    console.log(profileElement.dataset.profile);
    const profileData = JSON.parse(profileElement.dataset.profile);

    // Get all fields from the form
    const lastNameInput = document.getElementById('lastNameInput');
    const firstNameInput = document.getElementById('firstNameInput');
    const emailInput = document.getElementById('emailInput');
    const phoneInput = document.getElementById('phoneInput');

    console.log('profileData:', profileData);

    // If user is not currently editing, allow them to edit
    if (lastNameInput.readOnly) {
        console.log('Editing profile...');
        lastNameInput.readOnly = false;
        firstNameInput.readOnly = false;
        emailInput.readOnly = false;
        phoneInput.readOnly = false;
        editButton.textContent = 'Save';
        return;
    }

    // If user is currently editing, save the changes
    try {
        console.log('Saving profile changes...');
        editButton.textContent = 'Saving...';
        // Update the profile data
        profileData.FirstName = firstNameInput.value.trim();
        profileData.LastName = lastNameInput.value.trim();

        if (validateEmail(emailInput.value.trim())) {
            profileData.Email = emailInput.value.trim();
        } else {
            throw new Error('Invalid email address.');
        }

        if (validatePhone(phoneInput.value.trim())) {
            profileData.PhoneNumber = phoneInput.value.trim();
        } else {
            throw new Error('Invalid phone number.');
        }

        // Save the changes
        await saveEdits(profileData);

        // Disable editing and update the button text
        lastNameInput.readOnly = true;
        lastNameInput.removeAttribute('readonly');
        firstNameInput.readOnly = true;
        firstNameInput.removeAttribute('readonly');

        // If both are empty, then button says add
        if (!firstNameInput.value.trim() && !lastNameInput.value.trim()) {
            editSaveButton.textContent = 'add';
        } 
        else 
        {
            editSaveButton.textContent = 'edit';
        }
        
    } catch (error) {
        console.error('Error saving edits:', error);
        alert('Failed to save changes. Please try again.');

        // Re-enable editing if the save operation fails
        lastNameInput.readOnly = false;
        firstNameInput.readOnly = false;
        emailInput.readOnly = false;
        phoneInput.readOnly = false;
        editSaveButton.textContent = 'save';
    }
}

function validateEmail(email) {
    
    if( /(.+)@(.+){2,}\.(.+){2,}/.test(email) )
    {
        return true;
    }
    return false;
}