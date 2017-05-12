$('#dropDownState').change(function () {
    refreshDropDownEpisodes($(this).val());
});

$('#dropDownEpisodes').change(function () {
    refreshDropDownTeams($(this).val());
    loadMorris(1);

    initializeChart();

});

$('#dropDownTeams').change(function () {
    refreshDropDownWorkers($(this).val());

    if ($("#dropDownTeams").val() == "empty") {
        loadMorris(1);
    } else {
        loadMorris(2);
    }

});

$('#dropDownWorkers').change(function () {

    refreshCardResults($("#dropDownEpisodes").val(), $("#dropDownTeams").val(), $("#dropDownWorkers").val());
    if ($("#dropDownWorkers").val() == "empty") {
        loadMorris(2);
    } else {
        loadMorris(3);
    }

});

function refreshDropDownEpisodes(state, currentId) {
    $.ajax({
        url: "/public/dashboard/buscarEpisodios",
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

function refreshCardResults(episodeId, teamId, playerId) {
    $('#div-cards').empty();
    $.ajax({
        url: "/public/dashboard/buscarResultados",
        async: false,
        type: "GET",
        data:
        {
            "episodeId": episodeId,
            "teamId": teamId,
            "playerId": playerId
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
                else if (cardResults[i].percentGoal > 1.2) {
                    color = "card-blue";
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
                                + "<div class='back'><a href='/public/dashboard/detalhes/" + episodeId + "/" + cardResults[i].metricId + "/" + teamId + "/" + playerId + "'><i class='fa " + icon + " fa-4x'></i><span>Detalhes</span></a>"
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
                    numberStep: function (now, tween) {
                        var decimal_places = 2;
                        var decimal_factor = decimal_places === 0 ? 1 : Math.pow(10, decimal_places);
                        var floored_number = Math.floor(cardResults[i].percentGoal * decimal_factor * 100) / decimal_factor;
                        floored_number = floored_number.toString().replace('.', ',');
                        var target = $('#' + cardResults[i].metricId + '-percent');
                        target.text(floored_number)
                    }
                }, 1500);

                $('#' + cardResults[i].metricId + '-goal').animateNumber({
                    number: cardResults[i].goal,
                    numberStep: comma_separator_number_step,
                }, 1500);

                var bar_id = 'a' + cardResults[i].metricId + "-bar";
                createProgressBar(bar_id, cardResults[i].percentGoal);
            }

            $('.card.hover').hover(function () {
                $(this).addClass('flip');
            }, function () {
                $(this).removeClass('flip');
            });
        }
    });
}

function showLoading() {
    //document.getElementById('chartLoading').style.display = 'block';
    //document.getElementById('metrics').style.display = 'none';
}

function hideLoading() {
    // document.getElementById('chartLoading').style.display = 'none';
    //document.getElementById('metrics').style.display = 'block';
}

function LoadMetricResultsDataTable() {
    table = $('#MetricResultsDataTable').dataTable({
        "serverSide": true,
        "ajax": "/public/dashboard/resultadosMetrica/" + $('#MetricId').val() + "/" + $('#EpisodeId').val() + "/" + $('#TeamId').val() + "/" + $('#PlayerId').val(),
        "processing": true,
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
        "fnServerParams": function (aoData) { },
        "columnDefs": [
            {
                "width": "15%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "25%",
                "targets": 1,
                "orderable": false,
                "serchable": false,
            },
            {
                "width": "20%",
                "targets": 2,
                "orderable": false,
                "serchable": false
            },
            {
                "width": "15%",
                "targets": 3,
                "orderable": false,
                "serchable": false,
            },
            {
                "width": "10%",
                "targets": 4,
                "orderable": false,
                "serchable": false,
            },
            {
                "width": "10%",
                "targets": 5,
                "orderable": false,
                "serchable": false,
                "render": function (data, type, row) {
                    var value = row[3];
                    var name = row[1];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/public/dashboard/editar/" + data + "' title='Editar resultado.'> </a> &nbsp; <a class='fa fa-remove' href='#' onclick='removeClickResult(\"" + data + "\",\"" + value + "\",\"" + name + "\")' title='Remover resultado.'> </a>";
                    return links;
                }
            }
        ],
    });
}

function Remove(resultId) {
    $.ajax({
        url: "/public/dashboard/remover/" + resultId,
        async: true,
        type: "POST",
        success: function () {
            window.location.reload;
        }
    });

    window.location.reload();
}

function removeClickResult(data, value, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover o resultado " + value + " de " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/public/dashboard/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Resultdado removido com sucesso.", "success");

                        $('#MetricResultsDataTable').dataTable().fnDestroy();
                        LoadMetricResultsDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover o resultado.", "danger");
                        dialog.close();
                    }
                });

            }
        }, {
            label: 'Não',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });

    dialog.getModalHeader().css("background-color", "#AA0000");
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
    if (playerId != "" && playerId != undefined) {
        refreshDropDownWorkers(teamId, playerId);
    }
}

