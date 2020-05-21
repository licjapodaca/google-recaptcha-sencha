# Google reCaptcha V3 implemented over a SPA Sencha ExtJS solution

This solution it's conformed by different technologies like the following:

- ASP.NET Core (MVC and WebAPI)
- C#
- Sencha ExtJS (Javascript Framework)
- Google reCaptcha V3

There are 3 projects located in 2 sections, at Frontend 2 projects and in Backend 1 project.

To setup the projects, you need to do the following:

- For project called **frontend\mvc** you need to handle the "dotnet user-secrets" feature to add the following secret:

```json
{
  "GoogleRecaptchaSiteKey": "[Specify the Site Key from Google reCaptcha Console]"
}
```

- For project called **backend** you need to handle the "dotnet user-secrets" feature to add the following secrets:

```json
{
  "GoogleRecaptchaSecretKey": "[Specify the Secret Key from Google reCaptcha Console]",
  "GoogleRecaptchaUrlValidationToken": "https://www.google.com/recaptcha/api/siteverify"
}
```

- For project called **frontend\extjs** you must recreate the framework folder `ext` using Sencha Command and the Sencha ExtJS SDK version 7.2.
