function SendOTP() {
    var phone = $("#SearchPhone").val();
    $(".btn-sendOTP").prop('disabled', true);
    $.ajax({
        url: "/Home/SendOTP",
        type: "POST",
        dataType: "json",
        data: { phoneNumber: phone },
        success: function (response) {
            if (response.success == true) {
                Swal.fire({
                    icon: 'info',
                    text: response.message
                });
                window.bindTimeout(function (t) { setCookie("timer", t); }, 3000);
            }
            else {
                Swal.fire({
                    icon: 'error',
                    text: response.message
                });
                $(".btn-sendOTP").prop('disabled', false);
            }
        }
    });
    //Set remove disabled sau 30s;
    
}
var nativeSetTimeout = window.setTimeout;
window.bindTimeout = function (listener, interval) {
    function setTimeoutCus(code, delay) {
        var elapsed = 0,
            h;
        h = window.setInterval(function () {
            elapsed += interval;
            if (elapsed < delay) {
                listener(delay - elapsed);
            } else {
                window.clearInterval(h);
            }
        }, interval);
        return nativeSetTimeout(code, delay);
    }
    setTimeoutCus(function () {
        
    }, 30000);
    setTimeout._native = nativeSetTimeout;
};
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

