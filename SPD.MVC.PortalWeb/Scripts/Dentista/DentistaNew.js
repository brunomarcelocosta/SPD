$(function () {

    var urlList = '/Dentista/List';
    var msg = "Deseja realmente adicionar o dentista?";

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
                url: '/Dentista/Add',
                type: 'POST',
                data: $(this).serialize(),
                success: function (result) {
                    var url = '/Dentista/List';

                    if (!result.Success) {
                        //Caso não realize a gravação, apresenta mensagem ao usuário.
                        swal("", result.Response, "error");
                        return false;
                    }
                    else {
                        //apresenta mensagem ao usuário e redireciona para a tela de listagem.
                        swal("", "Dentista cadastrado com sucesso.", "success")
                            .then(() => {
                                window.location = url;
                            });
                    }
                }
            });

        });

        return false;

    });


});
