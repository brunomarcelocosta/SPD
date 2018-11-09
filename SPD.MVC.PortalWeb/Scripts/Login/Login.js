$(function () {
    $("#Login").val('');
    $("#Password").val('');

    $('form').submit(function () {
        if (!$(this).valid()) {
            swal("", "Login e Senha são obrigatórios.", "info");
            return false;
        } else {
            return true;
        }
    });

    $('input[name="Redefine"]').click(function () {
        var login = $('input[name="Login"]').val();
        login = login.trim();

        if (login == "") {
            swal({
                title: "O campo Login é obrigatório para redefinir a senha!",
                icon: "warning"
            });
        }
        else {
            $.ajax({
                url: '/Login/RedefinePassword', 
                type: "POST",
                data: { login: login },
                dataType: "json",
                success: function (data) {
                    if (data.result > 0) {
                        if (data.result == 1) {
                            swal({
                                title: "Senha redefinida com sucesso!",
                                icon: "success"
                            }).then(resut => {
                                window.location.href = '/Login/AutenticarLogin'; 
                            });
                        }
                        else if (data.result == 2) {
                            swal({
                                title: "Senha redefinida com sucesso, porém houve falha no envio por e-mail!",
                                icon: "warning"
                            }).then(resut => {
                                window.location.href = '/Login/AutenticarLogin'; 
                            });
                        }
                    }
                    else {
                        swal({
                            title: "Login inexistente!",
                            icon: "error"
                        });
                    }
                },
                error: function (error) {
                    console.log(error);
                    swal({
                        title: "Problemas com a redefinição de senha!",
                        icon: "error"
                    });
                }
            });
        }
    });

});