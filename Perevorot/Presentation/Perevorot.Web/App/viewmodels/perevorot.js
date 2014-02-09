define(["knockout", "infobar"], function (ko, infobar) {
    var ctor = function () {
        var self = this;
        self.displayName = "Perevorot, a test page";

        self.showTime = function () {
            infobar.show(new Date(), "info");
        }

        self.showError = function () {
            infobar.show("Error message", "error");
        }
    };

    //Note: This module exports a function. That means that you, the developer, can create multiple instances.
    //This pattern is also recognized by Durandal so that it can create instances on demand.
    //If you wish to create a singleton, you should export an object instead of a function.
    //See the "flickr" module for an example of object export.

    return ctor;
});