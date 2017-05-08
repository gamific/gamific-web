$('#dropDownState').change(function () {
    refreshDropDownEpisodes($(this).val());
});

$('#dropDownEpisodes').change(function () {
    refreshDropDownTeams($(this).val());
    $('#rankingDataTablePlayers').dataTable().fnDestroy();
    loadRankingDataTablePlayer();
});

$('#dropDownTeams').change(function () {
    refreshCardResults($("#dropDownEpisodes").val(), $("#dropDownTeams").val());
    loadRankingDataTable();
});

$('#dropDownMetrics').change(function () {
    $('#rankingDataTable').dataTable().fnDestroy();
    loadRankingDataTable();
    $('#rankingDataTablePlayers').dataTable().fnDestroy();
    loadRankingDataTablePlayer();
});

$(document).ready(function () {
    $.ajax({
        url: "/public/resultadosIndividuais/buscarMetricas",
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

function refreshDropDownEpisodes(state, currentId) {
    $.ajax({
        url: "/public/resultadosIndividuais/buscarEpisodios",
        async: false,
        type: "GET",
        data:
        {
            "state": state
        },
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
                $("#dropDownEpisodes").append($("<option value=''>Vazio</option>"));
                $("#dropDownTeams").append($("<option value=''>Vazio</option>"));
                $('#div-cards').empty();
            }

            $('#rankingDataTablePlayers').dataTable().fnDestroy();
            loadRankingDataTablePlayer();
        },
        error: function () {
            $("#dropDownEpisodes").empty();
        }
    });
}

function refreshDropDownTeams(episodeId, currentId) {
    $.ajax({
        url: "/public/resultadosIndividuais/buscarEquipes",
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
                $("#dropDownTeams").empty();
                $("#dropDownTeams").append($("<option value='empty'>Vazio</option>"));
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

            refreshCardResults($("#dropDownEpisodes").val(), $("#dropDownTeams").val());
        },
        error: function () {
            $("#dropDownTeams").empty();
        }
    });
}

function refreshCardResults(episodeId, teamId) {
    $('#div-cards').empty();
    $.ajax({
        url: "/public/resultadosIndividuais/buscarResultados",
        async: false,
        type: "GET",
        data:
        {
            "episodeId": episodeId,
            "teamId": teamId
        },
        success: function (data) {
            var cardResults = JSON.parse(data);
            var icon = "fa-bar-chart-o";

            var k = 1;
            for (var i in cardResults) {
                if (i % 8 == 0) {
                    containerCards = "";
                    if (i == 0) {
                        containerCards += "<div class='item active'>";
                    }
                    else {
                        containerCards += "<div class='item'>";
                    }

                    containerCards += "<div class='col-xs-12' id='div-cards-" + k + "'>";
                    containerCards += "</div>";
                    containerCards += "</div>";
                    k++;
                    $('#div-cards').append(containerCards);
                }
            }

            k = 1;
            for (var i in cardResults) {
                var color = "card-greensea";
                if (cardResults[i].percentGoal == 0) {
                    color = "card-redbrown";
                }
                else if (cardResults[i].percentGoal <= 0.3) {
                    color = "card-redbrown";
                }
                else if (cardResults[i].percentGoal < 1) {
                    color = "card-orange";
                }

                if (i % 8 == 0 && i != 0) {
                    k++;
                }

                var card = "";

                card += "<div class='card-container col-lg-3 col-md-3 col-sm-6'>"
                                + "<div class='card " + color + " hover'>"
                                + "<div class='front'> "
                                + "<div class='media'>"
                                + "<div class='media-body' style='text-overflow: ellipsis;width:100%; white-space:nowrap;'>"
                                + "<span class='pull-left'>"
                                + "<i class='fa " + cardResults[i].iconMetric + " media-object'></i>"
                                + "</span>"
                                + "<small style='padding-left: 5%;'>" + cardResults[i].metricName + "</small>"
                                + "<h2 class='media-heading animate-number'>"
                                + "<span id='" + cardResults[i].metricId + '-points' + "' class='media-heading animate-number' style='padding-left: 5%;'>"
                                + "</span>"
                                + "</h2>"
                                + "</div>"
                                + "</div>"
                                + "<div class='progress-list'>"
                                + "<div class='details'>"
                                + "<div class='status pull-right bg-transparent-black-1'>"
                                + "Meta: <span id='" + cardResults[i].metricId + "-goal'></span>"
                                + "</div>"
                                + "</div>"
                                + "<div class='status pull-right bg-transparent-black-1'>"
                                + "<span id='" + cardResults[i].metricId + "-percent' class='animate-number'>23</span>%"
                                + "</div>"
                                + "<div class='clearfix'></div>"
                                + "<div id='a" + cardResults[i].metricId + "-bar' class='bar-prog'></div>"
                                + "</div>"
                                + "</div>"
                                + "<div class='back'><a href='/public/resultadosIndividuais/detalhes/" + episodeId + "/" + cardResults[i].metricId + "/" + teamId + "/" + "" + "'><i class='fa " + icon + " fa-4x'></i><span>Detalhes</span></a>"
                                + "</div>"
                                + "</div>"
                                + "</div>";


                var id_card = "#div-cards-" + k;
                $(id_card).append(card);

                var comma_separator_number_step = $.animateNumber.numberStepFactories.separator('.');
                $('#' + cardResults[i].metricId + '-points').animateNumber({
                    number: cardResults[i].totalPoints,
                    numberStep: comma_separator_number_step
                }, 1500);

                $('#' + cardResults[i].metricId + '-percent').animateNumber({
                    number: cardResults[i].percentGoal * 100,
                    numberStep: comma_separator_number_step
                }, 1500);

                $('#' + cardResults[i].metricId + '-goal').animateNumber({
                    number: cardResults[i].goal,
                    numberStep: comma_separator_number_step
                }, 1500);

                var bar_id = 'a' + cardResults[i].metricId + "-bar";
                createProgressBar(bar_id, cardResults[i].percentGoal);
            }

            /*
            $('.card.hover').hover(function () {
                $(this).addClass('flip');
            }, function () {
                $(this).removeClass('flip');
            });
            */
        }
    });
}

function showLoading() {
    document.getElementById('chartLoading').style.display = 'block';
    document.getElementById('metrics').style.display = 'none';
}

function hideLoading() {
    document.getElementById('chartLoading').style.display = 'none';
    document.getElementById('metrics').style.display = 'block';
}

function Remove(resultId) {
    $.ajax({
        url: "/public/resultadosIndividuais/remover/" + resultId,
        async: true,
        type: "POST",
        success: function () {
            window.location.reload;
        }
    });

    window.location.reload();
}


function createProgressBar(id, percent) {
    if (percent > 1) {
        percent = 1;
    }
    var bar = new ProgressBar.Line('#' + id, {
        strokeWidth: 0.5,
        easing: 'easeInOut',
        duration: 2000,
        color: 'rgba(0,0,0,0.25)',
        trailColor: 'rgba(0,0,0,0.1)',
        trailWidth: 1,
        svgStyle: { width: '100%', height: '100%' }
    });

    bar.animate(percent);
}

function dashBoardLoad(state, episodeId, teamId, playerId) {
    refreshDropDownEpisodes(state, episodeId);
    if (teamId != "" && teamId != undefined) {
        refreshDropDownTeams(episodeId, teamId);
    }
}

function onSucessSaveResult() {
    window.location.reload();
    //$('#MetricResultsDataTable').dataTable().fnDestroy();
    //LoadMetricResultsDataTable();
}

function loadRankingDataTablePlayer() {
    table = $('#rankingDataTablePlayers').dataTable({
        "serverSide": true,
        "ajax": "/public/resultadosIndividuais/searchPlayers?episodeId=" + $('#dropDownEpisodes').val() + "&metricId=" + $('#dropDownMetrics').val(),
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
                    "width": "40%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true,
                    "render": function (data, type, row, meta) {
                        var ref;
                        if ($('#userProfile').val() == "JOGADOR") {
                            ref = "#";
                        }
                        else {
                            ref = "/public/dashboard/" + $('#dropDownEpisodes').val() + "/" + data.split(";")[3] + "/" + data.split(";")[2];
                        }

                        var links = "<a href='" + ref + "'>" + data.split(";")[0] + "</a>";

                        links += " <input id='posicaoPlayers" + (meta.row + 1) + "' value='" + data + "' hidden/>"

                        return links;
                    }
                },
                {
                    "width": "35%",
                    "targets": 2,
                    "orderable": true,
                    "searchable": true,
                },
                {
                    "width": "15%",
                    "targets": 3,
                    "orderable": true,
                    "searchable": true,
                }
        ],
        "initComplete": function (settings, json) {
            loadChartsPlayers();
        }
    });
};

