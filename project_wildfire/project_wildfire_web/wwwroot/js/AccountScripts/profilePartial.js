// Edit button calls this function
document.getElementById('nameEditSaveButton').addEventListener('click', () => toggleEditName());
document.getElementById('emailEditSaveButton').addEventListener('click', () => toggleEditEmail());
document.getElementById('phoneEditSaveButton').addEventListener('click', () => toggleEditPhone());

async function toggleEditName() {
    // Get the profile data from the HTML element
    const profileData = JSON.parse(document.getElementById('profile-data').dataset.profile);
    const lastNameInput = document.getElementById('lastNameInput');
    const firstNameInput = document.getElementById('firstNameInput');
    const editSaveButton = document.getElementById('nameEditSaveButton');

    // If user is not currently editing, allow them to edit
    if (lastNameInput.readOnly && firstNameInput.readOnly) {
        lastNameInput.readOnly = false;
        firstNameInput.readOnly = false;
        editSaveButton.textContent = 'Save';
        return;
    }

    // If user is currently editing, save the changes
    try {
        // Update the profile data
        profileData.FirstName = firstNameInput.value.trim();
        profileData.LastName = lastNameInput.value.trim();

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
        lastNameInput.setAttribute('readonly', true);
        firstNameInput.readOnly = false;
        firstNameInput.setAttribute('readonly', true);
        editSaveButton.textContent = 'save';
    }
}

async function toggleEditEmail() {
    // Get the profile data from the HTML element
    const profileData = JSON.parse(document.getElementById('profile-data').dataset.profile);
    const input = document.getElementById('emailInput');
    const editSaveButton = document.getElementById('emailEditSaveButton');

    // If user is not currently editing, allow them to edit
    if (input.readOnly) {
        input.readOnly = false;
        editSaveButton.textContent = 'save';
        return;
    }

    // If user is currently editing, save the changes
    try {
        // Validate input values
        if (!input.value.trim()) {
            alert('Email cannot be empty.');
            return;
        }

        // Update the profile data
        profileData.Email = input.value.trim();

        // Save the changes
        await saveEdits(profileData);

        // Disable editing and update the button text
        input.readOnly = true;
        editSaveButton.textContent = 'Edit';
    } catch (error) {
        console.error('Error saving edits:', error);
        alert('Failed to save changes. Please try again.');

        // Re-enable editing if the save operation fails
        input.readOnly = false;
        editSaveButton.textContent = 'Save';
    }
}

async function toggleEditPhone() {
    // Get the profile data from the HTML element
    const profileData = JSON.parse(document.getElementById('profile-data').dataset.profile);
    const input = document.getElementById('emailInput');
    const editSaveButton = document.getElementById('phoneEditSaveButton');

    // If user is not currently editing, allow them to edit
    if (input.readOnly) {
        input.readOnly = false;
        editSaveButton.textContent = 'Save';
        return;
    }

    // If user is currently editing, save the changes
    try {
        // Validate input values
        if (!input.value.trim()) {
            alert('Email cannot be empty.');
            return;
        }

        // Update the profile data
        profileData.Email = input.value.trim();

        // Save the changes
        await saveEdits(profileData);

        // Disable editing and update the button text
        input.readOnly = true;
        editSaveButton.textContent = 'Edit';
    } catch (error) {
        console.error('Error saving edits:', error);
        alert('Failed to save changes. Please try again.');

        // Re-enable editing if the save operation fails
        input.readOnly = false;
        editSaveButton.textContent = 'Save';
    }
}

// Each function uses this to save changes to the user's profile
async function saveEdits(profileData) {
    
    try {
        // Send the data to the backend
        const response = await fetch('/api/users/SaveModalEdits', {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(profileData)
        });

        if (response.ok) {
          alert('Saved successfully!');
        } else {
          const error = await response.json();
          alert('Failed to save: ' + error);
        }

    } catch (error) {
        console.error('Error:', error);
        alert('An error occurred while saving.');
    }
}