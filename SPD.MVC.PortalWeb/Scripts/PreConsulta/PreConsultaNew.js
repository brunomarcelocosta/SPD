
$(function () {
    var canvas = document.querySelector('#signature');
    var pad = new SignaturePad(canvas);
    var num = "1";

    $('#accept').click(function () {
        var data = pad.toDataURL();
        $('#savetarget').attr('src', data);
        $("#Img_string").val(data);
        $('#img_string_value').val(data);
        $('#accept_string_value').val(num);
        pad.off();
    });

    $("#clear").click(function () {
        pad.clear();
    });

});

$(document).ready(function () {

    var paciente = $("#paciente_string_value").val();
    var convenio = $("#convenio_string_value").val();
    var nome_convenio = $("#nomeConvenio_string_value").val();
    var nr_carterinha = $("#nrCarterinha_string_value").val();
    var nome_resp = $("#nomeResp_string_value").val();
    var cpf_resp = $("#cpfResp_string_value").val();

    if (paciente != "" && paciente != null) {

        GetPaciente(paciente);

        $("#Paciente_string").val(paciente);

        if (convenio == "true") {

            IsConvenio(nome_convenio, nr_carterinha);
        }
        else {
            $("#particular").click();
        }

        ShowItensCriancas(nome_resp, cpf_resp);
    }

});

function NewCanvas() {
    $('#savetarget').attr('src', '');
    $('#img_string_value').val("");
    $('#accept_string_value').val("");
    $("#idimg").hide();
    $("#btnSalvarPreConsulta").hide();
    $("#assinatura").show();
}

function OnChange() {

    CleanVariavel();
    CleanViewModel();
    HideHiddens();

    var _nome = $('#Paciente_string').val();
    GetPaciente(_nome);

}

function GetPaciente(_nome) {

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

                $("#Maior_Idade").val(m_idade);

                ShowHiddens(m_idade, idade);

                var accept = $("#accept_string_value").val();

                if (accept == "1") {
                    IsAccept();
                }
                else {
                    IsCleanAssinatura();
                    ValidaIdade(result.Idade);
                }

            } else {
                HideHiddens();

                swal("", result.Response, "error");
                return false;
            }
        },
        error: function (erro) {
            ShowHiddens(m_idade, idade);

            swal("", erro.Response, "error");
            return false;
        }
    });
}

function HideHiddens() {

    $("#idTpPaciente").prop("hidden", true);
    $("#idIdade").prop("hidden", true);
    $("#idBtnAutorizacao").prop("hidden", true);
    $("#particular").prop("checked", false);
    $("#Conveniado").prop("checked", false);
    $("#idNomeConvenio").prop("hidden", true);
    $("#idNrCarterinha").prop("hidden", true);
    $("#divButtons").prop("hidden", true);
    $("#divTermo").hide();
    $("#btnSalvarPreConsulta").hide();
}

function ShowHiddens(m_idade, idade) {

    $("#idTpPaciente").prop("hidden", false);

    $("#idBtnAutorizacao").prop("hidden", m_idade);
    $("#idNomeResponsavel").prop("hidden", m_idade);
    $("#idCpfResponsavel").prop("hidden", m_idade);


    $("#idIdade").prop("hidden", false);
    $("label[for='IdadeValue']").text(idade);

    $("#divButtons").prop("hidden", false);
}

function IsConvenio(nome_convenio, nr_carterinha) {

    $("#Conveniado").click();
    $("#Convenio").val(nome_convenio);
    $("#Numero_Carterinha").val(nr_carterinha);
}

function IsAccept() {

    var data = $('#img_string_value').val();
    $('#savetarget').attr('src', data);
    $("#Img_string").val(data);
    $("#idimg").show();
    $("#btnSalvarPreConsulta").show();
    $("#assinatura").hide();
}

function IsCleanAssinatura() {

    $("#idimg").hide();
    $("#assinatura").show();
    $("#btnSalvarPreConsulta").hide();
}

function ShowItensCriancas(nome_resp, cpf_resp) {

    $("#Nome_Responsavel").val(nome_resp);
    $("#Cpf_Responsavel").val(cpf_resp);
    $("#idBtnAutorizacao").hide();
    $("#divTermo").show();
}

function AutorizarConsulta() {
    $("#divTermo").show();
    $("#idBtnAutorizacao").hide();

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
    var msg = "Deseja realmente adicionar o Pré Atendimento?";

    var data = {
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
            url: '/PreConsulta/Add',
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
                    swal("", "Pré Atendimento realizado com sucesso.", "success")
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

function CleanVariavel() {
    $("#paciente_string_value").val("");
    $("#convenio_string_value").val("");
    $("#nomeConvenio_string_value").val("");
    $("#nrCarterinha_string_value").val("");
    $("#nomeResp_string_value").val("");
    $("#cpfResp_string_value").val("");
    $("#accept_string_value").val("");
}

function CleanViewModel() {

    $("#Nome_Responsavel").val("");
    $("#Cpf_Responsavel").val("");
    $("#Img_string").val("");
    $("#Convenio").val("");
    $("#Numero_Carterinha").val("");
    $("label[for='IdadeValue']").text("");

}

function ValidaIdade(idade) {

    if (idade >= 18) {
        $("#btnSalvarPreConsulta").show();
    } else {
        $("#btnSalvarPreConsulta").hide();
    }
}