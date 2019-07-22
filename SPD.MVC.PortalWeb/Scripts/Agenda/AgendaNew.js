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
        }
        return false;

    });


});
