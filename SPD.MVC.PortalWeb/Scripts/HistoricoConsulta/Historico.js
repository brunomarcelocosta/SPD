
function aplicarFiltro() {

    var urlList = '/HistoricoConsulta/List';

    var dataDe = $('#DataDe').val();
    var dataAte = $('#DataAte').val();
    var dentista = $("#Dentista").val();
    var paciente = $("#Paciente").val();

    window.location.href = urlList + '?page=&dt_init=' + dataDe + '&dt_end=' + dataAte + '&dentista=' + dentista + '&paciente=' + paciente;
}

function limpar_Filtros() {

    $("#Dentista").val("");
    $("#Paciente").val("");
    $("#DataDe").val("");
    $("#DataAte").val("");
}