using System.Text.Json.Serialization;

namespace Calabonga.Chat.ConsoleClient.TokenHelper
{
    /// <summary>
    /// This is a Token for deserialization from 
    /// </summary>
    public class SecurityToken
    {

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}