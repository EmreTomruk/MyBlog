function convertFirstLetterToUpperCase(text) {
    return text.charAt(0).toUpperCase() + text.slice(1); //slice(1): 1. indexten itibaren kalan kelimeleri al(fonksiyonla 'true -> True' yapilir)...
}

function convertToShortDate(dateString) { //DateTime veriler Json'dan Parse edildiginde string olarak gelir...
    const shortDate = new Date(dateString).toLocaleDateString('en-US');

    return shortDate;
}