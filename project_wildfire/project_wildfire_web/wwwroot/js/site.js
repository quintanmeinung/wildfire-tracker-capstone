// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Exposes the userId from an invisible div in _Layout.cshtml
const userDataDiv = document.getElementById('user-data');
// If the user is not logged in this will be null
export const userId = userDataDiv.dataset.userId;