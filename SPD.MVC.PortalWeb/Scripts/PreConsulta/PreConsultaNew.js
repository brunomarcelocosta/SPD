//Pré Consulta
$(function () {

    var urlList = '/PreConsulta/List';
    var msg = "Deseja realmente adicionar a Pré Consulta?";

    $('form').not('#Invalidar').submit(function () {

        //if ($(this).valid()) {
        swal({
            title: "Confirmação",
            text: msg,
            icon: "warning",
            buttons: ["Não", "Sim"],
            dangerMode: false,
        }).then((willDelete) => {
            if (!willDelete) {
                return window.location = urlList;
            }
            $.ajax({
                url: '/PreConsulta/Add',
                type: 'POST',
                data: $(this).serialize(),
                success: function (result) {

                    if (!result.Success) {
                        //Caso não realize a gravação, apresenta mensagem ao usuário.
                        swal("", result.Response, "error");
                        return false;
                    }
                    else {
                        //apresenta mensagem ao usuário e redireciona para a tela de listagem.
                        swal("", "Pré Consulta cadastrado com sucesso.", "success")
                            .then(() => {
                                window.location = urlList;
                            });
                    }
                }
            });

        });

        return false;

    });


});

$(document).ready(function () {

    $("#Paciente_string").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/PreConsulta/AutoCompletePaciente",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                //contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        item.label = item.Nome;
                        item.val = item.ID;

                        //var divsToHide = document.getElementsByClassName("ui-helper-hidden-accessible");

                        //for (var i = 0; i < divsToHide.length; i++) {
                        //    divsToHide[i].style.visibility = "hidden";
                        //}

                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#Paciente_string").val(i.item.val);
            // carrega demais informações aqui.
        },
        minLength: 1
    });
});
