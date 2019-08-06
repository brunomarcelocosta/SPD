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
    var groupColumn = 1;

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
            "url": "/PreConsulta/Paginacao",
            "type": "POST",
            "dataType": "JSON",
            "data": {
                'Paciente_string': $('#Paciente_string').val(),
                'Convenio': $('#Convenio').val(),
                'DataDe': $('#DataDe').val()
            }
        },

        "createdRow": function (row, data, rowIndex) {
            $.each($('td', row), function (colIndex) {
                $(this).attr("title", "Duplo clique para editar");
            });
        },

        "columnDefs": [
            {
                "visible": false,
                "targets": [0,1]
            },
            {
                "className": "text-center",
                "targets": [2, 3, 4, 5, 6]
            }
        ],

        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr><td colspan="5" style="background-color: #dddddd !important;"><b>' + group + '</b></td></tr>'
                    );

                    last = group;
                }
            });
        },
        "columns": [
            { "data": "ID", "orderable": false, "autoWidth": true },
            { "data": "Dentista", "orderable": false, "autoWidth": true },
            { "data": "Horario", "orderable": false, "autoWidth": true },
            { "data": "Paciente", "orderable": false, "autoWidth": true },
            { "data": "Autorizado", "orderable": false, "autoWidth": true },
            { "data": "Convenio", "orderable": false, "autoWidth": true },
            {
                "data": null, "render": function (data, type, row) {
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

    $('#idGrid tbody').on('dblclick', 'tr.odd, tr.even', function () {
        Render('PreConsulta', 'Edit', table.row(this).data().ID);
    });
}

function Excluir(obj) {

    var valueID = "";
    valueID = $(obj).attr('ID').toString();

    var urlList = '/PreConsulta/List';
    var msg = "Deseja realmente excluir o Pré Atendimento?";

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
                url: '/PreConsulta/Delete',
                data: { id: valueID },
                dataType: 'JSON',
                success: function (result) {

                    if (!result.Success) {

                        swal("", result.Response, "error");
                        return;

                    }
                    else {
                        swal("", "Pré Atendimento exluída com sucesso.", "success")
                            .then(() => {
                                window.location = urlList;
                            });
                    }
                }
            });
        });
}
