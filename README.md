# Update
## 2017-12-28
Update Packages

## 2017-10-18
Separate Data part from main project

Add publish setting

# JWTDemo
A demo of .Net Core 2 Web Api with JWT

Please open the solution with Visual Studio 2017 Community 
(https://www.visualstudio.com/zh-hant/thank-you-downloading-visual-studio/?sku=Community&rel=15)

Make sure updated to .Net Core 2 (https://www.microsoft.com/net/download/core)

1. Update-Datebase (create structure only)
2. Run (seed data)
3. Then you can test the api(s), you can also use the ConsoleApp1 to test the api

# ConsoleApp1
A console app to test the api.
Aim to demo how to call jwt api in C# program

Also, please open it with visual studio 2017 and .Net Core 2

# .Net Core
How to publish , if target platform is `win10-x64`
1. Edit .csproj, add 
  `<PropertyGroup>
    <RuntimeIdentifier>win10-x64</RuntimeIdentifier>
  </PropertyGroup>`
2. In Nuget package console, `dotnet restore` to makesure all packages are installed properly.
3. In Nuget package console, `dotnet restore -r win10-x64` to install packages which can run in the target platform.
4. Right click project select `Publish`
5. .exe will found in `bin\Release\netcoreapp2.0\target platform`


# Reference

Call JWT api in ajax:
https://stackoverflow.com/questions/35861012/how-to-send-a-token-with-an-ajax-request-from-jquery

JWT Document:
https://tools.ietf.org/html/rfc7519

jwt-dotnet document:
https://github.com/jwt-dotnet/jwt

To test and decode JWT:
https://jwt.io/

Deploying .NET Core apps with Visual Studio
https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-vs


