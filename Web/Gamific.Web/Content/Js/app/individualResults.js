$('#dropDownState').change(function () {
    refreshDropDownEpisodes($(this).val());
});

$('#dropDownEpisodes').change(function () {
    refreshDropDownTeams($(this).val());
});

$('#dropDownTeams').change(function () {
    refreshCardResults($("#dropDownEpisodes").val(), $("#dropDownTeams").val());
});

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
