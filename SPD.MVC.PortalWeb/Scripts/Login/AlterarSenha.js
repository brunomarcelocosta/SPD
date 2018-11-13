$(function () {

    $('form').submit(function () {
        if ($(this).valid()) {
            swal({
                title: "Deseja confirmar a alteração da senha?",
                text: "",
                icon: "warning",
                buttons: ["Não", "Sim"],
                dangerMode: false,
            }).then((isConfirm) => {
                if (!isConfirm) { return false; }

                $.ajax({
                    url: '/AlterarSenha/Confirmar',
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function (result) {
                        confirmarResult(result);
                    }
                });
            });
        } else {
            swal("", "A senha informada não atende aos critérios de complexidade, comprimento ou histórico do sistema.", "info");
        }
        return false;
    });
});

function confirmarResult(result) {
    var url = 'http://' + window.location.hostname + ':59916/';

    if (!result.Success) {
        swal(result.Mensagem, "", "error");
        return;
    }

    swal("Pronto", result.Mensagem, "success")
        .then(() => {
            window.location.href = url;
        });
}

function cancelar(trocaSenhaObrigatoria) {

    if (trocaSenhaObrigatoria == 1) {
        swal("", "A alteração de senha é obrigatória. Não é possível cancelar a operação.", "warning");
        return false;
    }

    window.location.href = 'http://' + window.location.hostname + ':59916/';
}