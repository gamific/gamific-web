unityOn = false;

$(document).ready(function () {
    refreshDropDownEpisodes();
});

$('#dropDownEpisodes').change(function () {
    refreshDropDownTeams($(this).val());
    if (unityOn === true) {
        UpdateResults();
    }
});

$('#dropDownTeams').change(function () {
    if (unityOn === true) {
        UpdateResults();
    }
});

$('#dropDownMetrics').change(function () {
    if (unityOn === true) {
        UpdateResults();
    }
});

function refreshDropDownEpisodes(currentId) {
    $.ajax({
        url: "/public/corrida/buscarEpisodios",
        async: false,
        type: "GET",
        success: function (data) {
            $("#dropDownEpisodes").empty();
            var episodes = JSON.parse(data);

            for (var i = 0; i < episodes.length; i++) {
                var selected = "";
                if (currentId == episodes[i].id) {
                    selected = "selected";
                }
                $("#dropDownEpisodes").append($("<option value='" + episodes[i].id + "'" + selected + " >" + episodes[i].name + "</option>"));
            }

            refreshDropDownTeams($('#dropDownEpisodes').val());

            if (episodes.length <= 0) {
                $("#dropDownTeams").empty();
                $("#dropDownEpisodes").append($("<option value=''>Vazio</option>"));
                $("#dropDownTeams").append($("<option value=''>Vazio</option>"));
            }
        },
        error: function () {
            $("#dropDownEpisodes").empty();
            $("#dropDownTeams").empty();
        }
    });
}

function refreshDropDownTeams(episodeId, currentId) {
    $.ajax({
        url: "/public/corrida/buscarEquipes",
        async: false,
        type: "GET",
        data:
        {
            "episodeId": episodeId
        },
        success: function (data) {
            $("#dropDownTeams").empty();
            var teams = JSON.parse(data);

            if (teams.length < 1) {
                $("#dropDownTeams").append($("<option value=''>Vazio</option>"));
            }
            else {
                $("#dropDownTeams").append($("<option value=''>Todas</option>"));
            }

            for (var i = 0; i < teams.length; i++) {
                var selected = "";
                if (currentId == teams[i].id) {
                    selected = "selected";
                }
                $("#dropDownTeams").append($("<option value='" + teams[i].id + "'" + selected + " >" + teams[i].nick + "</option>"));
            }
        },
        error: function () {
            $("#dropDownTeams").empty();
        }
    });
}


function UpdateResults() {
    $.ajax({
        url: "/public/corrida/buscarScore/",
        async: false,
        type: "GET",
        data: {
            "episodeId": $('#dropDownEpisodes').val(),
            "teamId": $('#dropDownTeams').val(),
            "metricId": $('#dropDownMetrics').val()
        },
        success: function (dto) {
            SendMessage("Configuration", "StartUnity", dto);
        },
        error: function () {
            //SendMessage("Configuration", "StartUnity", "{}");
        }
    });
}

$(document).ready(function () {
    $.ajax({
        url: "/public/corrida/buscarMetricas",
        async: false,
        type: "GET",
        success: function (data) {
            var metrics = JSON.parse(data);

            for (var i = 0; i < metrics.length; i++) {
                $("#dropDownMetrics").append($("<option value='" + metrics[i].id + "'>" + metrics[i].name + "</option>"));
            }
        },
        error: function () {
            $("#dropDownMetrics").empty();
        }
    });
})


function submit(value) {
    $('#destination').val(value);
    $('#submitButton').click();
}

function StartUnity() {
    UpdateResults();
    $('#Loading').hide();
    $('#unityPlay').show();
    $('#raceOptions').show();
    unityOn = true;
}

function StopRace() {
    SendMessage("Configuration", "StopRace");
}

function SetTimer() {
    SendMessage("Configuration", "SetTimer");
}

function ChangeCamera() {
    SendMessage("Configuration", "ChangeCamera");
}

function TurnOffMarks() {
    SendMessage("Configuration", "TurnOffMarks");
}
