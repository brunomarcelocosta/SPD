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
        //    "ordering": true,
        "order": [[1, 'ASC']],
        "ajax":
        {
            "url": "/HistoricoOperacao/Paginacao",
            "type": "POST",
            "dataType": "JSON",
            "data": {
                'Nome': $('#Descricao_Filtro').val(),
                'Email': $('#DataDe').val(),
                'isAtivo': $('#DataAte').val(),
                'isBloqueado': $('#TipoOperacao_Filtro').val()
            }
        },
        "columnDefs": [
            {
                "targets": -1,
                "data": null,
                "defaultContent": "<input type='button' id='btnDelete' class='btn btn-succes' width='25px' value='Excluir' />"
            }
        ],
        "displayLength": 100,
        "createdRow": function (row, data, rowIndex) {
            $.each($('td', row), function (colIndex) {
                $(this).attr("title", "Duplo clique para editar");
            });
        },
        "columns": [
            { "data": "ID", "orderable": true, "autoWidth": true },
            { "data": "Nome", "orderable": true, "autoWidth": true },
            { "data": "Email", "orderable": true, "autoWidth": true },
            { "data": "isAtivo", "orderable": true, "autoWidth": true },
            { "data": "isBloqueado", "orderable": true, "autoWidth": true }
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

    $('#idGrid tbody').on('click', '[id*=btnDetails]', function () {
        var data = table.row($(this).parents('tr')).data();
        var userID = data[0];

        Excluir(userID);
    });

    //DoubleClick outside the grouping
    $('#idGrid tbody').on('dblclick', 'tr.odd, tr.even', function () {
        Render('Usuario', 'Edit', table.row(this).data().ID);
    });
}

function Excluir(userID) {
    var urlList = '/Usuario/List';
    var url = '/Usuario/Delete';
    var msg = "Deseja realmente excluir o usuário?";

    swal({
        title: "Confirmação",
        text: msg,
        icon: "warning",
        buttons: ["Não", "Sim"],
        dangerMode: false,
    })
        .then((willDelete) => {
            if (!willDelete) {
                return window.location = urlList;
            }

            $.ajax({
                type: "POST",
                url: url,
                data: { id = userID },
                dataType: 'JSON',
                traditional: true,
            }).done((result) => {
                if (result == "Não autorizado.") {
                    swal("", result, "error");
                    return;
                }

                if (!result.Success) {

                    swal("", result.Response, "error");
                    return;

                } else {
                    swal("", "Usuário excluído com sucesso.", "success")
                        .then(() => {
                            window.location = urlList;
                        });
                }
            }).fail((error) => {
                console.log(error);
            });
        });
}
