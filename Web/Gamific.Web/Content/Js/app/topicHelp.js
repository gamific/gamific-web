var table;
function loadTopicHelpDataTable() {
    table = $('#topicHelpDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/topicoAjuda/search",
        "processing": true,
        "ordering": true,
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

                        var links = "<a class='fa fa-plus' href='/admin/ajuda/" + data + "' title='Cadastrar Ajudas para o Topico de Ajuda.'> </a> &nbsp; <a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/topicoAjuda/editar/" + data + "' title='Editar Topico de Ajuda.'> </a> &nbsp; <a class='fa fa-remove' href='/admin/topicoAjuda/remover/" + data + "' title='Remover Topico de Ajuda.'> </a>";

                        return links;
                    }
                }
        ]
    });

};

loadTopicHelpDataTable();

function onSuccessSaveTopicHelp(data) {

    verifyErrors();
}

function onFailureSaveTopicHelp(data) {

}