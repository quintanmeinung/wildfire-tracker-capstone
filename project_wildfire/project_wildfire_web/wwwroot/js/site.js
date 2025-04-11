// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// By using let we can expose the variable to the global scope
let userId;

// When the DOM is fully loaded, we can access the userId from the body element
document.addEventListener("DOMContentLoaded", function () {
    userId = document.body.dataset.id;
});

// This function can be called from other scripts to get the userId
export function getUserId() {
    console.log("Sending  User ID:", userId);
    return userId;
}

export function isLoggedIn() {
    if (userId !== "") {
        return true;
    }
    return false;
}