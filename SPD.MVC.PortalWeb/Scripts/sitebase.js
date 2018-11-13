var form = document.forms[0];

// For String.format
if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

/// <summary>
/// função para abrir modal genérico
/// </summary>
function questionar(action, controller) {
    //lstIDs = $("#selected_itens").val();    
    $("#modalGenerica").modal('show');
    Loading($("#ContentmodalGenerica"));
    RenderPartial(controller, action, "get").done(function (success) {
        $("#ContentmodalGenerica").html(success);
    });
}

/// <summary>
/// função para validar dados de relatório antes de abrir modal de exportação ou impressão de pdf
/// </summary>
function ValidaDados(controller, action, value) {

    var data = $("form").serialize();

    RenderPartial(controller, action, data, "post", "json").done(function (result) {       
    });
}

/// <summary>
/// função para renderizar partialView genérica
/// </summary>
function RenderPartial(controller, action, data, tipo, dataTipo) {
    var url = ROOT.concat("/", controller, "/", action);
    return $.ajax({
        type: tipo === undefined ? "post" : tipo,
        dataType: dataTipo == undefined ? "html" : dataTipo,
        url: url,
        data: data,
        error: function (status) {
            var errorData = $.parseJSON(status.responseText);
            ShowWarningMessage(errorData[0])
        }
    });
}

/// <summary>
/// função para chamada do método de geração de relatório em excel ou pdf
/// </summary>
function Processar() {

    if ($('#csv').is(':checked')) {
        var idValor = $('#idValor').val();

        if (idValor == "") {
            $('#mensagem').show();
        } else {
            $('#mensagem').hide();
            CloseModalGenerica();
            $('#IdDelimitador').val(idValor);
            $('#btnbaixarCSV').click();
        }
    } else {
        CloseModalGenerica();
        $('#btnbaixarExcel').click();
    }
}

/// <summary>
/// função para fechar modal genérico
/// </summary>
function CloseModalGenerica() {
    $("#modalGenerica").modal('hide');
}

/// <summary>
/// função genérica para execução de método de exclusão
/// </summary>
function excluir(action, controller, dado, tipo) {
    var urlList = '/' + controller + '/List';
    var url = '/' + controller + '/' + action;
    var msg = "Deseja realmente excluir o registro?";

    swal({
        title: "Confirmação",
        text: msg,
        icon: "warning",
        buttons: ["Não", "Sim"],
        dangerMode: false,
    })
        .then((willDelete) => {
            if (!willDelete) {
                return window.location = urlList;
            }

            $.ajax({
                type: "POST",
                url: url,
                data: dado,
                dataType: 'JSON',
                traditional: true,
            }).done((result) => {
                if (result == "Não autorizado.") {
                    swal("", result, "error");
                    return;
                }

                excluirResult(result, dado.Nome, tipo);
            }).fail((error) => {
                console.log(error);
            });
        });
}

/// <summary>
/// função genérica para execução de método de exclusão: renderização do resultado
/// </summary>
function excluirResult(result, nomeItem, tipo) {
    var url = '/' + tipo + '/List';

    if (!result.Success) {
        console.log("Não foi possível excluir o registro: " + result.Response);
        swal("", result.Response, "error");

        return;
    } else {
        swal("Pronto ", nomeItem + " excluído.", "success")
            .then(() => {
                window.location = url;
            });
    }
}

/// <summary>
/// função para limpar div loading
/// </summary>
function Loading(div) {
    div.html('');
}

/// <summary>
/// html-encodes messages for display in the page
/// </summary>
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();

    return encodedValue;
}

$(document).ready(function () {
    $('form input, select, textarea').each(function () {
        // negrito nos labels de campos obrigatórios de acordo com configuração Required da ViewModel
        var req = $(this).attr('data-val-required');
        if (undefined != req) {
            var label = $('label[for="' + $(this).attr('id') + '"]');
            label.attr('style', 'font-weight: bold');
            // Caso queira colocar asterisco ao lado do label, remover comentários abaixo
            //var text = label.text();
            //if (text.length > 0) {
            //    label.append('<span style="color: red"> *</span>');
            //}
        }

        // setar maxlength dos campos de acordo com configuração MaxLength da ViewModel
        var max = $(this).attr('data-val-maxlength-max');
        if (undefined != max) {
            var $this = $(this);
            var data = $this.data();
            $this.attr("maxlength", max);
        }
    });
});

