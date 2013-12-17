define(["knockout"],function (ko) {
    var ctor = function () {
        var self = this;
        self.displayName = 'Perevorot, a test page';
        self.counter = ko.observable(0);

        var incCounter = function () {
            self.counter(self.counter() + 1);

            setTimeout(incCounter, 1000);
        }

        incCounter();
    };

    //Note: This module exports a function. That means that you, the developer, can create multiple instances.
    //This pattern is also recognized by Durandal so that it can create instances on demand.
    //If you wish to create a singleton, you should export an object instead of a function.
    //See the "flickr" module for an example of object export.

    return ctor;
});