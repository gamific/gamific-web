function CheckChange(value) {

    if ($('#' + value).is(":checked")) {
        if ($('#DaysOfWeek').val() != "") {
            $('#DaysOfWeek').val($('#DaysOfWeek').val() + "," + value);
        }
        else {
            $('#DaysOfWeek').val(value);
        }
    }
    else {
        var daysOfWeek = $('#DaysOfWeek').val().split(',');
        var daysChecked = "";

        daysOfWeek.forEach(function (val, key) {
            if (val != value && val != "") {
                if (key != 0) {
                    daysChecked += ",";
                }
                daysChecked += val;
            }
        });

        $('#DaysOfWeek').val(daysChecked);
    }

}