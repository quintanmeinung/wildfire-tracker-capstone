document.addEventListener("DOMContentLoaded", function () {
    // Get the profile link
    var profileLink = document.getElementById("manage");
    console.log("profileLink:", profileLink); // Debug profile link

    // Get the profile modal
    var modalElement = document.getElementById('profileModal');
    console.log("modalElement:", modalElement); // Debug modal element

    var profileModal = new bootstrap.Modal(modalElement);
    console.log("Bootstrap modal initialized");

    // Add click event listener to the profile link
    if (profileLink) {
        profileLink.addEventListener("click", function (e) {
            e.preventDefault(); // Prevent default link behavior
            console.log("Profile link clicked");
            profileModal.show(); // Show the modal
        });
    }
});
