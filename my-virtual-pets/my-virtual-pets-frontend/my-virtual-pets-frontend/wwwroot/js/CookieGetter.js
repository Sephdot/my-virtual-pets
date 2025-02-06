function getCookie() {
    let match = document.cookie.match(new RegExp('(^| )' + 'my_virtual_pets_api' + '=([^;]+)'));
    if (match) return match[2];
    return null;
}