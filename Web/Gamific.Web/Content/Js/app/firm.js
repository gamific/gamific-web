var table;
function loadFirmsDataTable() {
    table = $('#empresasDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/empresas/search",
        "processing": true,
        "pagingType": 'simple',
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
        "dom": '<"newtoolbar">frtip',
        "fnServerParams": function (aoData) {
            aoData.search.value += "";
        },
        "columnDefs": [

            {
                "width": "70%",
                "targets": 0,
                "orderable": true,
                "searchable": true,
            },

            {
                "width": "20%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            },
            {
                "width": "10%",
                "targets": 2,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {

                    var links = "<a class='fa fa-pencil' href='/admin/empresas/editar/" + data + "' title='Editar Empresa.'> </a>";

                    return links;
                }
            }
        ]
    });
};



function loadLogo(inputFile) {
    if (inputFile.files && inputFile.files[0]) {
        document.getElementById('img').src = '/api/media/0';
        document.getElementById('img').src = URL.createObjectURL(inputFile.files[0]);
        document.getElementById('img').style.display = 'block';
    }
}

VMasker(document.getElementById('DataInfo_Phone')).maskPattern('(99) 99999 - 9999');
VMasker(document.getElementById('ProfileInfo_Phone')).maskPattern('(99) 99999 - 9999');
VMasker(document.getElementById('DataInfo_Cnpj')).maskPattern('99.999.999/9999-99');
VMasker(document.getElementById('ProfileInfo_CPF')).maskPattern('999.999.999-99');

function onSuccessSaveWorker(data) {

    verifyErrors();
}

function onFailureSaveWorker(data) {

}