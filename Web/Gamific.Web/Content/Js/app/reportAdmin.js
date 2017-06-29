//$(document).ready(tableUsers());
$(document).ready(function () {
    DropDownGame();
});

function searchTable() {
    $('#div-tableUsers').empty();
    var data;
    data = "<p class='text-center' ><i class='fa fa-spinner w3-spin' style='font-size:64px; margin: 250px;'></i></p>";
    $('#div-tableUsers').append(data);
}

function isEmpty() {
    $('#div-tableUsers').empty();
    var data;
    data = "<p class='text-center' style='font-size:13px; margin: 250px;'>É necessario selecionar duas datas. Caso queira em apenas um dia, selecione a mesma data nas duas caixas de seleção.</p>";
    $('#div-tableUsers').append(data);
}
/*
function createTable(initDate, finishDate, gameId) {
    
    if (initDate != null && initDate != "" && finishDate != null && finishDate != "") {
        searchTable();
    $.ajax({
        url: "/admin/relatorio/create/" + initDate + "/" + finishDate + "/" + gameId,
        async: false,
        type: "GET",
        success: function (data) {
            $('#div-tableUsers').empty();
            $('#div-tableUsers').append(data);
            $("#demo").append("html");
        }
    });
    }
    else {
        isEmpty()
    }
    
}*/

function DropDownGame() {
    $.ajax({
        url: "/admin/relatorio/buscarEmpresa",
        async: false,
        type: "GET",
        success: function (data) {
            var empresas = JSON.parse(data);

            var html = "";
            $("#dropDownGame").append("<option value='" + "------" + "'>" + "Todas" + "</option>");
            for (var i = 0; i < empresas.length; i++) {
                $("#dropDownGame").append("<option value='" + empresas[i].externalId + "'>" + empresas[i].firmName + "</option>");
            }

            $("#dropDownGame").append(html);

            
        }
    });
}

function createTable(initDate, finishDate, gameId) {

    if (initDate != null && initDate != "" && finishDate != null && finishDate != "") {
        searchTable();
        $.ajax({
            url: "/admin/relatorio/buscarEmpresa/" + initDate + "/" + finishDate + "/" + gameId,
            async: false,
            type: "GET",
            success: function (data) {
                $('#div-tableUsers').empty();
                $('#div-tableUsers').append(data);
                $("#demo").append("html");
            }
        });
    }
    else {
        isEmpty()
    }

}