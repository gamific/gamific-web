function loadEpisodeDataTable() {
    loadData();
    table = $('#episodeDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/episode/search/",
        "scrollY": "300px",
        "processing": true,
        "ordering": true,
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
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "fnServerParams": function (aoData) { },
        "columnDefs": [
            {
                "width": "17%",
                "targets": 0,
                "orderable": true,
                "searchable": true
            },
            {
                "width": "15%",
                "targets": 1,
                "orderable": true,
                "searchable": true
            },
            {
                "width": "15%",
                "targets": 2,
                "orderable": true,
                "searchable": true
            },
            {
                "width": "15%",
                "targets": 3,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "15%",
                "targets": 4,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 5,
                "orderable": true,
                "searchable": true,
            }/*,
            {
                "width": "13%",
                "targets": 6,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/episode/editar/" + data + "' title='Edite está campanha'> </a>";
                    links += " <a class='fa fa-clone' onclick='showEntityModal(this); return false;'  href='/admin/episode/clonar/" + data + "' title='Clone está campanha, com todas as equipes,metas e metricas'> </a>";
                    links += " <a class='fa fa-eraser' href='#' onclick='cleanClickEpisode(\"" + data + "\")' title='Zera todos os resultados desta campanha.'> </a>";
                    if (row[4] == "Sim") {
                        links += " <a class='fa fa-power-off'  href='#' onclick='removeClickEpisode(\"" + data + "\",\"" + name + "\")' title='Inativar esta campanha.'> </a>";
                    }
                    if (row[4] == "Não") {
                        links += " <a class='fa fa-tachometer'  href='/public/dashboardHistorico/" + data + "/empty/empty' title='Ver Dashboard.'> </a>";
                    }
                    return links;
                }
            }*/

        ]
    });
}

function removeClickEpisode(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo inativar a campanha " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/episode/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Campanha inativada com sucesso.", "success");

                        $('#episodeDataTable').dataTable().fnDestroy();
                        loadEpisodeDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao inativar a campanha.", "danger");
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

function cleanClickEpisode(data) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo apagar todos os resultados da campanha " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/episode/clean/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Resultados da campanha apagados com sucesso.", "success");

                        $('#episodeDataTable').dataTable().fnDestroy();
                        loadEpisodeDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao tentar apagar resultados da campanha.", "danger");
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


function onSucessSaveEpisode() {
    verifyErrors();
   /* $('#episodeDataTable').dataTable().fnDestroy();
    loadEpisodeDataTable();*/
}

function onFailureSaveEpisode() {
   /* $('#episodeDataTable').dataTable().fnDestroy();
    loadEpisodeDataTable();*/
}

function loadData() {
}

$(document).ready(function () {
    loadEpisodeDataTable();


    /*
    daysOfWeek.forEach(function (value, key) {
        $('#' + value).prop('checked', true);
    });*/
});

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

// ----- Worker Type ----- //

function loadWorkerTypeDataTable() {
    $('#workerTypeDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/funcoes/search/" + $('#NumberOfWorkerTypes').val(),
        "scrollY": "300px",
        "processing": true,
        "ordering": true,
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
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "columnDefs": [
            {
                "width": "50%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "50%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            }/*,
            {
                "width": "10%",
                "targets": 2,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/funcoes/editar/" + data + "' title='Editar Função.'> </a> &nbsp; <a class='fa fa-remove' href='#' onclick='removeClickWorkerType(\"" + data + "\",\"" + name + "\")' title='Remover Função.'> </a>";

                    return links;
                }
            }*/
        ]
    });
};

function removeClickWorkerType(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover a função " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/funcoes/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Função removida com sucesso.", "success");

                        $('#workerTypeDataTable').dataTable().fnDestroy();
                        loadWorkerTypeDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover função.", "danger");
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


//loadWorkerTypeDataTable();

function onSuccessSaveWorkerType(data) {
    verifyErrors();
    $('#workerTypeDataTable').dataTable().fnDestroy();
    loadWorkerTypeDataTable();
}

