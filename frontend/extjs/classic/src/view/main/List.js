/**
 * This view is an example list of people.
 */
Ext.define('Recaptcha.view.main.List', {
    extend: 'Ext.grid.Panel',
    xtype: 'mainlist',

    requires: [
        'Recaptcha.store.Personnel'
    ],

    title: 'Personnel',

    store: {
        type: 'personnel'
	},
	
	tbar: [
		{
			text: "Registrar Suscripción (Google reCaptcha V3)",
			handler: function() {
				window.grecaptcha.ready(function() {
					window.grecaptcha.execute(window.GoogleRecaptchaSiteKey, {action: 'register'}).then(function(token) {
						// Add your logic to submit to your backend server here.
						//console.log(token);
						window.GoogleRecaptchaToken = token;
						Ext.Ajax.request({
							url: 'https://localhost:5003/subscription/register',
							method: 'POST',
							headers: {
								"Content-Type": "application/json",
								"Accept": "application/json",
								"GoogleRecaptchaToken": token
							},
							callback: function(opts, success, response) {
								console.log(Ext.decode(response.responseText));
							}
						});
					});
				  });
			}
		},
		{
			text: "Registrar Suscripción (Google reCaptcha V3) - Same Token",
			handler: function() {
				Ext.Ajax.request({
					url: 'https://localhost:5003/subscription/register',
					method: 'POST',
					headers: {
						"Content-Type": "application/json",
						"Accept": "application/json",
						"GoogleRecaptchaToken": window.GoogleRecaptchaToken
					},
					callback: function(opts, success, response) {
						console.log(Ext.decode(response.responseText));
					}
				});
			}
		}
	],

    columns: [
        { text: 'Name',  dataIndex: 'name' },
        { text: 'Email', dataIndex: 'email', flex: 1 },
        { text: 'Phone', dataIndex: 'phone', flex: 1 }
    ],

    listeners: {
        select: 'onItemSelected'
    }
});
