// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

let userId;

document.addEventListener("DOMContentLoaded", function () {
    userId = document.body.dataset.id;
});

export function getUserId() {
    console.log("Sending  User ID:", userId);
    return userId;
}