function onSucessSaveResult() {
    window.location.reload();
    //$('#MetricResultsDataTable').dataTable().fnDestroy();
    //LoadMetricResultsDataTable();
}

function initializeChart() {

    $('#statistics-chart').empty();

    var campaignId = $('#dropDownEpisodes').val();

    var metrics = $('.metricsChart');

    var metricsIds = [];

    var n;
    for (n = 0; n < metrics.length; n++) {
        if (metrics[n].checked) {
            metricsIds.push(metrics[n].value);
        }
    }

    var metricToInitialize = $('#metricToInitialize').val();

    if (metricsIds.length == 0) {
        var id = '#chart-' + metricToInitialize;

        $(id).click();

        return;
    }

    var initDate = $('#InitialDate').val();

    var endDate = $('#FinishDate').val();

    if (campaignId && campaignId.length > 0) {

        $.ajax({
            url: "/public/dashboard/loadChart",
            async: false,
            data: { metricsIds: metricsIds, campaignId: campaignId, initDate: initDate, endDate: endDate },
            type: "POST",
            dataType: 'json',
            success: function (data) {
                var lineData = [];
                var metricNameList = [];
                var colors = [];

                var w;
                for (w = 0; w < data.length; w++) {
                    var i;

                    for (i = 0; i < data[w].entries.length; i++) {
                        var metricName = data[w].entries[i].description;
                        metricNameList.push(metricName);

                        var lineElement = { period: data[w].entries[i].name }
                        lineElement[metricName] = data[w].entries[i].value;
                        lineData.push(lineElement);
                    }
                    colors.push(generateColor());
                }

                var config = {
                    data: lineData,
                    xkey: 'period',
                    ykeys: [metricNameList[0]],
                    labels: [metricNameList[0]],
                    parseTime: false,
                    fillOpacity: 0.6,
                    hideHover: 'auto',
                    behaveLikeLine: true,
                    resize: true,
                    pointFillColors: ['#ffffff'],
                    pointStrokeColors: ['black'],
                    continuousLine: true,
                    lineColors: [colors]
                };

                config.element = 'statistics-chart';

                Morris.Line(config);

            }
        });

    }

}

function generateRandomNumber(inferior, superior) {
    numPossibilidades = superior - inferior
    aleat = Math.random() * numPossibilidades
    aleat = Math.floor(aleat)
    return parseInt(inferior) + aleat
}

function generateColor() {
    hexadecimal = new Array("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F")
    cor_aleatoria = "#";
    for (i = 0; i < 6; i++) {
        posarray = generateRandomNumber(0, hexadecimal.length)
        cor_aleatoria += hexadecimal[posarray]
    }
    return cor_aleatoria
}

$('#metricMorris').change(function () {
    loadMorris($('#morrisType').val());
});

