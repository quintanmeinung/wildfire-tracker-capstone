document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('update-email-form').addEventListener('submit', async (e) => {
        e.preventDefault();

        const emailInput = document.getElementById('newEmail');
        const newEmail = emailInput.value.trim();

        if (!validateEmail(newEmail)) {
            alert('Invalid email address.');
            return;
        }

        try {
            const response = await fetch('/api/User/UpdateEmail', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ newEmail }),
            });

            if (response.ok) {
                // Display a callout or notification for success
                const successMessage = document.createElement('div');
                successMessage.className = 'alert alert-success';
                successMessage.id = 'email-update-success';
                successMessage.textContent = 'Your email has been updated successfully!';
                document.body.prepend(successMessage);

                // Remove the notification after 5 seconds
                setTimeout(() => successMessage.remove(), 10000);
            } else {
                let errorMessage = 'Unknown error';
                try {
                    const error = await response.json();
                    errorMessage = error.message || errorMessage;
                } catch (jsonError) {
                    console.error('Failed to parse error response:', jsonError);
                }
                alert('Failed to update email: ' + errorMessage);
            }
        } catch (error) {
            console.error('Error updating email:', error);
            alert('An error occurred while updating the email. Please try again.');
        }
    });

    function validateEmail(email) {
        return /(.+)@(.+){2,}\.(.+){2,}/.test(email);
    }
});