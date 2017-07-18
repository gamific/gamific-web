var table;
function loadCampaignsDataTable() {
    table = $('#campaignDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/campanhas/search",
        "processing": true,
        "pagingType": 'simple',
        "scrollY": "300px",
        "scrollCollapse": true,
        "ordering": true,
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
            aoData.search.value += "";
        },
        "columnDefs": [
                {
                    "width": "90%",
                    "targets": 0,
                    "orderable": true,
                    "searchable": true,
                },
                {
                    "width": "10%",
                    "targets": 1,
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row) {

                        var links = "<a class='fa fa-pencil' href='/admin/campanhas/editar/" + data + "' title='Editar Campanha.'> </a> &nbsp;";

                        if ($('#CurrentProfile').val() == "SUPERVISOR DE CAMPANHA" || $('#CurrentProfile').val() == "ADMINISTRADOR") {
                            links += "<a class='fa fa-remove' href='/admin/campanhas/remover/" + data + "' title='Remover Campanha.'> </a>";
                        }

                        return links;
                    }
                }
        ]
    });

};

loadCampaignsDataTable();

var table;
function loadTeamsToAssociate() {
    var teamsIdList = $('#TeamsId').val().length > 0 ? $('#TeamsId').val() : ",";
    table = $('#associateTeamsDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/campanhas/associarEquipes/" + $('#ProfileId').val() + "/" + ($('#SponsorId').val() != "" ? $('#SponsorId').val() : 0) + "/" + $('#Id').val() + "/" + teamsIdList,
        "processing": true,
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
            aoData.search.value += "";
        },
        "columnDefs": [
                {
                    "width": "10%",
                    "targets": 0,
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row) {
                        var links = "<input type='checkbox' class='teamsIdList' value='" + data + "'>";

                        return links;
                    }
                },
                {
                    "width": "40%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true,
                },
                {
                    "width": "40%",
                    "targets": 2,
                    "orderable": true,
                    "searchable": true,
                },
                {
                    "width": "10%",
                    "targets": 3,
                    "orderable": false,
                    "searchable": false,
                    "render": function (data, type, row) {
                        var links = "<input type='number' class='teamsGoalList' id='teamsGoalList;" + data + "'>";

                        return links;
                    }
                }
        ]
    });

};

function loadTeamsFromCampaign() {
    $('#teamsFromCampaignDataTable').dataTable().fnDestroy();

    var teamsIdToSend = $('#TeamsId').val().length > 0 ? $('#TeamsId').val() : "!key!";

    table = $('#teamsFromCampaignDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/campanhas/carregarEquipes/" + $('#Id').val() + "/" + teamsIdToSend,
        "processing": true,
        "pagingType": 'simple',
        "scrollY": "300px",
        "width": "100%",
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
            aoData.search.value += "";
        },
        "columnDefs": [
                {
                    "width": "30%",
                    "targets": 0,
                    "orderable": true,
                    "searchable": true,
                    "render": function (data, type, row) {
                        var render = data.split(";")[0] + "<img src='/api/media/" + data.split(";")[1] + "?cache=@Html.Raw(DateTime.Now.Millisecond)' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' />";
                        return render;
                    }
                },
                {
                    "width": "30%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true
                },
                {
                    "width": "30%",
                    "targets": 2,
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row) {

                        var links = "";

                        if (data.split(";")[1] != "0") {
                            if ($('#CurrentProfile').val() == "SUPERVISOR DE CAMPANHA" || $('#CurrentProfile').val() == "ADMINISTRADOR" || ($('#CurrentProfile').val() == "SUPERVISOR DE EQUIPE") && data.split(";")[2] == $('#CurrentWorker').val()) {
                                links = "<a id='demo-dt-addrow-btn' class='btn btn-default sing-up-button' href='/admin/equipes/detalheEquipe/" + data.split(";")[0] + "?CampaignTeamId=" + data.split(";")[1] + "'title='Lançar Resultado' >Lançar Resultado</a>";
                            }
                        }

                        return links;
                    }
                },
                {
                    "width": "10%",
                    "targets": 3,
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row) {

                        var links = "";

                        if ($('#CurrentProfile').val() == "SUPERVISOR DE CAMPANHA" || $('#CurrentProfile').val() == "ADMINISTRADOR") {

                            var dataSplited = data.split(";")[0];

                            if (dataSplited != "0") {
                                links = "<a class='fa fa-remove' onclick='removeTeamFromServer(" + dataSplited + ");' href='#' title='Remover Equipe.'> </a>";
                            }
                            else {
                                var id = data.split(";")[1];
                                links = "<a class='fa fa-remove' href='#' onclick='RemoveTableRow(" + "this" + "),RemoveFromDatatable(" + id + ")'> </a>"
                            }
                        }

                        return links;
                    }
                }
        ]
    });
};

