var questionSelected = [];
var listQuestion = [];


function loadDataTableQuestionAssociate() {

    $("ol.questionAssociate").sortable();

    table = $('#questionAssociateDataTable').dataTable({
        "serverSide": true,
        "searching": true,
        "ajax": "/admin/answer/search/",
        "dom": "lfrti",
        "sRowSelect": 'multi',
        //"aButtons": [
        //    {
        //        sExtends: 'select_all',
        //        sButtonText: 'Select All',
        //        fnClick: function (nButton, oConfig, oFlash) {
        //            TableTools.fnGetInstance('example').fnSelectAll(true);
        //        }
        //    }
        //],

        "rowCallback": function (row, data) {
            var exist = false;
            $.each(listQuestion, function (index, item) {
                if (item[0] === data[0]) {
                    exist = true;
                }
            });

            if (!exist) {
                listQuestion.push(data);
            }

            if ($.inArray(data[0], questionSelected) !== -1) {

                $(row).css("background-color", "#22beef");
                $(row).css("color", " #f2f2f2");
            }
        },
        "language": {
            "emptyTable": "Não foram encontrados resultados.",
            "paginate": {
                "previous": '<i class="fa fa-angle-left"></i>',
                "next": '<i class="fa fa-angle-right"></i>'
            }
        },
        "columnDefs": [
            {
                "width": "15%",
                "targets": 0,
                "orderable": true,
                "searchable": false,
            },
            {
                "width": "40%",
                "targets": 1,
                "orderable": true,
                "searchable": true,
            }
        ]
    });

    $('#questionAssociateDataTable tbody').on('click', 'tr', function () {
        var id = this.childNodes[0].innerText;
        var index = $.inArray(id, questionSelected);

        var exist = false;
        $.each(questionSelected, function (indexS, item) {
            if (item == id) {
                exist = true;
            }
        });

        if (!exist) {
            questionSelected.push(id);
            $(this).css("background-color", "#22beef");
            $(this).css("color", " #f2f2f2");
        } else {
            questionSelected.splice(questionSelected.indexOf(this.childNodes[0].innerText), 1);
            $(this).css("background-color", "#fff");
            $(this).css("color", " #848484");
        }



    });

}

function onSucessSaveToast(data, status, xhr) {
    if (data && data.status === 'error') {
        toastr.error(data.message, 'Erro');
    } else if (data && data.status === 'warn') {
        toastr.warning(data.message, 'Aviso');
    }
    else {
        if (data && data.status === 'sucess') {
            toastr.success(data.message, 'Sucesso');
        }
        $('#questionAssociateDataTable').dataTable().fnDestroy();
        loadDataTableQuestionAssociate();
        $('.modal').modal('hide');
    }
}

function onFailureSaveToast() {
    toastr.error('Ocorreu um erro inesperado no sistema!', 'Erro');
}

loadDataTableQuestionAssociate();
