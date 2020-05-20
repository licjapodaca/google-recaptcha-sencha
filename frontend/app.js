/*
 * This file launches the application by asking Ext JS to create
 * and launch() the Application class.
 */
Ext.application({
    extend: 'Recaptcha.Application',

    name: 'Recaptcha',

    requires: [
        // This will automatically load all classes in the Recaptcha namespace
        // so that application classes do not need to require each other.
        'Recaptcha.*'
    ],

    // The name of the initial view to create.
    mainView: 'Recaptcha.view.main.Main'
});
