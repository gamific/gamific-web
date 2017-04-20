function showEntityModal(editLink) {

    $('#entity-edit-modal').removeData("bs.modal");
    $('#entity-edit-modal').empty();
    $('#entity-edit-modal').load(editLink.href)
    $('#entity-edit-modal').modal('show');
}

function hideEntityModal() {

    $('#entity-edit-modal').removeData("bs.modal");
    $('#entity-edit-modal').empty();
    $('#entity-edit-modal').modal('hide');
}

function verifyErrors() {

    if (!hasErrors()) {
        location.reload();
    }
}

function hasErrors() {
    if (document.getElementById('validation-data') == null || document.getElementById('validation-data') == undefined) {
        return false;
    }

    var hasValidationErrors = document.getElementById('validation-data').classList[0];
    if (hasValidationErrors == null || hasValidationErrors == undefined || hasValidationErrors != 'validation-summary-errors') {
        return false;
    }

    return true;
}