$(function () {

    var urlList = '/Paciente/List';
    var msg = "Deseja realmente adicionar o paciente?";

    $('form').not('#Invalidar').submit(function () {

        var img = $("#idImagem").attr('src');

        $("#srcImage").val(img);

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
                    url: '/Paciente/Add',
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function (result) {
                        var url = '/Paciente/List';

                        if (!result.Success) {
                            //Caso não realize a gravação, apresenta mensagem ao usuário.
                            swal("", result.Response, "error");
                            return false;
                        }
                        else {
                            //apresenta mensagem ao usuário e redireciona para a tela de listagem.
                            swal("", "Paciente cadastrado com sucesso.", "success")
                                .then(() => {
                                    window.location = url;
                                });
                        }
                    }
                });

            });
        }
        //else {
        //    swal("", "Os campos Nome Completo, E-mail, Login e Status são obrigatórios.", "info");
        //}

        return false;
    });

    Webcam.set({
        width: 230,
        height: 230,
        image_format: 'jpeg',
        jpeg_quality: 90
    });
    Webcam.attach('#my_camera');

});

function ValidaCheckBox(check, tipo) {

    if (check.checked) {
        if (tipo == 1) {
            $("#particular").prop("checked", true);
            $("#conveniado").prop("checked", false);
        } else {
            $("#particular").prop("checked", false);
            $("#conveniado").prop("checked", true);
        }
    }
}

function ws_GetCEP() {
    $('.rua').val('');
    $('.bairro').val('');
    $('.cidade').val('');
    $('.uf').val('');
    $('.pais').val('');

    var cep = $('#Cep').val();


    if (cep != "") {
        $.ajax({
            url: '/Paciente/WS_GetCEP',
            type: 'POST',
            data: { cep: cep },
            dataType: 'JSON',
            success: function (result) {
                if (result.erro) {
                    swal(result.erroMsg, "", "error");
                } else {
                    $('.rua').val(result.logradouro);
                    $('.bairro').val(result.bairro);
                    $('.cidade').val(result.localidade);
                    $('.uf').val(result.uf);
                    $('.pais').val(result.pais);

                }
            },
            error: function (erro) {
                swal("", erro.Response, "error");
                return false;
            }
        });
    }
}

function BuscaHorarioPaciente() {

    var dia = $("#Agenda_Dia").val();

    var uri = "../../Paciente/BuscaHorarioPaciente";

    if (dia != "" && dia != null) {

        $.getJSON(uri, { dia: dia })
            .done(function (list) {

                var select = $("#Horario");

                select.empty();

                select.append($('<option/>', {
                    value: "",
                    text: "Selecione um horário"
                }));

                var count = list.length;
                if (count > 0) {
                    $("#Horario").prop("disabled", false);
                }

                $.each(list, function (index, item) {
                    select.append($('<option/>', {
                        value: item,
                        text: item
                    }));
                });


            });
    }

    return false;

}

function BuscaPacienteAgenda() {

    var dia = $("#Agenda_Dia").val();
    var hora = $("#Horario").val();

    var uri = "../../Paciente/c";

    if (hora != "" && hora != null) {

        $.getJSON(uri, { hora: hora, dia: dia })
            .done(function (item) {

                if (item != "" && item != null) {

                    $("#Nome_Paciente").val(item);
                }
            });
    }

    return false;
}