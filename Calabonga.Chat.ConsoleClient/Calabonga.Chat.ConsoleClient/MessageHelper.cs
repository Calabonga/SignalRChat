using System;

namespace Calabonga.Chat.ConsoleClient
{
    public class MessageHelper
    {
        private readonly Random _random;
        private readonly string[] _messages = { 
            "Hi there!",
            "Welcome!", 
            "I'a chatting. Cool",
            "The weather is wonderful",
            "Is there anybody in chat?" };

        public MessageHelper()
        {
            _random = new Random();
        }

        internal string GetRandom()
        {
            var index = _random.Next(0, _messages.Length - 1);
            return _messages[index];
        }
    }
}
