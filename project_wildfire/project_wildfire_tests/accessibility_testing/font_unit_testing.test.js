const { adjustFontSize } = require('../../project_wildfire_web/wwwroot/js/accessibility');
const { JSDOM } = require('jsdom');

describe('adjustFontSize function', () => {
    let dom, document, body;

    beforeEach(() => {
        dom = new JSDOM(`<!DOCTYPE html><body style="font-size: 16px;"></body>`);
        document = dom.window.document;
        body = document.body;
    });

    test('should increase font size', () => {
        adjustFontSize(2, document);
        expect(body.style.fontSize).toBe('18px');
    });

    test('should decrease font size', () => {
        adjustFontSize(-2, document);
        expect(body.style.fontSize).toBe('14px');
    });

    test('should not exceed max font size (e.g., 32px)', () => {
        body.style.fontSize = '32px';
        adjustFontSize(2, document);
        expect(body.style.fontSize).toBe('32px');
    });

    test('should not go below min font size (e.g., 12px)', () => {
        body.style.fontSize = '12px';
        adjustFontSize(-2, document);
        expect(body.style.fontSize).toBe('12px');
    });
});
