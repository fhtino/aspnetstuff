## Step by step
- create an empty web application - .NET Framework (4.6.2 in my case)
- add required Nuget packages:
   - Microsoft.AspNet.WebApi 5.2.7 + required packages
     - (this add a new section under web.config <system.webServer <handlers )
   - update Newtonsoft.Json
   - add global.asax
   - global.asax, in Application_Start add GlobalConfiguration.Configure(Register);
   - create Controllers folder
   - Swashbuckle (swagger) - optional

...  
...  

   - ???  Microsoft.AspNet.Mvc 5.2.7   [not required]
   

## Links
https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api
