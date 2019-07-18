$(function () {

    var url = '/Agenda/List';
    var msg = "Deseja realmente agendar este horário?";

    $('form').not('#Invalidar').submit(function () {

        if ($(this).valid()) {
            swal({
                title: "Confirmação",
                text: msg,
                icon: "warning",
                buttons: ["Não", "Sim"],
                dangerMode: false,
            }).then((willDelete) => {
                if (!willDelete) {
                    return false;
                }
                $.ajax({
                    url: '/Agenda/Add',
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
                            swal("", "Horário agendado com sucesso.", "success")
                                .then(() => {
                                    window.location = url;
                                });
                        }
                    }
                });

            });
        }
        return false;

    });


});
