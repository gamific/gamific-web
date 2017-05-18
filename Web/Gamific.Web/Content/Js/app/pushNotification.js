var checkedMap = new Map();

$('#dropDownEpisodes').change(function () {
    checkedMap.clear();
    refreshDropDownTeams($(this).val());
    $('#notificationDataTable').dataTable().fnDestroy();
    loadNotificationDataTable();
});

$(document).ready(function () {
    refreshDropDownEpisodes();
    $('#nameNotificationDataTable').html("Nome das equipes");
    loadNotificationDataTable();
});

$('#dropDownTeams').change(function () {
    checkedMap.clear();
    if ($('#dropDownTeams').val() === "empty") {
        $('#nameNotificationDataTable').html("Nome das equipes");
    }
    else {
        $('#nameNotificationDataTable').html("Nome dos jogadores");
    }
    
    $('#notificationDataTable').dataTable().fnDestroy();
    loadNotificationDataTable();
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
                $("#dropDownEpisodes").append($("<option value=''>Vazio</option>"));
                $("#dropDownTeams").append($("<option value=''>Vazio</option>"));
            }
        },
        error: function () {
            $("#dropDownEpisodes").empty();
        }
    });
}

function refreshDropDownTeams(episodeId, currentId) {
    $.ajax({
        url: "/admin/notificacoes/buscarEquipes",
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

function loadNotificationDataTable() {
    table = $('#notificationDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/notificacoes/search?episodeId=" + $('#dropDownEpisodes').val() + "&teamId=" + $("#dropDownTeams").val(),
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
        "dom": '<"newtoolbar">rt',
        "fnServerParams": function (aoData) {
        },
        "columnDefs": [
                {
                    "width": "10%",
                    "targets": 0,
                    "orderable": false,
                    "searchable": false,
                    "render": function (data, type, row) {
                        var isChecked = "";
                        if (checkedMap.get(data)) {
                            isChecked = "checked";
                        }

                        var links = "<input type='checkbox' class='playersIdList' onchange='checkBoxChange(this)' value='" + data + "' + " + isChecked + ">";

                        return links;
                    }
                },
                {
                    "width": "90%",
                    "targets": 1,
                    "orderable": true,
                    "searchable": true
                }
        ],
    });
};

function SubmitNotification()
{
    var checkedsList = [];

    checkedMap.forEach(function (value, key) {
        checkedsList.push(key);
    });

    var message = $("#messageNotification").val();
    var title = $("#titleNotification").val();

    $("#messageNotification").val('');
    $("#titleNotification").val('');

    checkedMap.clear();

    $('#notificationDataTable').dataTable().fnDestroy();
    loadNotificationDataTable();

    $.ajax({
        url: "/admin/notificacoes/send",
        async: true,
        type: "POST",
        data:
        {
            "teamId": $("#dropDownTeams").val(),
            "checkedIds": checkedsList,
            "message": message,
            "title": title
        },
        success: function (message, teste) {

            if (message.error == false) {
                alertMessage(message.text, "success");
            }
            else {
                alertMessage(message.text, "danger");
            }
            
        },
        error: function (data) {
            alertMessage("Erro ao enviar mensagens.", "danger");
        }
    });
}

function checkBoxChange(value) {
    if ($(value).is(':checked') == false) {
        checkedMap.delete($(value).val());
    }
    else {
        checkedMap.set($(value).val(), $(value).is(':checked'));
    }
}

function selectAll()
{
    if ($('#dropDownTeams').val() != 'empty')
    {
        $.ajax({
            url: "/admin/notificacoes/getAllPlayerIdsFromTeam/" + $('#dropDownTeams').val(),
            async: true,
            type: "GET",
            success: function (data) {
                var playerIds = JSON.parse(data);
                playerIds.forEach(function (value, key) {
                    checkedMap.set(value, true);
                });

                $('#notificationDataTable').dataTable().fnDestroy();
                loadNotificationDataTable();
            },
            error: function () {
                
            }
        });
    }
    else{
        $.ajax({
            url: "/admin/notificacoes/getAllTeamIdsFromEpisode/" + $('#dropDownEpisodes').val(),
            async: true,
            type: "GET",
            success: function (data) {
                var playerIds = JSON.parse(data);
                playerIds.forEach(function (value, key) {
                    checkedMap.set(value, true);
                });

                $('#notificationDataTable').dataTable().fnDestroy();
                loadNotificationDataTable();
            },
            error: function () {
            }
        });
    }
}
