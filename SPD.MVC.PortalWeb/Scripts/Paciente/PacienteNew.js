$(function () {

    var urlList = '/Usuario/List';
    var msg = "Deseja realmente adicionar o paciente?";

    $('form').not('#Invalidar').submit(function () {

        var img = $("#idImagem").attr('src');

        $("#srcImage").val(img);

        //if ($(this).valid()) {
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
        //}
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