define(["knockout", "jquery", "moment", "datatables", "datatablesknockout"], function(ko, $, moment) {

    /* Object code */
    function customerRow(id, name, creationDate) {
        var self = this;
        self.id = id;
        self.name = name;
        self.creationDate = ko.observable(creationDate.format(window.DateTimeFormat));

        var completedFields = Math.floor(Math.random() * 21);
        var registeredCalls = Math.floor(Math.random() * 6);
        var details = "";
        self.completedFields = ko.observable(completedFields);
        self.registeredCalls = ko.observable(registeredCalls);
        self.details = details;


        //self.link = ko.computed(function() {
        //    return "/#user/" + self.id;
        //});
        
        self.customerId = ko.computed(function () {
            return '<a href="#' + self.id + '">' + self.name + '</a>';
        });

        self.nameWithLink = ko.computed(function() {
            return '<a href="#' + self.id + '">' + self.name + '</a>';
        });
        
        self.creationDate = ko.computed(function () {
            return '<span>' + self.creationDate() + '</span>';
        });
        
        self.completedFields = ko.computed(function () {
            return '<span>' + self.completedFields() + '</span>';
        });
        
        self.registeredCalls = ko.computed(function () {
            return '<span>' + self.registeredCalls() + '</span>';
        });
        
        self.details = ko.computed(function () {
            return '<span class="glyphicon glyphicon-cog"></span>';
        });

        //self.actions = ko.computed(function() {
        //    return '<a class="btn btn-danger" data-bind="click: function() {removeMember(' + self.id + ')}">' + '<i class="icon-minus-sign"></i>' + '</a>';
        //});
    }

    var getData = function () {
        var resultArray = [];
        var actionUrl = "../Customer/GetCustomers";
        $.ajax({
            type: "POST",
            async :false,
            url: actionUrl,
            //data: $("#loginForm").serialize(), // serializes the form's elements.
            success: function(data) {
                $.each(data, function (index, value) {
                    resultArray.push(new customerRow(value.Id, value.CustomerName, moment(value.CreationDate)));
                });
                
            }
        });
        return resultArray;
    };

    /* View model */
    var customerViewModel = {        
        customers: ko.observableArray([])
    };

    customerViewModel.dateRenderer = function (date) {
        return moment(date.aData.CreationDate).format(window.DateTimeFormat);
    };

    customerViewModel.customerTableRows = ko.computed(function() {
        var dataFromServer = [];//getData();

        var finalArray = new Array();        
        $.each(dataFromServer, function (index, value) {
            var rowArray = new Array();
            rowArray.push(value.customerId());
            rowArray.push(value.nameWithLink());
            rowArray.push(value.creationDate());
            rowArray.push(value.completedFields());
            rowArray.push(value.registeredCalls());
            rowArray.push(value.details());
            finalArray.push(rowArray);
        });
       
        return finalArray;
    }, customerViewModel);



    //customerViewModel.removeMember = function(id) {
    //    var self = this;

    //    self.customers.remove(function(groupMember) {
    //        return groupMember.id == id;
    //    });
    //};


    return customerViewModel;
});
