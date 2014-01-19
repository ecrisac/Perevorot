define(["knockout", "jquery", "datatables", "datatablesknockout"], function(ko, jQuery) {

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

    /* View model */
    var groupViewModel = {
        groupMembers: ko.observableArray([
            new GroupMember("1", "Abe", true),
            new GroupMember("2", "Bob", false),
            new GroupMember("3", "Bill", false)])
    };

    groupViewModel.membersTable = ko.computed(function() {
        var self = this;

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
