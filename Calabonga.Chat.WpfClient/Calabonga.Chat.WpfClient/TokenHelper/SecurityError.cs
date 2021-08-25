using System.Text.Json.Serialization;

namespace Calabonga.Chat.ConsoleClient.TokenHelper
{
    /// <summary>
    /// Server authentication error
    /// </summary>
    public class SecurityError
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string Description { get; set; }
    }
}