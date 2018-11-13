/// <summary>
/// For initialization
/// </summary>
$(document).ready(function () {
    // Start the hub
    $.connection.hub
        .start({
            jsonp: true
        })
        .done(function () {
            console.log('Now connected from SPD.MVC.PortalWeb, connection ID = ' + $.connection.hub.id);

            notificationHub.server.addUser(getAuthentication());

            // For Debug
            //sistemaHub.server.isAlive('SignalR is running...');
        })
        .fail(function (error) {
            // For Debug
            console.log('Could not connect: ' + error);
        });    

    // Exibe pager com uma única página
    var gridPager = $(".grid-footer");
    if (gridPager.children().length == 0) {
        gridPager.append('<ul class="pagination"> <li class="active"><span>1</span></li></ul>');
    }
});
