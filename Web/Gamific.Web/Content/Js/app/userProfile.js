VMasker(document.getElementById('Phone')).maskPattern('(99) 99999 - 9999');
VMasker(document.getElementById('CPF')).maskPattern('99.999.999/9999-99');

function loadLogo(inputFile) {
    if (inputFile.files && inputFile.files[0]) {
        document.getElementById('img').src = '/api/media/0';
        document.getElementById('img').src = URL.createObjectURL(inputFile.files[0]);
        document.getElementById('img').style.display = 'block';
    }
}