function onFailureSaveWorkerType(data) {
    $('#workerTypeDataTable').dataTable().fnDestroy();
    loadWorkerTypeDataTable();

}


// ----- Worker ------ //

function loadWorkersDataTable() {
    var logoPath = "";
    $.ajax({
        url: window.location.origin + "/apiMedia/imagePath",
        async: false,
        type: "GET",
        success: function (data) {
            logoPath = data;
        }
    });

    $('#funcionariosDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/funcionarios/search/" + $('#NumberOfWorkers').val(),
        "scrollY": "300px",
        "processing": true,
        "ordering": true,
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
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "fnServerParams": function (aoData) {
        },
        "columnDefs": [
            {
                "width": "50%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
                "render": function (data, type, row) {

                    var render = data.split(";")[0] + "<img src='" + logoPath + data.split(";")[1] + "?cache=@Html.Raw(DateTime.Now.Millisecond)' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' />";

                    return render;
                }
            },
            {
                "width": "25%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "25%",
                "targets": 2,
                "orderable": true,
                "searchable": true,
            }/*,
            {
                "width": "5%",
                "targets": 3,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-exchange' onclick='changePassword(\"" + data + "\",\"" + name + "\")' href='#' title='Alterar senha.'> </a> &nbsp; <a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/funcionarios/editar/" + data + "' title='Editar Funcionário.'> </a> &nbsp; <a class='fa fa-remove' href='#' onclick='removeClickTeam(\"" + data + "\",\"" + name + "\")' title='Remover Funcionário.'> </a>";

                    return links;
                }
            }*/
        ]
    });
};

function changePassword(data, name) {

    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo alterar a senha de " + name + " para 'Gamific123'?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/funcionarios/changePassword/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Senha de " + name + " alterada com sucesso.", "success");
                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao alterar a senha.", "danger");
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

function removeClickTeam(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover o jogador " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/funcionarios/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Jogador removido com sucesso.", "success");

                        $('#funcionariosDataTable').dataTable().fnDestroy();
                        loadWorkersDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover jogador.", "danger");
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

function onSuccessSaveWorker(data) {

    verifyErrors();
}

function onFailureSaveWorker(data) {

}

function onSuccessSaveWorkerArchive(data) {

    verifyErrors();
}

function onFailureSaveWorkerArchive(data) {

}

function loadLogo(inputFile) {
    if (inputFile.files && inputFile.files[0]) {
        document.getElementById('img').src = '/api/media/0';
        document.getElementById('img').src = URL.createObjectURL(inputFile.files[0]);
        document.getElementById('img').style.display = 'block';
    }
}

//$(document).ready(function () {
  //  loadWorkersDataTable();
//})

// ----- Team ----- //

var checkedMap = new Map();
var expanded = false;

function showCheckboxesTeam() {
    var checkboxes = document.getElementById("checkboxes-team");
    if (!expanded) {
        checkboxes.style.display = "block";
        expanded = true;
    } else {
        checkboxes.style.display = "none";
        expanded = false;
    }
}

function checkedChange(checkBox) {
    if ($(checkBox).val() == "true") {
        $(checkBox).val("false");
    }
    else {
        $(checkBox).val("true");
    }
}



function loadAssociatePlayersDataTable() {
    $('#associatePlayersDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/equipes/carregarJogadores/" + $('#TeamId').val() + "/" + $('#Count').val() + "/" + $('#dropDownWorkerTypes').val(),
        "scrollY": "300px",
        "ordering": false,
        "processing": true,
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
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "fnServerParams": function (aoData) {
        },
        "columnDefs": [
            {
                "width": "5%",
                "targets": 0,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var isChecked = "";
                    if (checkedMap.get(data)) {
                        isChecked = "checked";
                    }

                    var links = "<input type='checkbox' class='workersIdList' onchange='checkBoxChange(this)' value='" + data + "' + " + isChecked + ">";

                    return links;
                }
            },
            {
                "width": "50%",
                "targets": 1,
                "orderable": true,
                "searchable": true
            },
            {
                "width": "45%",
                "targets": 2,
                "orderable": true,
                "searchable": true
            }
        ]
    });

};

