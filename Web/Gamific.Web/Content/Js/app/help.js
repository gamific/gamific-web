var table;
function loadHelpDataTable() {
    table = $('#helpDataTable').dataTable({
        "serverSide": true,
        "ajax": "/admin/ajuda/search/" + $("#TopicHelpId").val(),
        "processing": true,
        "pagingType": 'simple',
        "scrollY": "300px",
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
        "dom": '<"newtoolbar">frtip',
        "fnServerParams": function (aoData) {
        },
        "columnDefs": [

                {
                    "width": "90%",
                    "targets": 0,
                    "orderable": true,
                    "searchable": true
                },
                {
                    "width": "10%",
                    "targets": 1,
                    "searchable": false,
                    "orderable": false,
                    "render": function (data, type, row) {

                        var links = "<a class='fa fa-pencil' onclick='showEntityModal(this); return false;' href='/admin/ajuda/editar/" + data + "' title='Editar Ajuda.'> </a> &nbsp; <a class='fa fa-remove' href='/admin/ajuda/remover/" + data + "' title='Remover Ajuda.'> </a>";

                        return links;
                    }
                }
        ]
    });

};

loadHelpDataTable();

function loadTinyMCE() {
    var tinymce_win;
    var tinymce_field_name;

    tinymce.init({
        selector: "textarea.content",
        height: 400,
        language: 'pt_BR',
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table contextmenu paste"
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
        file_browser_callback: function (field_name, url, type, win) {
            tinymce_win = win;
            tinymce_field_name = field_name;
            showModal("/News/Gallery");
        }
    });
};

var faqsSections = $('.cd-faq-group'),
faqTrigger = $('.cd-faq-trigger'),
faqsContainer = $('.cd-faq-items'),
faqsCategoriesContainer = $('.cd-faq-categories'),
faqsCategories = faqsCategoriesContainer.find('a'),
closeFaqsContainer = $('.cd-close-panel');

$('body').bind('click touchstart', function (event) {
    if ($(event.target).is('body.cd-overlay') || $(event.target).is('.cd-close-panel')) {
        closePanel(event);
    }
});
faqsContainer.on('swiperight', function (event) {
    closePanel(event);
});

faqTrigger.on('click', function (event) {
    event.preventDefault();
    $(this).next('.cd-faq-content').slideToggle(1000).end().parent('li').toggleClass('content-visible');
});

function onSuccessSaveHelp(data) {

    verifyErrors();
}

function onFailureSaveHelp(data) {

}