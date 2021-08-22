using System;

namespace Calabonga.Chat.API.Web.Hubs
{
    /// <summary>
    /// User connection form one of the device (web)
    /// </summary>
    public class ChatConnection
    {
        /// <summary>
        /// Registered at time
        /// </summary>
        public DateTime ConnectedAt { get; set; }

        /// <summary>
        /// Connection Id from client
        /// </summary>
        public string ConnectionId { get; set; } = null!;
    }
}