function loadPlayersDataTable() {
    var logoPath = "";
    $.ajax({
        url: window.location.origin + "/apiMedia/imagePath",
        async: false,
        type: "GET",
        success: function (data) {
            logoPath = data;
        }
    });

    $('#playersDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/equipes/jogadores/" + $('#CurrentTeam').val(),
        "scrollY": "300px",
        "ordering": false,
        "processing": true,
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
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "fnServerParams": function (aoData) {
            aoData.search.value += "";
        },
        "columnDefs": [
            {
                "width": "50%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
                "render": function (data, type, row) {
                    var render = data.split(";")[0] + "<img src='" + logoPath + data.split(";")[1] + "?cache=@Html.Raw(DateTime.Now.Millisecond)' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' />";
                    return render;
                }
            },
            {
                "width": "50%",
                "targets": 1,
                "orderable": true,
                "searchable": true
            }/*,
            {
                "width": "10%",
                "targets": 2,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    var links = "";

                    links = "<a class='fa fa-remove' href='#' onclick='removeClickAssociation(\"" + data + "\")' title='Remover Associação.'> </a>";

                    return links;
                }
            }*/
        ]
    });

};

//loadPlayersDataTable();

function loadTeamsDataTable() {
    var logoPath = "";

    $.ajax({
        url: window.location.origin + "/apiMedia/imagePath",
        async: false,
        type: "GET",
        success: function (data) {
            logoPath = data;
        }
    });

    $('#teamDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/equipes/search/" + $('#dropDownEpisodes').val(),
        "scrollY": "300px",
        "ordering": false,
        "processing": true,
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
        "dom": '<"top">rt<"bottom"ip><"clear">',
        "columnDefs": [
            {
                "targets": 0,
                "width": "50%",
                "orderable": false,
                "render": function (data, type, row) {
                    var render = data.split(";")[0] + "<img src='" + logoPath + data.split(";")[1] + "?cache=@Html.Raw(DateTime.Now.Millisecond)' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' />";
                    return render;
                }
            },
            {
                "targets": 1,
                "width": "50%",
                "orderable": false
            }/*,
            {
                "targets": 2,
                "width": "10%",
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];

                    var links = "<a class='fa fa-plus' href='/admin/equipes/detalheEquipe/" + data + "' title='Cadastrar Jogadores para Equipe.'> </a> &nbsp;";
                    links += " <a class='fa fa-pencil' onclick='showEntityModal(this); return false' href='/admin/equipes/editar/" + data + "' title='Editar Equipe.'> </a> &nbsp; <a class='fa fa-remove'  href='#' onclick='removeClickWorker(\"" + data + "\",\"" + name + "\")' title='Remover Equipe.'> </a>";

                    return links;
                }
            }*/
        ]
    });

};



function saveAssociations() {
    var checkedsList = [];

    checkedMap.forEach(function (value, key) {
        checkedsList.push(key);
    });

    $.ajax({
        url: "/admin/equipes/salvarAssociacoes/",
        async: true,
        type: "POST",
        data: {
            "workerTypeId": $('#dropDownWorkerTypes').val(),
            "workersId": checkedsList,
            "teamId": $('#CurrentTeam').val()
        },
        success: function () {
            alertMessage("Funcionarios adicionados a equipe com sucesso.", "success");

            $('#playersDataTable').dataTable().fnDestroy();
            loadPlayersDataTable();
        },
        error: function () {
            alertMessage("Houve um erro ao remover equipe.", "danger");
        }
    });
}

function dropDownChange() {
    checkedMap.clear();

    $.ajax({
        url: "/admin/equipes/obterQuantidadeParaAssociar/" + $('#dropDownWorkerTypes').val(),
        async: false,
        type: "GET",
        success: function (data) {
            var count = JSON.parse(data);
            $('#Count').val(count);

            $('#associatePlayersDataTable').dataTable().fnDestroy();
            loadAssociatePlayersDataTable();
        }
    });
}

