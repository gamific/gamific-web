$('#dropDownEpisodes').change(function () {
    refreshDropDownTeams($(this).val());
});
$(document).ready(function () {
    refreshDropDownEpisodes();
});

$('#dropDownTeams').change(function () {
    refreshDropDownWorkers($(this).val());
});

$('#dropDownWorkers').change(function () {
    refreshCardResults($("#dropDownEpisodes").val(), $("#dropDownTeams").val(), $("#dropDownWorkers").val());
});

function refreshDropDownEpisodes(currentId) {
    $.ajax({
        url: "/admin/notificacoes/buscarEpisodios",
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

            if (currentId == "" || currentId == undefined) {
                refreshDropDownTeams($('#dropDownEpisodes').val());
            }

            if (episodes.length <= 0) {
                $("#dropDownEpisodes").empty();
                $("#dropDownTeams").empty();
                $("#dropDownWorkers").empty();
                $("#dropDownEpisodes").append($("<option value=''>Vazio</option>"));
                $("#dropDownTeams").append($("<option value=''>Vazio</option>"));
                $("#dropDownWorkers").append($("<option value=''>Vazio</option>"));
                $('#div-cards').empty();
            }

        },
        error: function () {
            $("#dropDownEpisodes").empty();
        }
    });
}

function refreshDropDownTeams(episodeId, currentId) {
    $.ajax({
        url: "/public/dashboard/buscarEquipes",
        async: false,
        type: "GET",
        data:
        {
            "episodeId": episodeId
        },
        success: function (data) {
            $("#dropDownTeams").empty();
            var teams = JSON.parse(data);

            if (teams.length >= 1) {
                $("#dropDownTeams").append($("<option value='empty'>Todos</option>"));
            }
            else {
                $("#dropDownTeams").empty();
                $("#dropDownTeams").append($("<option value='empty'>Vazio</option>"));
                $("#dropDownWorkers").empty();
                $("#dropDownWorkers").append($("<option value='empty'>Vazio</option>"));
            }

            for (var i = 0; i < teams.length; i++) {
                var selected = "";
                if (currentId == teams[i].id) {
                    selected = "selected";
                }
                $("#dropDownTeams").append($("<option value='" + teams[i].id + "'" + selected + " >" + teams[i].nick + "</option>"));
            }

            if (currentId == "" || currentId == undefined) {
                refreshDropDownWorkers($('#dropDownTeams').val());
            }

        },
        error: function () {
            $("#dropDownTeams").empty();
        }
    });
}

function refreshDropDownWorkers(teamId, currentId) {
    $.ajax({
        url: "/public/dashboard/buscarJogadores",
        async: false,
        type: "GET",
        data:
        {
            "teamId": teamId
        },
        success: function (data) {
            $("#dropDownWorkers").empty();
            var workers = JSON.parse(data);

            if (workers.length >= 1) {
                $("#dropDownWorkers").append($("<option value='empty' selected>Todos</option>"));
            }
            else {
                $("#dropDownWorkers").append($("<option value='empty' selected>Vazio</option>"));
            }

            for (var i = 0; i < workers.length; i++) {
                var selected = "";
                if (currentId == workers[i].Value) {
                    selected = "selected";
                }
                $("#dropDownWorkers").append($("<option value='" + workers[i].Value + "'" + selected + " >" + workers[i].Text + "</option>"));
            }

            refreshCardResults($("#dropDownEpisodes").val(), $("#dropDownTeams").val(), $("#dropDownWorkers").val());

        },
        error: function () {
            $("#dropDownWorkers").empty();
        }
    });
}

function loadRankingDataTable() {
    table = $('#rankingDataTable').dataTable({
        "serverSide": true,
        "ajax": "/public/ranking/search?episodeId=" + $('#dropDownEpisodes').val() + "&teamId=" + $("#dropDownTeams").val() + "&metricId=" + $('#dropDownMetrics').val(),
        "processing": true,
        "ordering": true,
        "scrollY": "300px",
        "scrollCollapse": true,
        "deferRender": true,
        "lengthChange": false,
        "language": {
            "emptyTable": "Não foram encontrados resultados.",
            "paginate": {
                "previous": '<i class="fa fa-angle-left"></i>',
                "next": '<i class="fa fa-angle-right"></i>'
            }
        },
        "dom": '<"newtoolbar">rtip',
        "fnServerParams": function (aoData) {
        },
        "columnDefs": [
                {
                    "width": "10%",
                    "targets": 0,
                    "orderable": true,
                    "searchable": true,
                    "render": function (data, type, row, meta) {
                        var links = ((meta.row + 1) + (10 * data)) + "°";

                        return links;
                    }
                },
                {
                    "width": "45%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true,
                    "render": function (data, type, row, meta) {
                        var ref;
                        if ($('#userProfile').val() == "JOGADOR") {
                            ref = "#";
                        }
                        else if ($('#dropDownTeams').val() != 'empty') {
                            ref = "/public/dashboard/" + $('#dropDownEpisodes').val() + "/" + $('#dropDownTeams').val() + "/" + data.split(";")[2];
                        }
                        else {
                            ref = "/public/dashboard/" + $('#dropDownEpisodes').val() + "/" + data.split(";")[2] + "/" + "empty";
                        }

                        var links = "<a href='" + ref + "'>" + data.split(";")[0] + "</a>";

                        links += " <input id='posicao" + (meta.row + 1) + "' value='" + data + "' hidden/>"

                        return links;
                    }
                },
                {
                    "width": "45%",
                    "targets": 2,
                    "orderable": true,
                    "searchable": true,
                }
        ],
    });
};

