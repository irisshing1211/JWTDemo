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
Aim to demo how th=o call jwt api in C# program

Also, please open it with visual studio 2017 and .Net Core 2

# Call JWT api in jquery

$.ajax({
    type: "POST", //GET, POST, PUT
    url: '/authenticatedService'  //the url to call
    data: yourData,     //Data sent to server
    contentType: contentType,           
    beforeSend: function (xhr) {   //Include the bearer token in header
        xhr.setRequestHeader("Authorization", 'Bearer '+ jwt);
    }
}).done(function (response) {
    //Response ok. process reuslt
}).fail(function (err)  {
    //Error during request
});


