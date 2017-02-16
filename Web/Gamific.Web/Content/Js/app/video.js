var table;
function loadVideoDataTable() {
    table = $('#videoDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/videos/search",
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

                        var links = "<a class='fa fa-plus' href='/admin/perguntasVideo/" + data + "' title='Cadastrar Perguntas para o Video.'> </a> &nbsp; <a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/videos/editar/" + data + "' title='Editar Video.'> </a> &nbsp; <a class='fa fa-remove' href='/admin/videos/remover/" + data + "' title='Remover Video.'> </a>";

                        return links;
                    }
                }
        ]
    });

};

loadVideoDataTable();

function onSuccessSaveVideo(data) {
    verifyErrors();
}

function onFailureSaveVideo(data) {

}