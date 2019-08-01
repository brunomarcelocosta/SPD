$(document).ready(function () {

    $.fn.DataTable.ext.pager.full_numbers_no_ellipses = function (page, pages) {
        var numbers = [];
        var buttons = $.fn.DataTable.ext.pager.numbers_length;
        var half = Math.floor(buttons / 2);

        var _range = function (len, start) {
            var end;

            if (typeof start === "undefined") {
                start = 0;
                end = len;

            } else {
                end = start;
                start = len;
            }

            var out = [];
            for (var i = start; i < end; i++) { out.push(i); }

            return out;
        };


        if (pages <= buttons) {
            numbers = _range(0, pages);

        } else if (page <= half) {
            numbers = _range(0, buttons);

        } else if (page >= pages - 1 - half) {
            numbers = _range(pages - buttons, pages);

        } else {
            numbers = _range(page - half, page + half + 1);
        }

        numbers.DT_el = 'span';

        //return ['first', 'previous', numbers, 'next', 'last'];

        return ['first', numbers, 'last'];
    };

    DataTable();

});

function DataTable() {

    var groupColumn = 0;

    var table = $('#idGrid').DataTable({
        "sDom": '<"row view-filter"<"col-sm-12"<"pull-left"l><"pull-left"f><"clearfix">>>t<"row view-pager"<"col-sm-12"<"pull-left"ip>>>',
        "sPaginationType": "full_numbers_no_ellipses",
        "pageLength": 100,
        "serverSide": true,
        "searching": false,
        "processing": true,
        "stateSave": true,
        "ordering": false,
        "order": [[groupColumn, 'asc']],
        "ajax":
        {
            "url": "/Agenda/Paginacao",
            "type": "POST",
            "dataType": "JSON",
            "data": {
                'Nome_Paciente': $('#Nome_Paciente').val(),
                'Dentista_string': $('#Dentista_string').val(),
                'DataDe': $('#DataDe').val()
            }
        },

        "columnDefs": [
            {
                "visible": false,
                "targets": groupColumn
            },
            {
                "className": "text-center",
                "targets": [1, 2, 3, 4]
            }
        ],

        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr><td colspan="4" style="background-color: #dddddd !important;"><b>' + group + '</b></td></tr>'
                    );

                    last = group;
                }
            });
        },

        "columns": [
            { "data": "Dentista", "orderable": false, "autoWidth": true },
            { "data": "Hora", "orderable": false, "autoWidth": true },
            { "data": "Paciente", "orderable": false, "autoWidth": true },
            { "data": "Celular", "orderable": false, "autoWidth": true },
            {
                "data": null, "orderable": false, "autoWidth": true, "render": function (data, type, row) {
                    return '<button type="button" id="' + row.ID + '" class="btn btn-danger btn-sm" onclick="Excluir(this)" >Excluir</button>';
                }
            }
        ],
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": " _TOTAL_ registros encontrados",
            "sInfoEmpty": "0 registro encontrado.",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Mostrar _MENU_ registros por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": ">",
                "sPrevious": "<",
                "sFirst": "«",
                "sLast": "»"
            }
        }
    });
}


function Excluir(obj) {

    var valueID = "";
    valueID = $(obj).attr('ID').toString();

    var urlList = '/Agenda/List';
    var msg = "Deseja realmente excluir este horário de consulta?";

    swal({
        title: "Confirmação",
        text: msg,
        icon: "warning",
        buttons: ["Não", "Sim"],
        dangerMode: false,
    })
        .then((willDelete) => {
            if (!willDelete) { return; }
            $.ajax({
                type: "POST",
                url: '/Agenda/Delete',
                data: { id: valueID },
                dataType: 'JSON',
                success: function (result) {

                    if (!result.Success) {

                        swal("", result.Response, "error");
                        return;

                    }
                    else {
                        swal("", "Horário de consulta exluída com sucesso.", "success")
                            .then(() => {
                                window.location = urlList;
                            });
                    }
                }
            });
        });
}