loadTeamsFromCampaign();

var currentDate = new Date();
var dateInitial = moment($("#InitialDate").val(), "DD/MM/YYYY HH:mm:SS").toDate()
var dateEnd = moment($("#EndDate").val(), "DD/MM/YYYY HH:mm:SS").toDate()

    $('#EndDate').datepicker({
        language: 'pt-BR',
        changeMonth: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        selectOtherYears: true,
        changeYear: true,
        orientation: 'bottom',
    });

if ($('#EndDate').val() == "01/01/0001 00:00:00") {
    $("#EndDate").datepicker("setDate", currentDate);
} else {
    $("#EndDate").datepicker("setDate", dateEnd);
}

$('#InitialDate').datepicker({
    language: 'pt-BR',
    changeMonth: true,
    showOtherMonths: true,
    selectOtherMonths: true,
    selectOtherYears: true,
    changeYear: true,
    orientation: 'bottom'
});

if ($('#InitialDate').val() == "01/01/0001 00:00:00") {
    $("#InitialDate").datepicker("setDate", currentDate);
} else {
    $("#InitialDate").datepicker("setDate", dateInitial);
}
//initialize form wizard
$('#rootwizard').bootstrapWizard({

    'tabClass': 'nav nav-tabs tabdrop',
    onTabShow: function (tab, navigation, index) {
        var $total = navigation.find('li').not('.tabdrop').length;
        var $current = index + 1;
        var $percent = ($current / $total) * 100;
        $('#rootwizard').find('#bar .progress-bar').css({ width: $percent + '%' });

        // If it's the last tab then hide the last button and show the finish instead
        if ($current >= $total) {
            $('#rootwizard').find('.pager .next').hide();
            $('#rootwizard').find('.pager .finish').show();
            $('#rootwizard').find('.pager .finish').removeClass('disabled');
        } else {
            $('#rootwizard').find('.pager .next').show();
            $('#rootwizard').find('.pager .finish').hide();
        }
    },

    onNext: function (tab, navigation, index) {

        var form = $('.form' + index)

        tab.addClass('success');

    },

    onTabClick: function (tab, navigation, index) {

        var form = $('.form' + (index + 1))

        tab.addClass('success');

    }

});

// Initialize tabDrop
$('.tabdrop').tabdrop({ text: '<i class="fa fa-th-list"></i>' });

function addRow(btn) {
    var parentRow = btn.parentNode.parentNode;
    var table = parentRow.parentNode;
    var rowCount = table.rows.length;

    var row = table.insertRow(rowCount);

    var cell = row.insertCell(0);
    var remove = document.createElement("i");
    remove.className = "fa fa-remove";
    remove.title = "Remover métrica";
    remove.onclick = function () { deleteRow(this); };
    cell.appendChild(remove);

    var metrica = document.createElement("input");
    metrica.type = "text";
    metrica.placeholder = "Informe o nome da métrica";
    metrica.className = "form-control col-xs-6";
    metrica.id = "MetricaList[".concat(table.rows.length - 2) + "].MetricName";
    metrica.name = "MetricaList[".concat(table.rows.length - 2) + "].MetricName";
    cell.appendChild(metrica);

    var peso = document.createElement("input");
    peso.type = "number";
    peso.placeholder = "Informe o peso da métrica";
    peso.className = "form-control col-xs-6";
    peso.id = "MetricaList[".concat(table.rows.length - 2) + "].Weigth";
    peso.name = "MetricaList[".concat(table.rows.length - 2) + "].Weigth";
    peso.min = "1";
    cell.appendChild(peso);

    var minimo = document.createElement("input");
    minimo.type = "number";
    minimo.placeholder = "Valor Mínimo";
    minimo.className = "form-control col-xs-6";
    minimo.id = "MetricaList[".concat(table.rows.length - 2) + "].MinValue";
    minimo.name = "MetricaList[".concat(table.rows.length - 2) + "].MinValue";
    minimo.min = "1";
    cell.appendChild(minimo);

    var maximo = document.createElement("input");
    maximo.type = "number";
    maximo.placeholder = "Valor Máximo";
    maximo.className = "form-control col-xs-6";
    maximo.id = "MetricaList[".concat(table.rows.length - 2) + "].ValueMax";
    maximo.name = "MetricaList[".concat(table.rows.length - 2) + "].ValueMax";
    maximo.min = "1";
    cell.appendChild(maximo);

    var icon = document.createElement("select");
    icon.className = "form-control col-xs-6";
    icon.onchange = function () {
        showIcon(this, this.id);
    };
    icon.style = "float:left"
    icon.id = table.rows.length - 2;
    icon.name = "MetricaList[".concat(table.rows.length - 2) + "].Icon";

    var j;
    var icons = $("#IconsList").val();

    var iconsString = icons.split(";");

    for (j = 0; j < iconsString.length - 1; j++) {
        var option = document.createElement("option");
        option.innerHTML = iconsString[j];
        option.value = iconsString[j];
        icon.add(option, null);
    }
    cell.appendChild(icon);

    var iconShow = document.createElement("i");
    iconShow.className = table.rows.length - 2 + " col-xs-6";
    cell.appendChild(iconShow);
}

