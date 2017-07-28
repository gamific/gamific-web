//$(document).ready(tableUsers());
$(document).ready(function () {
    DropDownGame();

    $("#demo-dt-addrow-btn-2").click(function (e) {
        e.preventDefault();

        //getting data from our table
        var data_type = 'data:application/vnd.ms-excel';
        var table_div = document.getElementById('table_wrapper');
        var table_html = table_div.outerHTML.replace(/ /g, '%20');

        var a = document.createElement('a');
        a.href = data_type + ', ' + table_html;
        a.download = 'exported_table_' + Math.floor((Math.random() * 9999999) + 1000000) + '.xls';
        a.click();
    });
});

function searchTable() {
    $('#div-tableUsers').empty();
    var data;
    //data = "<p class='text-center' ><i class='fa fa-spinner w3-spin' style='font-size:64px; margin: 250px;'></i></p>";
    data = + data + "<p class='text-center' >ERRO</p>";
    $('#div-tableUsers').append(data);
}

function isEmpty() {
    $('#div-tableUsers').empty();
    var data;
    data = "<p class='text-center' style='font-size:13px; margin: 250px;'>É necessario selecionar duas datas. Caso queira em apenas um dia, selecione a mesma data nas duas caixas de seleção.</p>";
    $('#div-tableUsers').append(data);
}


function DropDownGame() {
    $.ajax({
        url: "/admin/relatorio/buscarEmpresa",
        async: false,
        type: "GET",
        success: function (data) {
            var empresas = JSON.parse(data);

            var html = "";
            $("#dropDownGame").append("<option value='" + "empty" + "'>" + "Todas" + "</option>");
            for (var i = 0; i < empresas.length; i++) {
                $("#dropDownGame").append("<option value='" + empresas[i].externalId + "'>" + empresas[i].firmName + "</option>");
            }

            $("#dropDownGame").append(html);
            $("#dropDownGameInative").append("<option value='" + "empty" + "'>" + "Todas" + "</option>");
            for (var i = 0; i < empresas.length; i++) {
                $("#dropDownGameInative").append("<option value='" + empresas[i].externalId + "'>" + empresas[i].firmName + "</option>");
            }
            $("#dropDownGameInative").append(html);

            
        }
    });
}

function createTable(initDate, finishDate, gameId) {
    //searchTable();
    if (initDate != null && initDate != "" && finishDate != null && finishDate != "") {
        
        $.ajax({
            url: "/admin/relatorio/buscarUsuario/" + initDate + "/" + finishDate + "/" + gameId + "/" + true,
            async: false,
            type: "GET",
            success: function (data) {
                $('#div-tableUsers').empty();
                var report = JSON.parse(data);
                var util = "<table>";
                util = util + "<tr class='bg-transparent-black-5'> <th>Nome</th> <th>Email</th> <th>Empresa</th> <th>Web</th> <th>Mobile</th> <th>Ultimo Email</th> <th>Quantidade</th> </tr>";

                for (var i = 0; i < report.length; i++) {

                    util = util + "<tr>";

                    util = util + "<th>" + report[i].Name + "</th>";

                    util = util + "<th>" + report[i].Email + "</th>";

                    util = util + "<th>" + report[i].GameName + "</th>";

                    util = util + "<th>" + report[i].LastUpdateWebString + "</th>";

                    util = util + "<th>" + report[i].LastUpdateMobileString + "</th>";

                    util = util + "<th>" + report[i].LastReciveEmailString + "</th>";

                    util = util + "<th>" + report[i].CountEmails + "</th>";

                    util = util + "</tr>";
                }

                util = util + "</table>";
                
                $('#div-tableUsers').append(util);
            }
        });
    }
    else {
        isEmpty()
    }

}

function createTableInative(initDate, finishDate, gameId) {
    //searchTable();
    if (initDate != null && initDate != "" && finishDate != null && finishDate != "") {

        $.ajax({
            //url: "/admin/relatorio/buscarUsuarioInativo/" + initDate + "/" + finishDate + "/" + gameId,
            url: "/admin/relatorio/buscarUsuario/" + initDate + "/" + finishDate + "/" + gameId + "/" + false,
            async: false,
            type: "GET",
            success: function (data) {
                $('#div-tableUsersInative').empty();
                var report = JSON.parse(data);
                var util = "<table>";
                util = util + "<tr class='bg-transparent-black-5'> <th>Nome</th> <th>Email</th> <th>Empresa</th> <th>Web</th> <th>Mobile</th> <th>Ultimo Email</th> <th>Quantidade</th> </tr>";

                for (var i = 0; i < report.length; i++) {

                    util = util + "<tr>";

                    util = util + "<th>" + report[i].Name + "</th>";

                    util = util + "<th>" + report[i].Email + "</th>";

                    util = util + "<th>" + report[i].GameName + "</th>";

                    util = util + "<th>" + report[i].LastUpdateWebString + "</th>";

                    util = util + "<th>" + report[i].LastUpdateMobileString + "</th>";

                    util = util + "<th>" + report[i].LastReciveEmailString + "</th>";

                    util = util + "<th>" + report[i].CountEmails + "</th>";

                    util = util + "</tr>";
                }

                util = util + "</table>";

                $('#div-tableUsersInative').append(util);
            }
        });
    }
    else {
        isEmpty()
    }

}