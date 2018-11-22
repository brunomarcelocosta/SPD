$(function () {
    $('form').not('#Invalidar').submit(function () {

        if ($(this).valid()) {
            $.ajax({
                url: '/Usuario/Update',
                type: 'POST',
                data: $(this).serialize(),
                success: function (result) {
                    var url = '/Usuario/List';

                    if (!result.Success) {
                        swal("", result.Response, "error");
                        return false;
                    }
                    else {
                        swal("", "Usuário atualizado com sucesso.", "success")
                            .then(() => {

                                if (result.AlteracaoFuncionalidade === true) {
                                    //caso tenha tido alguma alteração no perfil de usuário, o sistema notifica e o desconecta em 3 minutos.
                                    notificationHub.server.notifyUser(result.IdUsuario, 'Você será desconectado em {0} minutos por alteração no perfil de acesso. Por favor, salve seus trabalhos correntes.', NotificationType.DANGER, false, "{ $.ajax({ url: '" + moduloAdministracaoURL + "/Usuario/Desconecta', type: 'GET', data: { usuarioID: " + result.IdUsuario + " },  dataType: 'jsonp' }); }", true, 180);
                                }
                                //redireciona para a tela de listagem
                                window.location = '/Usuario/List';
                            });
                    }
                }
            });
        } else {
            swal("", "Os campos Nome, E-mail, Login e Status são obrigatórios.", "info");
        }
        return false;
    });
});

function resetarSenha(login) {
    var msg = "Deseja realmente resetar a senha deste usuário?";
    swal({
        title: "Redefinição de senha.",
        text: msg,
        icon: "warning",
        buttons: ["Não", "Sim"],
        dangerMode: false,
    })
        .then(
            (willDelete) => {
                if (!willDelete) { return; }

                $.ajax({
                    url: '/Usuario/RedefinirSenha',
                    type: 'POST',
                    data: { sLogin: login },
                    success: function (result) {
                        var url = '/Usuario/List';

                        if (!result.Success) {
                            swal("", result.Response, "error");
                            return false;
                        }
                        else {
                            swal("", "Senha redefinida com sucesso.", "success")
                                .then(() => {
                                    window.location = url;
                                });
                        }
                    }
                });
            });
}

function desBloquearUsuario() {

    var msg = "Deseja realmente desbloquear este usuário?";
    swal({
        title: "Desbloqueio de Usuário.",
        text: msg,
        icon: "warning",
        buttons: ["Não", "Sim"],
        dangerMode: false,
    })
        .then(
            (willDelete) => {
                if (!willDelete) { return; }

                $.ajax({
                    url: '/Usuario/DesbloquearUsuario',
                    type: 'POST',
                    data: $('form').not('#Invalidar').serializeArray(),
                    success: function (result) {
                        var url = '/Usuario/List';

                        if (!result.Success) {
                            swal("", result.Response, "error");
                            return false;
                        }
                        else {
                            swal("", "Usuário desbloqueado com sucesso.", "success")
                                .then(() => {
                                    window.location = url;
                                });
                        }
                    }
                });
            });
}