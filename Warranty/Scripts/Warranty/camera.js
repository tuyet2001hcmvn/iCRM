var timer;
const player = document.getElementById('player');
const canvas = document.getElementById('canvasU');
const context = canvas.getContext('2d');
const captureButton = document.getElementById('capture');

const constraints = {
    audio: false,
    video: { facingMode: "environment" }
};

function startWebcam() {
    // Get access to the camera!
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        // Not adding `{ audio: true }` since we only want video now
        navigator.mediaDevices.getUserMedia(constraints).then(function (stream) {
            player.srcObject = stream;
            player.play();
        });
    }
    //Legacy code below: getUserMedia
    else if (navigator.getUserMedia) { // Standard
        navigator.getUserMedia({ video: true }, function (stream) {
            player.src = stream;
            player.play();
        }, errBack);
    } else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
        navigator.webkitGetUserMedia({ video: true }, function (stream) {
            player.src = window.webkitURL.createObjectURL(stream);
            player.play();
        }, errBack);
    } else if (navigator.mozGetUserMedia) { // Mozilla-prefixed
        navigator.mozGetUserMedia({ video: true }, function (stream) {
            player.srcObject = stream;
            player.play();
        }, errBack);
    }
    //  UploadToCloud();
}

function videoOff() {
    clearTimeout(timer);
    // Stop all video streams.
    player.srcObject.getVideoTracks().forEach(track => track.stop());
    //player.srcObject = null;
}

// (HTML5 based camera only)
// uploads the image to the server
function UploadToCloud() {
    //context.drawImage(video, 0, 0, 460, 360);
    // Draw the video frame to the canvas.
    canvas.width = player.videoWidth;
    canvas.height = player.videoHeight;
    context.drawImage(player, 0, 0);
    var img = canvas.toDataURL('image/jpeg', 0.9).split(',')[1];
    //timer = setTimeout(UploadToCloud, 3000);  // will capture new image to detect barcode after 3000 mili second
    // send AJAX request to the server with image data
    $.ajax({
        url: "/Home/GetSerialNo",
        type: "POST",
        data: "{ 'image': '" + img + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != "NONE") {
                //console.log(data.d);
                //var codeCheck = $("#frmCheckWarranty input[name='SearchSeriNo']").val();
                var codeReg = $("#frmRegWarranty input[name='SerialCheck']").val();
                //codeCheck += data.d + ",";
                codeReg += data.d + ",";
                $("#frmCheckWarranty input[name='SearchSeriNo']").val(data.d);
                $("#frmRegWarranty input[name='SerialCheck']").val(codeReg);
                $("#scanQRCodeModal").modal("hide");
                videoOff();
            } else {
                Swal.fire({
                    icon: "error",
                    text: "QR code không hợp lệ!"
                });
                $("#scanQRCodeModal").modal("hide");
                videoOff();
            }
        },
        async: false
    });
}

const player1 = document.getElementById('scanOD_QRCodeModal').querySelector("#player");
const canvas1 = document.getElementById('scanOD_QRCodeModal').querySelector("#canvasU");
const context1 = canvas1.getContext('2d');

function startWebcam1() {
    // Get access to the camera!
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        // Not adding `{ audio: true }` since we only want video now
        navigator.mediaDevices.getUserMedia(constraints).then(function (stream) {
            player1.srcObject = stream;
            player1.play();
        });
    }
    //Legacy code below: getUserMedia
    else if (navigator.getUserMedia) { // Standard
        navigator.getUserMedia({ video: true }, function (stream) {
            player1.src = stream;
            player1.play();
        }, errBack);
    }
    else if (navigator.webkitGetUserMedia) { // WebKit-prefixed
        navigator.webkitGetUserMedia({ video: true }, function (stream) {
            player1.src = window.webkitURL.createObjectURL(stream);
            player1.play();
        }, errBack);
    }
    else if (navigator.mozGetUserMedia) { // Mozilla-prefixed
        navigator.mozGetUserMedia({ video: true }, function (stream) {
            player1.srcObject = stream;
            player1.play();
        }, errBack);
    }
    //  UploadToCloud();
}

function videoOff1() {
    clearTimeout(timer);
    // Stop all video streams.
    player1.srcObject.getVideoTracks().forEach(track => track.stop());
    //player.srcObject = null;
}

function UploadToCloud_OD() {
    //context.drawImage(video, 0, 0, 460, 360);
    // Draw the video frame to the canvas.
    canvas1.width = player1.videoWidth;
    canvas1.height = player1.videoHeight;
    context1.drawImage(player1, 0, 0);
    var img = canvas1.toDataURL('image/jpeg', 0.9).split(',')[1];
    //timer = setTimeout(UploadToCloud, 3000);  // will capture new image to detect barcode after 3000 mili second
    // send AJAX request to the server with image data
    $.ajax({
        url: "/Home/GetOrderDelivery",
        type: "POST",
        data: "{ 'image': '" + img + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != "NONE") {
                $("#frmRegWarranty input[name='OrderDelivery']").val(data.d);
                $("#scanOD_QRCodeModal").modal("hide");
                videoOff1();
            } else {
                Swal.fire({
                    icon: "error",
                    text: "QR code không hợp lệ!"
                });
                $("#scanOD_QRCodeModal").modal("hide");
                videoOff1();
            }
        },
        async: false
    });
}