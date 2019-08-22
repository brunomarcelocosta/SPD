var fundoImg = new Image();

$(function () {
    var canvas = document.querySelector('#odontogramaAdulto');
    var pad = new SignaturePad(canvas);
    var num = "1";

    fundoImg.src = "fundo.png";
    pad.drawImage(fundoImg, 0, 0);


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

function fundo() {
    fundoImg.src = "fundo.png";
    ctx.drawImage(fundoImg, 0, 0);
}