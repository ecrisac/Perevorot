define(['durandal/app', 'knockout', 'jquery','knockout-jqueryui'], function (app, ko, jq) {
    var _self;
    var addCustomer = function () {
        
        _self = this;
        
        _self.isOpen = ko.observable(false);
        _self.customerName = ko.observable();
        
        _self.OpenAddCustomerDialog = function () {
            _self.isOpen(true);
        };
        
        this.SaveCustomer = function () {
            console.log("sending data to server" + this.customerName());    
            jq.ajax({
                url: "/Customer/AddNewCustomer",
                type: 'POST',
                cache: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    Name: _self.customerName()                
                }),
                success: function (returnedData) {
                    if (returnedData.Result == "Success") {
                        //cleanup and close window
                        _self.customerName('');
                        _self.isOpen(false);
                    } else {
                        alert(returnedData.Message);
                    }
                },
                error: function (jqXHR, exception) {
                    alert('Error happened while sending request:' + exception);
                }
            });
        };

    };
 
    return addCustomer;
});