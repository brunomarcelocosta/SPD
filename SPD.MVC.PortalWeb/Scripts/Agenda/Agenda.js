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
        //    "info": false,
        "sPaginationType": "full_numbers_no_ellipses",
        "serverSide": true,
        "searching": false,
        "processing": true,
        "stateSave": false,
        "ordering": false,
        //"order": [[1, 'ASC']],
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
                "type": "num",
                "visible": false,
                "targets": groupColumn
            },
            {
                "className": "text-center",
                "targets": [1, 2, 3, 4, 5]
            }
        ],

        "displayLength": 100,
        "drawCallback": function (settings) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr><td colspan="11" style="background-color: #dddddd !important;"><b>' + group + '</b></td></tr>'
                    );

                    last = group;
                }
            });
        },
        "createdRow": function (row, data, rowIndex) {
            $.each($('td', row), function (colIndex) {
                $(this).attr("title", "Duplo clique para editar");
            });
        },
        "columns": [
            { "data": "Hora", "orderable": true, "autoWidth": true },
            { "data": "Dentista", "orderable": true, "autoWidth": true },
            { "data": "Paciente", "orderable": true, "autoWidth": true },
            {
                "data": null, "render": function (data, type, row) {
                    return '<button type="button" id="' + row.ID + '" class="btn btn-danger btn-sm" onclick="Excluir(this)" >Excluir</button>';
                }
            }
        ],
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
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
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });

    //DoubleClick outside the grouping
    $('#idGrid tbody').on('dblclick', 'tr.odd, tr.even', function () {
        Render('Agenda', 'Edit', table.row(this).data().ID);
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