function loadMorris(type) {

    //type legend
    // 1 = episode
    // 2 = team
    // 3 = run

    $('#morrisType').val(type);

    var metricId = $('#metricMorris').val();
    var teamId = $('#dropDownTeams').val();
    var episodeId = $('#dropDownEpisodes').val();
    var runId = $('#dropDownWorkers').val();

    if (type == 1) {
        $.ajax({
            url: "/public/dashboard/loadMorrisByEpisode/" + metricId + "/" + episodeId,
            async: false,
            type: "GET",
            success: function (data) {
                Morris.Donut({
                    element: 'products',
                    data: data.products,
                    colors: data.colors
                });

                $('#products').find("path[stroke='#ffffff']").attr('stroke', 'rgba(0,0,0,0)');

                $('.morrisLi').remove();

                var z;
                for (z = 0; z < data.products.length; z++) {
                    $("#productsList").append('<li class="morrisLi"><span style="border-color: ' + data.colors[z] + '" class="badge badge-outline" ></span>' + data.products[z].label2 + '<small>' + data.products[z].value + '</small></li>');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }

    if (type == 2) {
        $.ajax({
            url: "/public/dashboard/loadMorrisByTeam/" + metricId + "/" + teamId,
            async: false,
            type: "GET",
            success: function (data) {
                Morris.Donut({
                    element: 'products',
                    data: data.products,
                    colors: data.colors
                });

                $('#products').find("path[stroke='#ffffff']").attr('stroke', 'rgba(0,0,0,0)');

                $('.morrisLi').remove();

                var z;
                for (z = 0; z < data.products.length; z++) {
                    $("#productsList").append('<li class="morrisLi"><span style="border-color: ' + data.colors[z] + '" class="badge badge-outline" ></span>' + data.products[z].label + '<small>' + data.products[z].value + '</small></li>');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }

    if (type == 3) {
        $.ajax({
            url: "/public/dashboard/loadMorrisByRun/" + metricId + "/" + runId + "/" + teamId,
            async: false,
            type: "GET",
            success: function (data) {
                Morris.Donut({
                    element: 'products',
                    data: data.products,
                    colors: data.colors
                });

                $('#products').find("path[stroke='#ffffff']").attr('stroke', 'rgba(0,0,0,0)');

                $('.morrisLi').remove();

                var z;
                for (z = 0; z < data.products.length; z++) {
                    $("#productsList").append('<li class="morrisLi"><span style="border-color: ' + data.colors[z] + '" class="badge badge-outline" ></span>' + data.products[z].label + '<small>' + data.products[z].value + '</small></li>');
                }
            },
            error: function (data) {
                alert(data);
            }
        });
    }

}

$(document).ready(function () {
    if (window.location.pathname.search("detalhes") == -1) {
        loadMorris(1);
        loadBarChart();
        initializeChart();
    }
});

function onSuccessSaveFilter(data) {
    $('#entity-edit-modal').modal('hide');

    loadBarChart();
}

function onFailureSaveFilter(data) {

}

function loadBarChart() {

    $('#bar-chart').empty();

    var metrics = $('.barChart');
    var metricsIds = [];

    if (metrics.length > 0) {
        var z;
        for (z = 0; z < metrics.length; z++) {
            if (metrics[z].checked) {
                metricsIds.push(metrics[z].value);
            }
        }
    }

    if (metricsIds.length <= 0) {
        var id = '#bar-' + $('#metricToInitializeBar').val();

        $(id).click();
        return;
    }

    $.ajax({
        url: "/public/dashboard/loadBarChart",
        data: { metricsIds: metricsIds },
        async: false,
        type: "POST",
        success: function (data) {

            var barData = [];
            var labels = [];

            var i;
            for (i = 0; i < data.length; i++) {
                var w;
                var labels1 = [];
                barElement = { episodeName: data[i].EpisodeName }
                for (w = 0; w < data[i].Points.length; w++) {
                    labels1.push(data[i].Points[w].MetricName);
                    var metricName = data[i].Points[w].MetricName;
                    barElement[metricName] = data[i].Points[w].MetricResult;
                }
                labels = labels1;
                barData.push(barElement);
            }

            Morris.Bar({
                element: 'bar-chart',
                data: barData,
                xkey: 'episodeName',
                ykeys: labels,
                labels: labels,
                hideHover: 'auto',
                resize: true,
                gridTextColor: 'white',
                grid: true
            });
        },
        error: function (data) {
            alert(data);
        }
    });
}


$('#InitialDate').datepicker({

    language: 'pt-BR',
    setDate: new Date(),
    changeMonth: true,
    showOtherMonths: true,
    selectOtherMonths: true,
    selectOtherYears: true,
    changeYear: true,
    onSelect: function (value, date) {
        $("#InitialDate").hide();
    }

});


$('#FinishDate').datepicker({

    language: 'pt-BR',
    setDate: new Date(),
    changeMonth: true,
    showOtherMonths: true,
    selectOtherMonths: true,
    selectOtherYears: true,
    changeYear: true,
    onSelect: function (value, date) {
        $("#FinishDate").hide();
    }

});