function removeClickAssociation(data) {
    $.ajax({
        url: "/admin/equipes/removerAssociacao/" + data + "/" + $('#CurrentTeam').val(),
        async: true,
        type: "POST",
        success: function () {
            alertMessage("Funcionario removido da equipe com sucesso.", "success");

            $('#playersDataTable').dataTable().fnDestroy();
            loadPlayersDataTable();
        },
        error: function () {
            alertMessage("Houve um erro ao remover equipe.", "danger");
        }
    })
}

function removeClickWorker(data, name) {
    var dialog = BootstrapDialog.show({
        size: BootstrapDialog.SIZE_SMALL,
        title: "<div style='font-size:20px;'>Atenção!</div>",
        message: function () { return "<div style='font-size:20px;'>Deseja mesmo remover a equipe " + name + "?</div>"; },
        buttons: [{
            label: 'Sim',
            action: function (dialog) {
                $.ajax({
                    url: "/admin/equipes/remover/" + data,
                    async: true,
                    type: "POST",
                    success: function () {
                        alertMessage("Equipe removido com sucesso.", "success");

                        $('#teamDataTable').dataTable().fnDestroy();
                        loadTeamsDataTable();

                        dialog.close();
                    },
                    error: function () {
                        alertMessage("Houve um erro ao remover equipe.", "danger");
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

function checkBoxChange(value) {
    if ($(value).is(':checked') == false) {
        checkedMap.delete($(value).val());
    }
    else {
        checkedMap.set($(value).val(), $(value).is(':checked'));
    }
}

function loadLogo(inputFile) {
    if (inputFile.files && inputFile.files[0]) {
        document.getElementById('img').src = '/api/media/0';
        document.getElementById('img').src = URL.createObjectURL(inputFile.files[0]);
        document.getElementById('img').style.display = 'block';
    }
}

function refreshDataTable() {
    $('#teamDataTable').dataTable().fnDestroy();
    loadTeamsDataTable();
}

$(document).ready(function () {
    $.ajax({
        url: "/admin/equipes/buscarEpisodios",
        async: false,
        type: "GET",
        success: function (data) {
            $("#dropDownEpisodes").empty();
            var episodes = JSON.parse(data);

            for (var i = 0; i < episodes.length; i++) {
                $("#dropDownEpisodes").append($("<option value='" + episodes[i].id + "'>" + episodes[i].name + "</option>"));
            }

            if (episodes.length >= 1) {
                refreshDataTable();
            }
        },
        error: function () {
            $("#dropDownEpisodes").empty();
        }
    });
});

$('#dropDownEpisodes').change(function () {
    refreshDataTable();
});

function updateDropDownCheckBoxTeams() {
    $("#checkboxes-team").empty();
    $.ajax({
        url: "/admin/equipes/getTeamsToSelect/" + $("#episodeId").val(),
        type: "GET",
        async: false,
        dataType: "json",
        success: function (data) {
            var teams = JSON.parse(data);
            var subTeams = JSON.parse($("#subTeams").val());

            var html = "";

            var count = 0;

            for (var k = 0; k < teams.length; k++) {
                var i = k - count;
                if (teams[k].Value != $("#Id").val() && teams[k].Value != $("#SubOfTeamId").val()) {
                    html += "<label>";
                    if (subTeams.indexOf(teams[k].Value) > -1) {
                        html += "<input type='checkbox' name='checkBoxes[" + i + "].Checked' checked id='checkBoxes[" + i + "].Checked' onchange='checkedChange(this)' value='true' />" + teams[k].Text;
                    }
                    else {
                        html += "<input type='checkbox' name='checkBoxes[" + i + "].Checked' id='checkBoxes[" + i + "].Checked' onchange='checkedChange(this)' value='false' />" + teams[k].Text;
                    }
                    html += "<input type='hidden' name='checkBoxes[" + i + "].Text' id='checkBoxes[" + i + "].Text' value='" + teams[k].Value + "'>";
                    html += "</label>";
                }
                else {
                    count++;
                }
            }

            $("#checkboxes-team").append(html);
        }
    });
}

function onSuccessSaveTeam() {

    verifyErrors();
}

function onFailureSaveTeam() {

}
