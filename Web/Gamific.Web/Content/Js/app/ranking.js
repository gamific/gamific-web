$(document).ready(function () {
    refreshDropDownEpisodes();
});

$('#dropDownEpisodes').change(function () {
    refreshDropDownTeams($(this).val());
});

$('#dropDownTeams').change(function () {
    $('#rankingDataTable').dataTable().fnDestroy();
    loadRankingDataTable();
    loadRankingDataTablePlayer();
});

$('#dropDownMetrics').change(function () {
    $('#rankingDataTable').dataTable().fnDestroy();
    loadRankingDataTable();
    loadRankingDataTablePlayer();
});

function refreshDropDownEpisodes(currentId) {
    $.ajax({
        url: "/public/ranking/buscarEpisodios",
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
                $("#dropDownEpisodes").append($("<option value='empty'>Vazio</option>"));
                $("#dropDownTeams").append($("<option value='empty'>Vazio</option>"));
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

            if (teams.length < 1) {
                $("#dropDownTeams").append($("<option value='empty'>Vazio</option>"));
            }
            else {
                $("#dropDownTeams").append($("<option value='empty'>Todas</option>"));
            }

            for (var i = 0; i < teams.length; i++) {
                var selected = "";
                if (currentId == teams[i].id) {
                    selected = "selected";
                }
                $("#dropDownTeams").append($("<option value='" + teams[i].id + "'" + selected + " >" + teams[i].nick + "</option>"));
            }

            $('#rankingDataTable').dataTable().fnDestroy();
            loadRankingDataTable();
        },
        error: function () {
            $("#dropDownTeams").empty();
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
        "initComplete": function (settings, json) {
            loadCharts();
        }
    });
};

function loadRankingDataTablePlayer() {
    table = $('#rankingDataTablePlayer').dataTable({
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

                        if ($('#dropDownTeams').val() != 'empty') {
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
        "initComplete": function (settings, json) {
            loadCharts();
        }
    });
};

function loadCharts() {

    var logoPath = "";
    $.ajax({
        url: window.location.origin + "/apiMedia/imagePath",
        async: false,
        type: "GET",
        success: function (data) {
            logoPath = data;
        }
    });

    var firstInput = $("#posicao1").val();
    var secondInputs = $("#posicao2").val();
    var thirdInput = $("#posicao3").val();

    if (firstInput) {
        var first = firstInput.split(";")[0];
        var firstLogo = firstInput.split(";")[1];
    } else {
        var first = "1°";
        var firstLogo = "#";
    }

    if (secondInputs) {
        var second = secondInputs.split(";")[0];
        var secondLogo = secondInputs.split(";")[1];
    } else {
        var second = "2°";
        var secondLogo = "#";
    }

    if (thirdInput) {
        var third = thirdInput.split(";")[0];
        var thirdLogo = thirdInput.split(";")[1];
    } else {
        var third = "3°";
        var thirdLogo = "#";
    }

    var dataPodium = [];
    if (secondInputs != undefined) {
        dataPodium.push([second.length > 10 ? second.substring(0, 10) + "..." : second, 2]);
    }
    if (firstInput != undefined) {
        dataPodium.push([first.length > 10 ? first.substring(0, 10) + "..." : first, 3]);
    }
    if (thirdInput != undefined) {
        dataPodium.push([third.length > 10 ? third.substring(0, 10) + "..." : third, 1]);
    }

    $('#podiumContainer').highcharts({
        chart: {
            type: 'column',
            spacingTop: 60,
            style: {
                fontFamily: '"Segoe UI", Arial, sans-serif'
            },
            backgroundColor: 'rgba(255,255,255,0)'
        },
        title: {
            text: ' '
        },
        xAxis: {
            categories: false,
            lineWidth: 0,
            minorGridLineWidth: 0,
            lineColor: 'transparent',
            labels: {
                enabled: false
            },
            minorTickLength: 0,
            tickLength: 0
        },
        yAxis: {
            min: 0,
            gridLineWidth: 0,
            title: {
                text: false
            },
            labels: {
                enabled: false
            }
        },
        legend: {
            enabled: false

        },
        credits: {
            enabled: false
        },
        tooltip: false,
        plotOptions: {
            column: {
                pointPadding: -0.05,
                borderWidth: 0
            },
            series: {
                color: '#FFF',
                borderRadius: 6
            }
        },

        

        series: [{
            name: ' ',
            data: dataPodium,
            dataLabels: {
                enabled: true,
                color: '#95ceff',
                align: 'center',
                x: 3,
                y: 72,
                useHTML: true,
                overflow: false,
                crop: false,
                formatter: function () {
                    if (4 - this.y == 1 && firstInput != undefined) {
                        return '<div class="podiumChart" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + firstLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span style="font-family: \'Segoe UI\', Arial, sans-serif; font-size: 14px; color: #FFF; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                    if (4 - this.y == 2 && secondInputs != undefined) {
                        return '<div class="podiumChart" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + secondLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span style="font-family: \'Segoe UI\', Arial, sans-serif; font-size: 14px; color: #FFF; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                    if (4 - this.y == 3 && thirdInput != undefined) {
                        return '<div class="podiumChart" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + thirdLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span style="font-family: \'Segoe UI\', Arial, sans-serif; font-size: 14px; color: #FFF; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                },
                style: {
                    fontSize: '50px',
                    fontFamily: '"Segoe UI", Arial, sans-serif'
                }
            }
        }]
    });

};

$(document).ready(function () {
    $.ajax({
        url: "/public/ranking/buscarMetricas",
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







//////////////////////////////////////////////////////////////////////////////////////////
/*
var workerTypeId = $("#WorkerTypeId").val();
var teamId = $("#TeamId").val();
var episodeId = $("#EpisodeId").val();

function getTeamsByWorkerType(value) {
    $('#submitButton').click();
}

function getPlayersByTeam(value) {
    $('#submitButton').click();
}

function getTeamsByEpisode(value) {
    $('#submitButton').click();
}

var table;

function loadRankingDataTable() {
    table = $('#rankingDataTable').dataTable({
        "serverSide": true,
        "ajax": "/public/ranking/buscarResultados?workerTypeId=" + workerTypeId + "&teamId=" + teamId + "&episodeId=" + episodeId,
        "processing": true,
        "ordering": true,
        "pagingType": 'simple',
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
        "dom": '<"newtoolbar">frtip',
        "fnServerParams": function (aoData) {
        },
        "columnDefs": [

                {
                    "width": "20%",
                    "targets": 0,
                    "orderable": true,
                    "searchable": true,
                    "render": function (data, type, row, meta) {

                        var links = (meta.row + 1) + "°";

                        return links;
                    }
                },
                {
                    "width": "40%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true,
                    "render": function (data, type, row, meta) {

                        var links = "<a href='#'>" + data.split(";")[0] + "</a>";

                        links += " <input id='posicao" + (meta.row + 1) + "' value='" + data + "' hidden/>"

                        return links;
                    }
                },
                {
                    "width": "40%",
                    "targets": 2,
                    "orderable": true,
                    "searchable": true,
                }
        ],
        "initComplete": function (settings, json) {
            loadCharts();
        }
    });
};

loadRankingDataTable();


*/