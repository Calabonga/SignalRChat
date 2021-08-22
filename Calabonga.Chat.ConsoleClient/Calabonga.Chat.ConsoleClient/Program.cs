using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calabonga.Chat.ConsoleClient.TokenHelper;
using Microsoft.AspNetCore.SignalR.Client;

namespace Calabonga.Chat.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string defaultUserName = "user1@yopmail.com";
            const string password = "123qwe!@#";
            const string serverUrl = "https://localhost:10001/connect/token";
            const string chatUrl = "https://localhost:20001/chat";

            var userName = defaultUserName;
            if (args?.Length > 0)
            {
                userName = args[0];
            }

            Console.WriteLine("Getting token...");
            var token = await TokenLoader.RequestToken(userName, password, serverUrl);

            if (string.IsNullOrWhiteSpace(token?.AccessToken))
            {
                Console.WriteLine("Request token error. Exit.");
                return;
            }

            Console.WriteLine("Creating connection...");
            var connection = new HubConnectionBuilder()
                .WithUrl(chatUrl, options =>
                {
                    // custom Token Provider if you needed
                    // options.AccessTokenProvider =

                    options.Headers.Add("Authorization", $"Bearer {token.AccessToken}");
                })
                .WithAutomaticReconnect()
                .Build();

            Console.WriteLine("Subscribe to actions...");

            #region subscriptions

            connection.On<IEnumerable<string>>("UpdateUsersAsync", users =>
            {
                Console.WriteLine("--------------------------------");
                var enumerable = users as string[] ?? users.ToArray();
                Console.WriteLine($"Total users: {enumerable.Length}");
                foreach (var user in enumerable)
                {
                    Console.WriteLine($"{user}");
                }
            });

            connection.On<string, string>("SendMessageAsync", (user, message) =>
             {
                 Console.WriteLine("--------------------------------");
                 Console.WriteLine($"{user} says {message}");
             });

            #endregion

            try
            {
                connection.StartAsync().GetAwaiter().GetResult();
                var messageHelper = new MessageHelper();

                while (true)
                {

                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        await connection.SendAsync("SendMessageAsync", userName, messageHelper.GetRandom());
                    }

                    if (key.Key == ConsoleKey.Enter)
                    {
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
