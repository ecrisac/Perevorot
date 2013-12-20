define(['durandal/app', 'knockout', 'knockout-jqueryui'], function (app, ko) {
    var _self;
    var addCustomer = function () {
        
        _self = this;
        
        this.isOpen = ko.observable(false);
        this.customerName = ko.observable('Lorem ipsum dolor sit amet.');
        
        this.OpenAddCustomerDialog = function () {
            this.isOpen(true);
        };
        
        this.SaveCustomer = function () {
            console.log("sending data to server" + this.customerName());
        };

    };
 
    return addCustomer;
});