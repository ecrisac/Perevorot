requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions',
        'jquery': '../Scripts/jquery-2.0.3.min',
        'jquery-ui': '../Scripts/jquery-ui-1.10.3.min',
        'knockout': '../Scripts/knockout-3.0.0',
        'knockout-jqueryui': '../Scripts/knockout-jqueryui.min'
    }
});

//define('jquery', function () { return jQuery; });
//define('knockout', ko);

define(['durandal/app', 'durandal/viewLocator'], function (app, viewLocator) {
    //>>excludeStart("build", true);
    //system.debug(true);
    //>>excludeEnd("build");

    app.title = 'Durandal Starter Kit';

    app.configurePlugins({
        router: true,
        dialog: true,
        widget: true
    });
    
    //TODO: Remove later to speed up loading
    setTimeout(appStart, 500);

    function appStart(parameters) {
        app.start().then(function () {
            //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
            //Look for partial views in a 'views' folder in the root.
            viewLocator.useConvention();

            //Show the app by setting the root view model for our application with a transition.
            app.setRoot('viewmodels/shell', 'entrance');
        });
    }
   
});