function loadRankingDataTable() {
    table = $('#rankingDataTable').dataTable({
        "serverSide": true,
        "ajax": "/public/resultadosIndividuais/search?episodeId=" + $('#dropDownEpisodes').val() + "&teamId=" + $("#dropDownTeams").val() + "&metricId=" + $('#dropDownMetrics').val(),
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

function loadChartsPlayers() {

    var logoPath = "";
    $.ajax({
        url: window.location.origin + "/apiMedia/imagePath",
        async: false,
        type: "GET",
        success: function (data) {
            logoPath = data;
        }
    });

    var firstInput = $("#posicaoPlayers1").val();
    var secondInputs = $("#posicaoPlayers2").val();
    var thirdInput = $("#posicaoPlayers3").val();

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

    $('#podiumContainerPlayers').highcharts({
        chart: {
            type: 'column',
            spacingTop: 60,
            style: {
                fontFamily: '"Segoe UI", Roboto, sans-serif'
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
                        return '<div class="podiumChart dimencionament" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + firstLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span class="colore-name"; style="font-family: \'Segoe UI\', Roboto, sans-serif; font-size: 14px; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                    if (4 - this.y == 2 && secondInputs != undefined) {
                        return '<div class="podiumChart dimencionament" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + secondLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span class="colore-name"; style="font-family: \'Segoe UI\', Roboto, sans-serif; font-size: 14px; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                    if (4 - this.y == 3 && thirdInput != undefined) {
                        return '<div class="podiumChart dimencionament" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + thirdLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span class="colore-name"; style="font-family: \'Segoe UI\', Roboto, sans-serif; font-size: 14px; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                },
                style: {
                    fontSize: '50px',
                    fontFamily: '"Segoe UI", Roboto, sans-serif'
                }
            }
        }]
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
                fontFamily: '"Segoe UI", Roboto, sans-serif'
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
                        return '<div class="podiumChart dimencionament" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + firstLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span class="colore-name"; style="font-family: \'Segoe UI\', Roboto, sans-serif; font-size: 14px; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                    if (4 - this.y == 2 && secondInputs != undefined) {
                        return '<div class="podiumChart dimencionament" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + secondLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span class="colore-name"; style="font-family: \'Segoe UI\', Roboto, sans-serif; font-size: 14px; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                    if (4 - this.y == 3 && thirdInput != undefined) {
                        return '<div class="podiumChart dimencionament" title="' + (4 - this.y) + 'º lugar"> <div class="podiumImg podiumPosition' + (4 - this.y) + '" style="background-image: url(\'' + logoPath + thirdLogo + '\')" ></div><span style="width: 100%; text-align: center;display: inline-block;"><span class="colore-name"; style="font-family: \'Segoe UI\', Roboto, sans-serif; font-size: 14px; text-shadow: none; font-weight: normal;">' + this.key + '</span> <br>' + '<span class="podiumLabel podiumLabelPosition' + (4 - this.y) + '">' + (4 - this.y) + 'º</span></span></div>';
                    }
                },
                style: {
                    fontSize: '50px',
                    fontFamily: '"Segoe UI", Roboto, sans-serif'
                }
            }
        }]
    });
};
