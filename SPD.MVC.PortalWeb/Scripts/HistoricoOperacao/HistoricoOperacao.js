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
    document.getElementsByClassName("dataTables_info")[0].setAttribute("class", "negrito");

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
        "order": [[4, 'DESC']],
        "ajax":
        {
            "url": "/HistoricoOperacao/Paginacao",
            "type": "POST",
            "dataType": "JSON",
            "data": {
                'Descricao_Filtro': $('#Descricao_Filtro').val(),
                'DataDe': $('#DataDe').val(),
                'DataAte': $('#DataAte').val(),
                'TipoOperacao_Filtro': $('#TipoOperacao_Filtro').val(),
                'Usuario_Filtro': $('#Usuario_Filtro').val(),
                'Funcionalidade_Filtro': $('#Funcionalidade_Filtro').val()
            }
        },
        "displayLength": 100,
        //"createdRow": function (row, data, rowIndex) {
        //    $.each($('td', row), function (colIndex) {
        //        // $(this).attr("title", "Duplo clique para editar");
        //    });
        //},
        "columns": [
            { "data": "Descricao", "orderable": true, "autoWidth": true },
            { "data": "Nome_Tipo_Operacao", "orderable": true, "autoWidth": true },
            { "data": "Nome_Usuario", "orderable": true, "autoWidth": true },
            { "data": "Nome_Funcionalidade", "orderable": true, "autoWidth": true },
            { "data": "DataOperacao", "orderable": true, "autoWidth": true },
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

    // DoubleClick outside the grouping
    //$('#idGrid tbody').on('dblclick', 'tr.odd, tr.even', function () {
    //    Render('MensagensPendentes', 'Edit', table.row(this).data().ID_Erro_Mensagem);
    //});
}