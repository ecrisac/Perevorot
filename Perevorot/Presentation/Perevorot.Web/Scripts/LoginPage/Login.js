var Perevorot = Perevorot || {};
var Login = Perevorot.Login || {};


var Login = (function () {
    
    $(document).ready(function () {
        init();
    });
    
    var submitForm = function (callback) {
        var actionUrl = $("#loginForm").attr('action');
        $.ajax({
            type: "POST",
            url: actionUrl,
            data: $("#loginForm").serialize(), // serializes the form's elements.
            success: function (data) {
                callback(data); // show response from the php script.
            }
        });
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
        
        $('#login_submit').first('button').click(function() {
            submitForm(showUserDataValidationResult);
        });

    }
})();