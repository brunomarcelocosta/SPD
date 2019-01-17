/// <summary>
/// formatação de máscaras na entrada de dados
/// </summary>
$(document).ready(function () {
    $('.date').mask('00/00/0000');

    var mask = function (val) {
        val = val.split(":");
        return (parseInt(val[0]) > 19) ? "HZ:M0" : "H0:M0";
    }

    pattern = {
        onKeyPress: function (val, e, field, options) {
            field.mask(mask.apply({}, arguments), options);
        },
        translation: {
            'H': { pattern: /[0-2]/, optional: false },
            'Z': { pattern: /[0-3]/, optional: false },
            'M': { pattern: /[0-5]/, optional: false }
        }
    };

    $(".time").mask(mask, pattern);

    $('.date_time').mask('00/00/0000 00:00:00');

    $('.mixed').mask('AAA 000-S0S');

    $('.money').mask('000.000.000.000.000,00', { reverse: true });

    $('.money2').mask("#.##0,00", { reverse: true });

    $('.decimal').mask("000.000.000.000.000,0", { reverse: true });

    $('.decimal2').mask("#0,0", { reverse: true });

    $('.integer').mask("#.##0", { reverse: true });

    $('.integerNumber').mask("###0", { reverse: true });

    $('.clear-if-not-match').mask("00/00/0000", { clearIfNotMatch: true });

    $('.placeholder').mask("00/00/0000", { placeholder: "__/__/____" });

    $('.selectonfocus').mask("00/00/0000", { selectOnFocus: true });

    $('.matricula').mask("SSSSS");

    $('.hora').mask('00:00:00');

    $('.celular').mask('(00)00000-0000');

    $('.rg').mask('00.000.000-0');

    $('.cep').mask('00000-000');

    $('.cpf').mask('000.000.000-00');

});
