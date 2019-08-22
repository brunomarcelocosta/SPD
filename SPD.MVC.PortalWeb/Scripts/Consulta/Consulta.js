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

    var table = $('#idGrid').DataTable({
        "sDom": '<"row view-filter"<"col-sm-12"<"pull-left"l><"pull-left"f><"clearfix">>>t<"row view-pager"<"col-sm-12"<"pull-left"ip>>>',
        "sPaginationType": "full_numbers_no_ellipses",
        "pageLength": 100,
        "serverSide": true,
        "searching": false,
        "processing": true,
        "stateSave": true,
        "ordering": false,
        "order": [[0, 'asc']],
        "ajax":
        {
            "url": "/Consulta/Paginacao",
            "type": "POST",
            "dataType": "JSON",
            "data": {
                'Dentista_string': $('#Dentista_string').val(),
                'Paciente_string': $('#Paciente_string').val(),
                'HoraDe': $('#HoraDe').val(),
                'HoraAte': $('#HoraAte').val()
            }
        },

        "columnDefs": [
            {
                "visible": false,
                "targets": [0]
            },
            {
                "className": "text-center",
                "targets": [1, 2, 3, 4, 5]
            }
        ],

        "columns": [
            { "data": "ID", "orderable": false, "autoWidth": true },
            { "data": "Hora", "orderable": false, "autoWidth": true },
            { "data": "Paciente", "orderable": false, "autoWidth": true },
            { "data": "Autorizado", "orderable": false, "autoWidth": true },
            { "data": "Convenio", "orderable": false, "autoWidth": true },
            {
                "data": null, "render": function (data, type, row) {
                    return '<button type="button" id="' + row.ID + '" class="btn btn-primary btn-sm" onclick="IniciarConsulta(this)" >Iniciar Consulta</button>';
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

function IniciarConsulta(obj) {

    var valueID = "";
    valueID = $(obj).attr('ID').toString();

    $.ajax({
        type: "POST",
        url: '/Consulta/New',
        data: { id: valueID },
        dataType: 'JSON',
        success: function (result) {

            if (!result.Success) {

                swal("", result.Response, "error");
                return;

            }
            else {
                Render('Consulta', 'New', valueID);
            }
        }
    });
}
