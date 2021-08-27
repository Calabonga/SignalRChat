using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calabonga.Chat.ConsoleClient.TokenHelper
{
    /// <summary>
    /// Token request helper
    /// </summary>
    public static class TokenLoader
    {
        /// <summary>
        /// Sends request to IdentityServer for access token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="tokenServerUrl"></param>
        /// <returns></returns>
        public static Task<SecurityToken> RequestToken(string userName, string password, string tokenServerUrl)
        {
            var content = GetContent(userName, password);
            return GetTokenAsync(content, tokenServerUrl);
        }

        /// <summary>
        /// Generates data for request (Encoded Form) 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private static FormUrlEncodedContent GetContent(string userName, string password)
        {
            var values = new List<KeyValuePair<string, string>>{
                new("grant_type","password"),
                new("username", userName),
                new("password", password),
                new("client_secret", "secret"),
                new("client_id", "microservice1"),
                new("scope", "api1")
            };

            return new FormUrlEncodedContent(values);
        }

        /// <summary>
        /// Returns token
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tokenServerUrl"></param>
        private static async Task<SecurityToken> GetTokenAsync(FormUrlEncodedContent content, string tokenServerUrl)
        {
            string responseResult;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"{tokenServerUrl}", content);
                if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        Console.WriteLine(responseText);
                        return null;
                    }
                }

                response.EnsureSuccessStatusCode();
                responseResult = await response.Content.ReadAsStringAsync();
            }
            try
            {
                if (!string.IsNullOrEmpty(responseResult))
                {
                    return JsonSerializer.Deserialize<SecurityToken>(responseResult);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return null;
        }
    }
}
