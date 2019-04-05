$(function () {

    var urlList = '/PreConsulta/List';
    var msg = "Deseja realmente atualizar a Pré Consulta?";

    $('form').not('#Invalidar').submit(function () {

        //if ($(this).valid()) {
        swal({
            title: "Confirmação",
            text: msg,
            icon: "warning",
            buttons: [ "Não", "Sim"],
            dangerMode: false,
        }).then((willDelete) => {
            if (!willDelete) {
                return window.location = urlList;
            }
            $.ajax({
                url: '/PreConsulta/Update',
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
                        swal("", "Pré Consulta atualizado com sucesso.", "success")
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
