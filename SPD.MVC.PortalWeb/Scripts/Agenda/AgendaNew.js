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

    if (data != null && data != "" && data != "yyyy-MM-dd") {

        $("#Dentista_string").prop("disabled", false);
        $("#Nome_Paciente").prop("disabled", true);
        $("#Hora_Inicio").prop("disabled", true);
        $("#Tempo_Consulta").prop("disabled", true);
    }
    else {

        $("#Dentista_string").prop("disabled", true);
        $("#Nome_Paciente").prop("disabled", true);
        $("#Hora_Inicio").prop("disabled", true);
        $("#Tempo_Consulta").prop("disabled", true);
    }
}

function HabilitaHorario() {
    var data = $("#DataDe").val();
    var dentista = $("#Dentista_string").val();
    var uri = "../../Agenda/ListHoraDisponivel";

    if (dentista != "" && dentista != null) {

        $.getJSON(uri, { data: data, dentista: dentista })
            .done(function (list) {


                $("#Hora_Inicio").prop("disabled", false);
                $("#Tempo_Consulta").prop("disabled", false);
                $("#Nome_Paciente").prop("disabled", false);

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
    }
}