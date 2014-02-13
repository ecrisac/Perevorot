var Perevorot = Perevorot || {};
var Login = Perevorot.Login || {};


var Login = (function () {
    var _public = {};
    $(function () {
        init();
    });
    
    var submitForm = function (callback) {
        var actionUrl = $("#loginForm").attr('action');
        $.post(actionUrl,$("#loginForm").serialize(),
               function (data) { callback(data);});
    };
    
    var showUserDataValidationResult = function (data) {
        if (data.Result == "Success") {
            $('#result').empty();
            window.location.href = "/Home/Index";
        } else {
            $('#result').show();
            $('#result').html(data.Message);
        }

    };

    function init() {

        $('#result').hide();
      
        $('#login_submit').first('button').click(function () {
            submitForm(showUserDataValidationResult);
        });
        $('body').keydown(function (event) {
            if (event.keyCode == 13) {
                $('#login_submit').first('button').trigger('click');
            }
        });

        $(document).ajaxStart(function () {
            $("#loading").show();
        }).ajaxStop(function () {
            $("#loading").hide();
        });
    }

    return _public;
})();