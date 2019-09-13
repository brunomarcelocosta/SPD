
$(function () {

    var canvas = document.getElementById("odontogramaAdulto");

    var ctx = canvas.getContext("2d");


    var fundoImg = new Image();

    fundoImg.src = $("#Img_string").val();

    fundoImg.onload = () => {

        ctx.drawImage(fundoImg, 0, 0);

    };

    var signaturePad = new SignaturePad(canvas);
    signaturePad.minWidth = 3;
    signaturePad.maxWidth = 3;
    signaturePad.penColor = "rgb(66, 133, 244)";


});


function SalvarConsulta() {

    if (!ValidaDescricao()) {
        swal("", "A descrição do atendimento é obrigatório.", "error");
        return false;
    }

    var urlList = '/Consulta/List';
    var msg = "Deseja realmente finalizar Atendimento?";

    var canvas = document.querySelector('#odontogramaAdulto');
    var pad = new SignaturePad(canvas);
    var data = pad.toDataURL();

    $("#Img_string").val(data);

    var data = {
        ID_Pre_Consulta: $("#ID_Pre_Consulta").val(),
        ID_Dentista: $("#ID_Dentista").val(),
        Descricao_Procedimento: $("#Descricao_Procedimento").val(),
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
            url: '/Consulta/Add',
            type: 'POST',
            dataType: "JSON",
            data: { 'consultaViewModel': data },
            success: function (result) {

                if (!result.Success) {
                    //Caso não realize a gravação, apresenta mensagem ao usuário.
                    swal("", result.Response, "error");
                    return false;
                }
                else {
                    //apresenta mensagem ao usuário e redireciona para a tela de listagem.
                    swal("", "Atendimento realizado com sucesso.", "success")
                        .then(() => {
                            window.location = urlList;
                        });
                }
            }
        });

    });

    return false;

}

function ValidaDescricao() {
    var desc = $("#Descricao_Procedimento").val();

    if (desc == null || desc == "") {
        return false
    }

    return true;
}


