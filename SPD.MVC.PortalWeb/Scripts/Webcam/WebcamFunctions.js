
function preview_snapshot() {
    // freeze camera so user can preview pic
    Webcam.freeze();

    // swap button sets
    document.getElementById('pre_take_buttons').style.display = 'none';
    document.getElementById('post_take_buttons').style.display = '';
}

function cancel_preview() {
    // cancel preview freeze and return to live camera feed
    Webcam.unfreeze();

    // swap buttons back
    document.getElementById('pre_take_buttons').style.display = '';
    document.getElementById('post_take_buttons').style.display = 'none';
}

function save_photo() {
    // actually snap photo (from preview freeze) and display it
    Webcam.snap(function (data_uri) {
        // display results in page
        document.getElementById('results').innerHTML =
            '<img id="idImagem" src="' + data_uri + '" style="width: 230px;height: 230px;"/>';

        // swap buttons back
        document.getElementById('pre_take_buttons').style.display = 'none';
        document.getElementById('post_take_buttons').style.display = 'none';
        document.getElementById('my_camera').style.display = 'none';

        document.getElementById('idButtonCancel').style.display = '';

        //Webcam.upload (data_uri,  
        //    '/ Camera / Capture',
        //    function (código, texto) {  
        //               alerta ( 'Foto Capturada' );  
        //           });  

        //Webcam.save("@Url.Content("~/Paciente/Capture")/");

    });
}

function cancel_saved() {

    Webcam.unfreeze();

    document.getElementById('pre_take_buttons').style.display = '';
    document.getElementById('my_camera').style.display = '';


    document.getElementById('post_take_buttons').style.display = 'none';
    document.getElementById('idButtonCancel').style.display = 'none';
    document.getElementById('results').innerHTML = '';

}