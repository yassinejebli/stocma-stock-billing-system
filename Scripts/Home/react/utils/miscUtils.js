export function looseFocus() {
    window.focus();
    if (document.activeElement) {
        document.activeElement.blur();
    }
}