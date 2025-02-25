document.addEventListener("DOMContentLoaded", function() {
    // Font size adjustment
    const fontSizeDropdown = document.getElementById("fontSize");
    if (fontSizeDropdown) {
        fontSizeDropdown.addEventListener("change", function() {
            const fontSize = this.value;
            document.body.classList.remove("font-small", "font-medium", "font-large", "font-xlarge");
            if (fontSize === "small") {
                document.body.classList.add("font-small");
            } else if (fontSize === "medium") {
                document.body.classList.add("font-medium");
            } else if (fontSize === "large") {
                document.body.classList.add("font-large");
            } else if (fontSize === "xlarge") {
                document.body.classList.add("font-xlarge");
            }
        });
    }

    // Contrast mode toggle
    const contrastButton = document.getElementById("contrastToggle");
    if (contrastButton) {
        contrastButton.addEventListener("click", function() {
            document.body.classList.toggle("high-contrast");
        });
    }

    // Text-to-speech feature
    const speechButton = document.getElementById("speechToggle");
    if (speechButton) {
        speechButton.addEventListener("click", function() {
            const textToRead = document.body.innerText;
            const speech = new SpeechSynthesisUtterance(textToRead);
            speech.lang = "en-US";
            window.speechSynthesis.speak(speech);
        });
    }
});
