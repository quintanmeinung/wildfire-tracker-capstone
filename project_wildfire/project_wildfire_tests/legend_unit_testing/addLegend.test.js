
const { updateLegendInfo } = require('../../project_wildfire_web/wwwroot/js/addLegend'); 
const { JSDOM } = require('jsdom');

describe('updateLegendInfo function', () => {
  let document, legendInfoContent;

  beforeEach(() => {
    // Mock a DOM with the modal structure
    const dom = new JSDOM(`
      <!DOCTYPE html>
      <body>
        <div class="modal-body">
          <p id="legendInfoContent">Legend details will appear here.</p>
        </div>
      </body>
    `);

    document = dom.window.document;
    global.document = document;
    legendInfoContent = document.getElementById("legendInfoContent");
  });

  test('should update legend content correctly', () => {
    updateLegendInfo("This is the updated legend information.");

    expect(legendInfoContent.innerHTML).toBe("This is the updated legend information.");
  });

  test('should not throw an error if legendInfoContent is missing', () => {
    document.getElementById("legendInfoContent").remove(); // Remove element

    expect(() => updateLegendInfo("New Content")).not.toThrow();
  });

  test('should not throw an error if #legendInfoContent is missing', () => {
        const newDom = new JSDOM(`<!DOCTYPE html><body></body>`);
        const newDoc = newDom.window.document;
        expect(() => updateLegendInfo('Some Text', newDoc)).not.toThrow();
  });

  test('should not break when given null or undefined', () => {
        expect(() => updateLegendInfo(null, document)).not.toThrow();
        expect(() => updateLegendInfo(undefined, document)).not.toThrow();
  });

});
