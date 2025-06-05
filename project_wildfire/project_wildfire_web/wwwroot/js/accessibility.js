//Functionality to automatically save user preferences when they log out/in
document.addEventListener("DOMContentLoaded", function () {

    const prefs = window.userPreferences || {};

    // Font Size
    const fontSizeDropdown = document.getElementById("fontSize");
    switch (prefs.fontSize) {
        case "small":
            document.body.style.fontSize = "14px";
            if (fontSizeDropdown) fontSizeDropdown.value = "small";
            break;
        case "large":
            document.body.style.fontSize = "18px";
            if (fontSizeDropdown) fontSizeDropdown.value = "large";
            break;
        case "xlarge":
            document.body.style.fontSize = "22px";
            if (fontSizeDropdown) fontSizeDropdown.value = "xlarge";
            break;
        default:
            document.body.style.fontSize = "16px";
            if (fontSizeDropdown) fontSizeDropdown.value = "medium";
            break;
    }

    // Contrast
    if (prefs.contrastMode === true) {
        document.body.classList.add("high-contrast");
    }
});

document.addEventListener("DOMContentLoaded", function() {
    // Font Size Adjustment
    const fontSizeDropdown = document.getElementById("fontSize");
    if (fontSizeDropdown) {
        fontSizeDropdown.addEventListener("change", function() {
            document.body.style.fontSize = this.value === "small" ? "14px" :
                                           this.value === "medium" ? "16px" :
                                           this.value === "large" ? "18px" :
                                           this.value === "xlarge" ? "22px" : "16px";
        });
    }

    // High Contrast Mode Toggle
    const contrastButton = document.getElementById("contrastToggle");
    if (contrastButton) {
        contrastButton.addEventListener("click", function() {
            document.body.classList.toggle("high-contrast");
        });
    }

    // Text-to-Speech (Toggleable)
    let speech = new SpeechSynthesisUtterance();
    let isSpeaking = false;

    const speechButton = document.getElementById("speechToggle");
    if (speechButton) {
        speechButton.addEventListener("click", function() {
            if (!isSpeaking) {
                speech.text = document.body.innerText;
                speech.lang = "en-US";
                speech.rate = 1;
                window.speechSynthesis.speak(speech);
                isSpeaking = true;
                speechButton.textContent = "Stop Text-to-Speech";
            } else {
                window.speechSynthesis.cancel();
                isSpeaking = false;
                speechButton.textContent = "Enable Text-to-Speech";
            }
        });
    }
});

//Default Save Values for Accessibility
window.savePreferences = async function () {
    const fontSize = document.getElementById("fontSize")?.value || "default";
    const contrastMode = document.body.classList.contains("high-contrast");
    const textToSpeech = window.isSpeaking || false;
};

