export function looseFocus() {
    window.focus();
    if (document.activeElement) {
        document.activeElement.blur();
    }
}



function replacechars(c) {
    const frenchNumbersMap = {
        '&': '1',
        'é': '2',
        '"': '3',
        "'": '4',
        '(': '5',
        '-': '6',
        'è': '7',
        '_': '8',
        'ç': '9',
        'à': '0',
    }

    return frenchNumbersMap[c] || c;
}

export function convertLowercaseNumbersFR(chars) {
    return chars.split('').map(replacechars).join('');
}