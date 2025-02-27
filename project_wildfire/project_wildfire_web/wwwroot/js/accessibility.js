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
