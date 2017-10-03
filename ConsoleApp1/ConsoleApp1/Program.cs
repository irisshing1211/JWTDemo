using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = GetToken("http://localhost:3460/api/Login/Login", "User1", "12345");
            Console.WriteLine(token);
            GetAllAccount(token);
            CreateUser(token);
            Console.ReadKey();
        }
        static string GetToken(string url, string userName, string password)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post
                };
                request.Content = new StringContent("{\"UserName\":\"User1\",\"Password\":\"12345\"}",
                                    Encoding.UTF8,
                                    "application/json");
                var response = client.SendAsync(request).Result;
                var context = JsonConvert.DeserializeObject<TokenClass>(response.Content.ReadAsStringAsync().Result);
                return context.access_token;
            }

        }
        static void GetAllAccount(string token)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://localhost:3460/api/account"),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Authorization", $"Bearer {token}");
                var response = client.SendAsync(request).Result;
                var context = response.Content.ReadAsStringAsync();

            }
        }
        static void CreateUser(string token)
        {
            var newacc = new NewAccountResponseModels();
            Console.Write("User Name:");
            newacc.UserName = Console.ReadLine();
            Console.Write("Passwprd: ");
            newacc.Password = Console.ReadLine();
            Console.Write("Email: ");
            newacc.Email = Console.ReadLine();
            Console.Write("Role ID:");
            newacc.RoleID = int.Parse(Console.ReadLine());

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://localhost:3460/api/account/create"),
                    Method = HttpMethod.Post
                };
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Content = new StringContent(JsonConvert.SerializeObject(newacc),
                                    Encoding.UTF8,
                                    "application/json");
                var response = client.SendAsync(request).Result;
                var context = response.StatusCode;
                Console.WriteLine(context);
            }

        }
    }
    class TokenClass
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
    public class NewAccountResponseModels
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public int RoleID { get; set; }
    }
}
