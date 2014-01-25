define(["knockout", "jquery", "datatables", "datatablesknockout"], function(ko, $) {

    /* Object code */
    function GroupMember(id, name, isGroupLeader) {
        var self = this;
        self.id = id;
        self.name = name;
        self.isGroupLeader = ko.observable(isGroupLeader);

        self.link = ko.computed(function() {
            return "/#user/" + self.id;
        });

        self.nameWithLink = ko.computed(function() {
            return '<a href="' + self.link() + '">' + self.name + '</a>';
        });

        self.actions = ko.computed(function() {
            return '<a class="btn btn-danger" data-bind="click: function() {removeMember(' + self.id + ')}">' + '<i class="icon-minus-sign"></i>' + '</a>';
        });
    }

    var getData = function(array) {
        var actionUrl = "../Customer/GetCustomers";
        $.ajax({
            type: "POST",
            async :false,
            url: actionUrl,
            //data: $("#loginForm").serialize(), // serializes the form's elements.
            success: function(data) {
                $.each(data, function (index, value) {
                    array.push(value);
                });
                
            }
        });
    };

    /* View model */
    var groupViewModel = {        
        groupMembers: ko.observableArray([])
    };

    groupViewModel.membersTable = ko.computed(function() {
        var self = this;
        var datafromserver= [];
        getData(datafromserver);

        var somearr = new Array();
        $.each(datafromserver, function(index, value) {
            somearr.push(new GroupMember(value.id, value.CustomerName, value.CreationDate));
        });

        self.groupMembers(somearr);

        var finalArray = new Array();
        for (var i = 0; i < self.groupMembers().length; i++) {
            var rowArray = new Array();
            rowArray[0] = self.groupMembers()[i].nameWithLink();
            rowArray[1] = self.groupMembers()[i].actions();
            finalArray.push(rowArray);
        }

        return finalArray;
    }, groupViewModel);

    groupViewModel.removeMember = function(id) {
        var self = this;

        self.groupMembers.remove(function(groupMember) {
            return groupMember.id == id;
        });
    };


    return groupViewModel;
});
