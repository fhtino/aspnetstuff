# My Asp.Net stuff
A collection of miscellaneous stuff about Asp.Net: small sample projects, demos, code snippets, etc.

## OWIN Auth Sample
A simple example about using OWIN authentication without Asp.Net identity.  
The web-app uses Google and Microsoft authentication.   
There is also a custom extension for adding roles and storing user information in a fake database.  
...   

Tips:  
- Starting from Owin 4.1.0, autethentication only works on https connections:
https://github.com/aspnet/AspNetKatana/issues/341#issuecomment-600905630 
- old application seem not working with Owin 4.0.0 - https://github.com/aspnet/AspNetKatana/issues/212

Links:  
- Google Developer Console - https://console.developers.google.com
- Microsoft Application Registration Portal - https://apps.dev.microsoft.com/
- How to setup MSFT application: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins

## Inject Values in Web.Config
Sample about how to modify/inject values in web.config `<appSettings>` and `<connectionStrings>` at application start-up.
To automatically run the code, there are two options:
 - use the PreApplicationStartMethod attribute on the web-application assembly
 - call the code from global.asax --> Application_Start

## WebApi2Simple
Web API 2 sample application. It contatins only required libraries.
Swagger installed for debug / testing.
Apis:
 - Car : classic get, post, put, delete
 - Operations : "operation" style, to mimic soap services operations
 - Data : ...
   
  
## Render GitHub MD file
Render MD files from GitHub using Markdig and show in asp.net page


## Asp.Net Big file upload experiments
Using javascript to upload big files to asp.net website, dividing files in variable length chunks, dynamically adapting to available bandwidth.
Details here: [AspNet big files - README.md](AspNetBigFiles_common/README.md)




