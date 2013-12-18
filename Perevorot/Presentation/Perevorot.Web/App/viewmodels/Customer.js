define(["knockout"], function (ko) {
    var _self;
    var ctor = function () {
        _self = this;
        _self.counter = ko.observable(0);
        _self.counter("1");
    };
    
    this.createNewCustomer = function () {
        _self.counter("2");
    };

  
    //Note: This module exports a function. That means that you, the developer, can create multiple instances.
    //This pattern is also recognized by Durandal so that it can create instances on demand.
    //If you wish to create a singleton, you should export an object instead of a function.
    //See the "flickr" module for an example of object export.

    return ctor;
});