define(["jquery","knockout"], function ($, ko) {

    //define a custom binding for showing status backgrund
    ko.bindingHandlers.infobarLevel = {
        update: function (element, valueAccessor) {
            var value = ko.unwrap(valueAccessor());
            $(element).removeClass( "infobar_info infobar_warning infobar_error" ).addClass( "infobar_" + value );
        }
    };


    var exports = {};

    var Infobar = function () {
        var self = this;

        self.message = ko.observable();
        self.level = ko.observable();
    }

    exports.Instance = new Infobar();

    exports.show = function (message, level) {
        console.log(message);
        exports.Instance.message(message);
        exports.Instance.level(level);
    }

    return exports;
});