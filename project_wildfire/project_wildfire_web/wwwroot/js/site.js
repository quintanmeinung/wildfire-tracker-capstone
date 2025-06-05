let userId;

document.addEventListener("DOMContentLoaded", function () {
    userId = document.body.dataset.id;
});

export function getUserId() {
    return userId;
}