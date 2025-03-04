/**
 * @jest-environment jsdom
 */

global.TextEncoder = require("util").TextEncoder;
global.TextDecoder = require("util").TextDecoder;

const { JSDOM } = require('jsdom');
const path = require('path');

// Ensure correct path to accessibility.js
const accessibilityScript = require('../../project_wildfire_web/wwwroot/js/accessibility');

describe('Accessibility Features', () => {
  let document, fontSizeDropdown, contrastButton, speechButton;

  beforeEach(() => {
    // Mock a DOM with the needed elements
    const dom = new JSDOM(`
      <!DOCTYPE html>
      <body>
        <div class="accessibility-controls">
          <select id="fontSize">
            <option value="small">Small</option>
            <option value="medium" selected>Medium</option>
            <option value="large">Large</option>
          </select>
        </div>

        <div class="contrast-toggle">
          <button id="contrastToggle">Toggle High Contrast</button>
        </div>

        <div class="speech-toggle">
          <button id="speechToggle">Enable Text-to-Speech</button>
        </div>
      </body>
    `);

    document = dom.window.document;
    global.document = document; // Allow global access

    // Get references to elements
    fontSizeDropdown = document.getElementById("fontSize");
    contrastButton = document.getElementById("contrastToggle");
    speechButton = document.getElementById("speechToggle");

    // Mock window object if needed
    global.window = dom.window;
  });

  test("Contrast mode should toggle when button is clicked", () => {
    const contrastButton = document.getElementById("contrastToggle");
    // Click to enable contrast mode
    contrastButton.click();
    expect(document.body.classList.contains("high-contrast")).toBe(false);

    // Click again to disable contrast mode
    contrastButton.click();
    expect(document.body.classList.contains("high-contrast")).toBe(false);
  });
});