function deleteRow(r) {
    var i = r.parentNode.parentNode.rowIndex;

    var flagDeleted = document.createElement("input");
    flagDeleted.type = "hidden";
    flagDeleted.id = "MetricaList[".concat(i - 1) + "].FlagDeleted";
    flagDeleted.name = "MetricaList[".concat(i - 1) + "].FlagDeleted";
    flagDeleted.value = "1";
    document.getElementById("permissions").rows[i].appendChild(flagDeleted);

    document.getElementById("permissions").rows[i].style.display = 'none';
}

function getTeamsSelected() {
    var list = $('.teamsIdList');

    var listToSend = "";

    var i;

    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            listToSend += list[i].value + ",";
        }
    }

    $('#TeamsId').val($('#TeamsId').val() + listToSend);
}

function getGoalsInputed() {
    getTeamsSelected();

    var list = $('.teamsGoalList');

    var listToSend = "";

    var teamsIds = $('#TeamsId').val().split(',');

    var i, j;

    if (teamsIds[0].length > 0) {
        for (j = 0; j < teamsIds.length; j++) {
            for (i = 0; i < list.length; i++) {
                if (teamsIds[j] == list[i].id.split(";")[1]) {
                    if (list && list.length > 0) {
                        if (list[i].value && list[i].value != null && list[i].value.length > 0) {
                            listToSend += list[i].id.split(";")[1] + ";" + list[i].value + ",";
                        } else {
                            $('#TeamsId').val("");
                            return false;
                        }
                    }
                }
            }
        }
    }

    $('#GoalList').val(listToSend);

    return true;
}

function RemoveFromDatatable(id) {
    var teamsRemoved = "";
    var goalsRemoved = "";

    var teamsIdSplited = $('#TeamsId').val().split(",");

    var goalsSplited = $('#GoalList').val().split(",");

    for (var i = 0; i < teamsIdSplited.length - 1; i++) {
        var teamIdFromGoal = goalsSplited[i].split(";")[0];
        var goal = goalsSplited[i].split(";")[1];
        if (teamsIdSplited[i] != id) {
            goalsRemoved += teamIdFromGoal + ";" + goal + ",";
            teamsRemoved += (teamsIdSplited[i] + ",");
        }
    }

    $('#TeamsId').val(teamsRemoved);
    $('#GoalList').val(goalsRemoved);

    loadTeamsFromCampaign();
}

function RemoveTableRow(handler) {
    var tr = $(handler).closest('tr');

    tr.fadeOut(400, function () {
        tr.remove(true);
    });

    return false;
};

function checkIfIsEnable() {
    var profile = $('#ProfileId');

    var condition = false;

    if (profile && profile.val() > 0) {
        condition = true;
    }

    $("#addPlayers").attr("disabled", !condition);

    profileId = $('#ProfileId').val();
}

function showIcon(icon, counter) {
    var profile = $('.' + counter);

    profile.removeClass();
    profile.addClass(counter + " col-xs-6 fa");
    profile.addClass(icon.options[icon.selectedIndex].value.replace(/_/g, "-"));

    profile.show();
}

checkIfIsEnable();

function removeTeamFromServer(associationId) {
    $.ajax(
    {
        type: 'GET',
        url: '/admin/campanhas/removerAssociacao/' + associationId,
        dataType: 'html',
        cache: false,
        async: true,
        success: function (data) {
            loadTeamsFromCampaign();
        },
        error: function (data) {
            alert("Erro ao remover a equipe.");
        }
    });
}

function removeMetric(metricId, element) {
    $.ajax(
    {
        type: 'GET',
        url: '/admin/campanhas/removerMetrica/' + metricId,
        dataType: 'html',
        cache: false,
        async: true,
        success: function (data) {
            if (data) {
                alert(data);
            } else {
                deleteRow(element);
            }
        },
        error: function (data) {
            alert("Erro ao remover a métrica.");
        }
    });
}

function addTeam() {
    if (getGoalsInputed()) {
        $('#closeModal').click();
        loadTeamsFromCampaign();
    } else {
        alert("É necessário cadastrar a meta das equipes que serão adicionadas.");
    }
}