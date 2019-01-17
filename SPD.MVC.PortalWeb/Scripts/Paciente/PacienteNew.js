$(function () {

    var urlList = '/Usuario/List';
    var msg = "Deseja realmente adicionar o paciente?";

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
                    return window.location = urlList;
                }
                $.ajax({
                    url: '/Paciente/Add',
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function (result) {
                        var url = '/Paciente/List';

                        if (!result.Success) {
                            //Caso não realize a gravação, apresenta mensagem ao usuário.
                            swal(result.Response, "", "error");
                            return false;
                        }
                        else {
                            //apresenta mensagem ao usuário e redireciona para a tela de listagem.
                            swal("Paciente cadastrado com sucesso.", "", "success")
                                .then(() => {
                                    window.location = url;
                                });
                        }
                    }
                });

            });
        }
        else {
            swal("", "Os campos Nome Completo, E-mail, Login e Status são obrigatórios.", "info");
        }

        return false;
    });

    jQuery("#webcam").webcam({
        width: 320,
        height: 240,
        mode: "save",
        swffile: '/Scripts/Webcam/jscam.swf',
        debug: function (type, status) {
        },
        onSave: function (data, ab) {
            $.ajax({
                type: "POST",
                url: '/Paciente/GetCapture',
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (r) {
                    $("#imgCapture").css("visibility", "visible");
                    $("#imgCapture").attr("src", r);
                    $("#srcImage").val(r)
                    $("#webcam").css("visibility", "hidden");
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        },
        onCapture: function () {
            webcam.save('/Paciente/Capture');
        }
    });

});


function Capture() {
    webcam.capture();
}

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