/// <summary>
/// For notification
/// </summary>
var NotificationType = {
    SUCCESS: "success",
    INFORMATION: "information",
    WARNING: "warning",
    DANGER: "danger"
}

/// <summary>
/// For notification
/// </summary>
function notify(notification, type, useAnimation, id) {
    // For Debug
    console.log(id);

    var notificationFound = (typeof id !== 'undefined') ? ($('#notifications #' + id).length) : false;
    var container = (notificationFound) ? $('#notifications #' + id) : $('#notifications');

    if (notificationFound) {
        switch (type) {
            case NotificationType.SUCCESS:
                container.html(' <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification));
                break;

            case NotificationType.INFORMATION:
                container.html(' <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification));
                break;

            case NotificationType.WARNING:
                container.html(' <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification));
                break;

            case NotificationType.DANGER:
                container.html(' <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification));
                break;

            default:
                console.log('Invalid notification type: ' + type);
                break;
        }
    } else {
        switch (type) {
            case NotificationType.SUCCESS:
                container.append('<div id="' + id + '" class="alert alert-success fade in"> <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification) + '</div>');
                break;

            case NotificationType.INFORMATION:
                container.append('<div id="' + id + '" class="alert alert-info fade in"> <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification) + '</div>');
                break;

            case NotificationType.WARNING:
                container.append('<div id="' + id + '" class="alert alert-warning fade in"> <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification) + '</div>');
                break;

            case NotificationType.DANGER:
                container.append('<div id="' + id + '" class="alert alert-danger fade in"> <button type="button" class="close" data-dismiss="alert" aria-hidden="true"> &times; </button >' + htmlEncode(notification) + '</div>');
                break;

            default:
                console.log('Invalid notification type: ' + type);
                break;
        }
    }

    if (useAnimation) {
        //container.slideDown(500).delay(5000).slideUp(500);
        container.show().fadeOut(5000);
    } else {
        container.show();
    }
}

// For modules
var notificationHub = null;
var sistemaHub = null;
var portalweb = 'http://' + window.location.hostname + ':59916';

/// <summary>
/// função para setup de notificação hub
/// </summary>
function setupNotificationHub() {
    // Configure the connection
    $.connection.hub.url = portalweb + '/portalweb';
    //$.connection.hub.logging = true;

    // Create the proxy for the hub
    notificationHub = $.connection.notificationHub;

    // Setup the client
    notificationHub.client.notifyAll = function (notification, type, useAnimation, id) {
        // For Debug
        console.log('notificationHub.client.notifyAll');

        // Add the message to the page.
        notify(notification, type, useAnimation, id);
    };


    // Setup the client
    notificationHub.client.callBack = function (mustReload) {

        window.location.href = moduloAutenticacaoTelaPrincipalURL + "/Login/AutenticarLogin";
    };

}

/// <summary>
/// função para setup do sistema hub
/// </summary>
function setupSistemaHub() {
    // Configure the connection
    $.connection.hub.url = portalweb + '/portalweb';
    //$.connection.hub.logging = true;

    // Create the proxy for the hub
    sistemaHub = $.connection.sistemaHub;

    sistemaHub.client.desativar = function (url) {
        // For Debug
        console.log('sistemaHub.client.desativar');

        // Go to the new url
        window.location.href = url;
    };
}

/// <summary>
/// função para confirmação de logoff do sistema
/// </summary>
function setupLogoff() {
    $('#logoffSistema').click(function () {
        swal({
            title: "Deseja realmente sair?",
            icon: "warning",
            buttons: ["Não", "Sim"],
            dangerMode: false
        }).then((Ok) => {
            if (Ok) {
                swal({
                    title: "Logoff efetuado com sucesso",
                    text: "A página será recarregada.",
                    icon: "success",
                    button: true,
                    timer: 2000
                }).then((Login) => {
                    frmLogoffSistema.submit();
                });
            }
        });
    });
}

/// <summary>
/// For initialization
/// </summary>
$(document).ready(function () {
    // For Debug
    //console.log("Ready from sitebase");

    // Setups
    setupNotificationHub();
    setupSistemaHub();
    setupLogoff();
});


$(document).ready(function () {

    var id = $('#ID').val();
    console.log(id);

    if (id == 0) {
        $('#ID').val('');
        $('#isAtivo').val('');
        $('#Nacionalidade').val('');
    }
});

function callback(usuarioID) {
    console.log("Ajax para desconectar o usuário");
}