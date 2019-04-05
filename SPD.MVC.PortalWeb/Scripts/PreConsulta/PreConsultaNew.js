
$(document).ready(function () {

    $("#Paciente_string").change(function () {
        var _nome = $("#Paciente_string").val();

        $.ajax({
            url: "/PreConsulta/GetPaciente",
            data: { nome: _nome },
            type: "post",
            success: function (result) {
                if (result.Success) {
                    var m_idade = true;
                    var idade = result.Idade + " anos";

                    if (!result.MaiorIdade) {
                        m_idade = false
                    }

                    $("#idTpPaciente").prop("hidden", false);

                    $("#idBtnAutorizacao").prop("hidden", m_idade);
                    $("#idNomeResponsavel").prop("hidden", m_idade);
                    $("#idCpfResponsavel").prop("hidden", m_idade);


                    $("#idIdade").prop("hidden", false);
                    $("label[for='IdadeValue']").text(idade);


                    $("#divButtons").prop("hidden", false);

                } else {
                    $("#idTpPaciente").prop("hidden", true);
                    $("#idIdade").prop("hidden", true);
                    $("#idBtnAutorizacao").prop("hidden", true);
                    $("#idNomeResponsavel").prop("hidden", true);
                    $("#idCpfResponsavel").prop("hidden", true);
                    $("#divButtons").prop("hidden", true);

                    swal("", result.Response, "error");
                    return false;
                }
            },
            error: function (erro) {
                swal("", erro.Response, "error");
                return false;
            }
        });

    });


});

function AutorizarConsulta() {
    $("#divTermo").prop("hidden", false);
    $("#idBtnAutorizacao").prop("hidden", true);

}

function ValidaCheckBox(check, tipo) {

    if (check.checked) {
        if (tipo == 1) {
            $("#particular").prop("checked", true);
            $("#Conveniado").prop("checked", false);
            $("#idNomeConvenio").prop("hidden", true);
            $("#idNrCarterinha").prop("hidden", true);
        } else {
            $("#particular").prop("checked", false);
            $("#Conveniado").prop("checked", true);
            $("#idNomeConvenio").prop("hidden", false);
            $("#idNrCarterinha").prop("hidden", false);
        }
    } else {

        if (tipo == 1) {
            $("#particular").prop("checked", false);
            $("#idNomeConvenio").prop("hidden", true);
            $("#idNrCarterinha").prop("hidden", true);
        } else {
            $("#Conveniado").prop("checked", false);
            $("#idNomeConvenio").prop("hidden", true);
            $("#idNrCarterinha").prop("hidden", true);
        }

    }
}

function SalvarPreConsulta() {
    var urlList = '/PreConsulta/List';
    var msg = "Deseja realmente adicionar a Pré Consulta?";

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
                    swal("", "Pré Consulta realizada com sucesso.", "success")
                        .then(() => {
                            window.location = urlList;
                        });
                }
            }
        });

    });

    return false;

}

function Cancel() {
    window.location = '/PreConsulta/List';
}