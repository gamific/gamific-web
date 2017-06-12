var checkedMap = new Map();
var expanded = false;

function showCheckboxes() {
    var checkboxes = document.getElementById("checkboxes");
    if (!expanded) {
        checkboxes.style.display = "block";
        expanded = true;
    } else {
        checkboxes.style.display = "none";
        expanded = false;
    }
}

function checkedChange(checkBox) {
    if ($(checkBox).val() == "true"){
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
                    "width": "40%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true
                },
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
                }
        ]
    });

};

loadPlayersDataTable();

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
                    "width": "45%",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var render = data.split(";")[0] + "<img src='" + logoPath + data.split(";")[1] + "?cache=@Html.Raw(DateTime.Now.Millisecond)' style='width: 48px !important; height: 48px !important; border-radius:100%; float: left; margin-right: 10px;' />";
                        return render;
                    }
                },
                {
                    "targets": 1,
                    "width": "45%",
                    "orderable": false
                },
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
                }
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
            "workerTypeId" : $('#dropDownWorkerTypes').val(),
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
    if ($(value).is(':checked') == false){
        checkedMap.delete($(value).val());
    }
    else{
        checkedMap.set($(value).val(), $(value).is(':checked'));
    }
}

function submitCreateTeam() {
    if (expanded) {
        checkboxes.style.display = "none";
        expanded = false;
    } 

    var formData = new FormData($('#formCreateTeam')[0]);
    $.ajax({
        url: "/admin/equipes/salvar/",
        type: "POST",
        data: formData,
        async: true,
        processData: false,
        contentType: false,
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data.Success == true) {
                $('#teamDataTable').dataTable().fnDestroy();
                loadTeamsDataTable();
                alertMessage("Equipe adicionada com sucesso.", "success");
            }
            else {
                alertMessage("Não foi possivel adicionar esta equipe.", "danger");
            }
        },
        error: function (data1, data2, data3) {
            $('#teamDataTable').dataTable().fnDestroy();
            loadTeamsDataTable();
            alertMessage("Ocorreu um erro ao adicionar esta equipe.", "danger");
        }
    });
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

function updateDropDownCheckBoxTeams()
{
    $("#checkboxes").empty();
    $.ajax({
        url: "/admin/equipes/getTeamsToSelect/" + $("#episodeId").val(),
        type: "GET",
        async: false,
        dataType: "json",
        success: function (data){
            var teams = JSON.parse(data);
            var subTeams = JSON.parse($("#subTeams").val());

            var html = "";

            for (var i = 0; i < teams.length; i++)
            {
                html += "<label>";
                if (subTeams.indexOf(teams[i].Value) > -1) {
                    html += "<input type='checkbox' name='checkBoxes[" + i + "].Checked' checked id='checkBoxes[" + i + "].Checked' onchange='checkedChange(this)' value='true' />" + teams[i].Text;
                }
                else {
                    html += "<input type='checkbox' name='checkBoxes[" + i + "].Checked' id='checkBoxes[" + i + "].Checked' onchange='checkedChange(this)' value='false' />" + teams[i].Text;
                }
                html +=     "<input type='hidden' name='checkBoxes[" + i + "].Text' id='checkBoxes[" + i + "].Text' value='" + teams[i].Value + "'>";
                html += "</label>";
            }

            $("#checkboxes").append(html);
        }
    });
}