var textbox;
var selectValue;

$(function () {
    textbox = $("#Nome_Paciente");
    selectValue = $('ul#selectedValue');

    textbox.on("input", function () {
        ValidaAC();
        getAutoComplete(textbox.val());
    });
});

function ValidaAC() {
    var id = $('#id_ac').val();

    if (id == 1) {
        $('ul#selectedValue').show();
    }
}

function getAutoComplete(countryName) {
    $('#id_ac').val("");

    var uri = "../../Agenda/BuscaPaciente";
    $.getJSON(uri, { prefix: countryName })
        .done(function (data) {
            selectValue.html("");
            var count = 0;
            $.each(data, function (key, item) {
                var li = $('<li/>').addClass('ui-menu-item').attr('role', 'menuitem')
                    .html("<a href='#' onclick=\"setText('" + item + "') \">" + item + "</a>")
                    .appendTo(selectValue);

                count++;
            });
        });
}

function setText(text) {
    textbox.val(text);
    $('#id_ac').val(1);
    $('ul#selectedValue').hide();
}

$(function () {

    var url = '/Agenda/List';
    var msg = "Deseja realmente agendar este horário?";

    $('form').not('#Invalidar').submit(function () {

        var paciente = $("#Nome_Paciente").val();
        var horario = $("#Hora_Inicio").val();
        var duracao = $("#Tempo_Consulta").val();

        if (!ValidaCampos(paciente, horario, duracao)) {
            swal("", "Todos os campos devem estar preenchidos.", "error");
            return;
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

        return false;

    });
});

function HabilitaCampos() {

    var data = $("#DataDe").val();

    if (ValidaData(data)) {

        $("#Dentista_string").prop("disabled", false);

        DesabledCampos();

    } else {

        $("#Dentista_string").prop("disabled", true);

        DesabledCampos();

        swal("", "A data selecionada é inválida.", "error");
        return;
    }
}

function HabilitaHorario() {
    var data = $("#DataDe").val();
    var dentista = $("#Dentista_string").val();
    var uri = "../../Agenda/ListHoraDisponivel";

    if (dentista != "" && dentista != null) {

        $.getJSON(uri, { data: data, dentista: dentista })
            .done(function (list) {

                EnabledCampos();

                var select = $("#Hora_Inicio");

                select.empty();

                select.append($('<option/>', {
                    value: "",
                    text: "Selecione um horário"
                }));

                $.each(list, function (index, item) {
                    select.append($('<option/>', {
                        value: item,
                        text: item
                    }));
                });
            });
    } else {

        DesabledCampos();

        LimpaCampos();

        return;
    }
}

function ValidaData(date_) {

    var data = new Date(date_);
    var today = new Date();

    var dia = data.getDate() + 1;
    var mes = data.getMonth() + 1;
    var ano = data.getFullYear();
    var _data = dia + '/' + mes + '/' + ano;

    var dia_ = today.getDate();
    var mes_ = today.getMonth() + 1;
    var ano_ = today.getFullYear();
    var _today = dia_ + '/' + mes_ + '/' + ano_;

    var result = _data >= _today ? true : false;

    return result;
}

function ValidaCampos(paciente, horario, duracao, celular) {

    if ((paciente == null || paciente == "") ||
        (horario == null || horario == "") ||
        (duracao == null || duracao == "") ||
        (celular == null || celular == "")
    ) {
        return false;
    }

    return true;
}

function DesabledCampos() {

    $("#Nome_Paciente").prop("disabled", true);
    $("#Hora_Inicio").prop("disabled", true);
    $("#Tempo_Consulta").prop("disabled", true);
    $("#btnSalvar").prop("disabled", true);
    $("#Celular").prop("disabled", true);
}

function EnabledCampos() {

    $("#Nome_Paciente").prop("disabled", true);
    $("#Hora_Inicio").prop("disabled", true);
    $("#Tempo_Consulta").prop("disabled", true);
    $("#btnSalvar").prop("disabled", true);
    $("#Celular").prop("disabled", true);
}

function LimpaCampos() {

    $("#Nome_Paciente").val("");
    $("#Hora_Inicio").val("");
    $("#Tempo_Consulta").val("");
    $("#Celular").val("");
}