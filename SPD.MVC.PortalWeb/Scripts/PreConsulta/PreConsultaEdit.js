
function SalvarPreConsulta() {
    var urlList = '/PreConsulta/List';
    var msg = "Deseja realmente atualizar o Pré Atendimento?";

    var data = {
        ID: $("#ID").val(),
        ID_Agenda: $("#ID_Agenda").val(),
        Paciente_string: $("#Paciente_string").val(),
        Conveniado: $("#Conveniado").val(),
        particular: $("#particular").val(),
        Idade: $("#Idade").val(),
        Maior_Idade: $("#Maior_Idade").val(),
        Convenio: $("#Convenio").val(),
        Numero_Carterinha: $("#Numero_Carterinha").val(),
        Nome_Responsavel: $("#Nome_Responsavel").val(),
        Cpf_Responsavel: $("#Cpf_Responsavel").val(),
        Img_string: $("#Img_string").val()
    }

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
            url: '/PreConsulta/Update',
            type: 'POST',
            dataType: "JSON",
            data: { 'preConsultaViewModel': data },
            success: function (result) {

                if (!result.Success) {
                    //Caso não realize a gravação, apresenta mensagem ao usuário.
                    swal("", result.Response, "error");
                    return false;
                }
                else {
                    //apresenta mensagem ao usuário e redireciona para a tela de listagem.
                    swal("", "Pré Atendimento atualizado com sucesso.", "success")
                        .then(() => {
                            window.location = urlList;
                        });
                }
            }
        });

    });

    return false;

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
            $("#Convenio").val("");
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