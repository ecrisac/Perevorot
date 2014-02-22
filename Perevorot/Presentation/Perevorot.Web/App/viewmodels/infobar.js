define(["jquery","knockout"], function ($, ko) {

    //define a custom binding for showing status backgrund
    ko.bindingHandlers.infobarLevel = {
        update: function (element, valueAccessor) {
            var value = ko.unwrap(valueAccessor());
            $(element).removeClass( "infobar_info infobar_warning infobar_error" ).addClass( "infobar_" + value );
        }
    };


    var exports = {};

    var Infobar = function() {
        var self = this;

        self.visible = ko.observable(false);
        self.message = ko.observable();
        self.level = ko.observable();
        self.levelText = ko.observable();
        self.moreInfoShown = ko.observable(false);

        self.hide = function() {
            self.visible(false);
        };

        self.showMoreInfo = function() {
            self.moreInfoShown(true);
        };

        self.hideMoreInfo = function() {
            self.moreInfoShown(false);
        };

        self.show = function (level, message) {
            exports.instance.message(message);
            exports.instance.level(level);
            exports.instance.levelText(level.toUpperCase());
            exports.instance.visible(true);

            version++;
            if (level !== 'error')
                hideAfterTimeOut();
        };
        
        //this will be used when diciding if infobar should autohide
        var version = 0;
        var hideCallback = function (ver) {
            if (ver >= version) {
                self.visible(false);
            }
        };

        var millisecondsTillHide = 1000 * 10; 
        function hideAfterTimeOut() {
            var ver = version;
            setTimeout(function () {
                hideCallback(ver);
            }, millisecondsTillHide);
        }
    };

    exports.instance = new Infobar();

    exports.show = function(level, message) {
        exports.instance.show(level, message);
    };

    exports.showError = function(message) {
        exports.instance.show("error", message);
    };
    
    exports.showInfo = function (message) {
        exports.instance.show("info", message);
    };
    
    return exports;
});