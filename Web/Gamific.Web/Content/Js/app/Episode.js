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
                "width": "10%",
                "targets": 1,
                "orderable": true,
                "searchable": true
            },
            {
                "width": "10%",
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
            },
            {
                "width": "13%",
                "targets": 6,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/episode/editar/" + data + "' title='Edite está campanha'> </a>";
                    links += "<a class='fa fa-plus' href='/admin/episodeQuiz?episodeId=" + data + "' title='Edite está campanha'> </a>";
                    links +=  " <a class='fa fa-clone' onclick='showEntityModal(this); return false;'  href='/admin/episode/clonar/" + data + "' title='Clone está campanha, com todas as equipes,metas e metricas'> </a>";
                    links += " <a class='fa fa-eraser' href='#' onclick='cleanClickEpisode(\"" + data + "\")' title='Zera todos os resultados desta campanha.'> </a>";
                    if (row[4] == "Sim") {
                        links += " <a class='fa fa-power-off'  href='#' onclick='removeClickEpisode(\"" + data + "\",\"" + name + "\")' title='Inativar esta campanha.'> </a>";
                    }
                    if (row[4] == "Não") {
                        links += " <a class='fa fa-tachometer'  href='/public/dashboardHistorico/" + data + "/empty/empty' title='Ver Dashboard.'> </a>";
                    }
                    return links;
                }
            }
            /*
            {
                "width": "5%",
                "targets": 5,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    data = row[0].data;
                    type = row[0].type;
                    var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/episode/editar/" + data + "' title='Edite está campanha'> </a>";
                    
                    return links;
                }
            },
            {
                "width": "5%",
                "targets": 6,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    data = row[0].data;
                    type = row[0].type;
                    var links = " <a class='fa fa-clone' onclick='showEntityModal(this); return false;'  href='/admin/episode/clonar/" + data + "' title='Clone está campanha, com todas as equipes,metas e metricas'> </a>";
                    
                    return links;
                }
            },
            {
                "width": "5%",
                "targets": 7,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    data = row[0].data;
                    type = row[0].type;
                    var links = " <a class='fa fa-eraser' href='#' onclick='cleanClickEpisode(\"" + data + "\")' title='Zera todos os resultados desta campanha.'> </a>";
                   
                    return links;
                }
            },
            {
                "width": "5%",
                "targets": 8,
                "orderable": false,
                "searchable": false,
                "render": function (data, type, row) {
                    var name = row[0].split(";")[0];
                    data = row[0].data;
                    type = row[0].type;
                    if (row[4] == "Sim") {
                        var links = " <a class='fa fa-power-off'  href='#' onclick='removeClickEpisode(\"" + data + "\",\"" + name + "\")' title='Finaliza está campanha.'> </a>";
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
}

function onFailureSaveEpisode() {

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
                if(key != 0){
                    daysChecked += ",";
                }
                daysChecked += val;
            }
        });

        $('#DaysOfWeek').val(daysChecked);
    }

}
