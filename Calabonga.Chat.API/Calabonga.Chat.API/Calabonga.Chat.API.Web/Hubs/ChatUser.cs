using System;
using System.Collections.Generic;
using System.Linq;

namespace Calabonga.Chat.API.Web.Hubs
{
    /// <summary>
    /// User connected to the chat
    /// </summary>
    public class ChatUser
    {
        private readonly List<ChatConnection> _connections;

        public ChatUser(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            _connections = new List<ChatConnection>();
        }

        /// <summary>
        /// User identity name
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// UTC time connected
        /// </summary>
        public DateTime? ConnectedAt
        {
            get
            {
                if (Connections.Any())
                {
                    return Connections
                        .OrderByDescending(x => x.ConnectedAt)
                        .Select(x => x.ConnectedAt)
                        .First();
                }

                return null;
            }
        }

        /// <summary>
        /// All user connections
        /// </summary>
        public IEnumerable<ChatConnection> Connections => _connections;

        /// <summary>
        /// Append connection for user
        /// </summary>
        /// <param name="connectionId"></param>
        public void AppendConnection(string connectionId)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            var connection = new ChatConnection
            {
                ConnectedAt = DateTime.UtcNow,
                ConnectionId = connectionId
            };

            _connections.Add(connection);
        }

        /// <summary>
        /// Remove connection from user
        /// </summary>
        public void RemoveConnection(string connectionId)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            var connection = _connections.SingleOrDefault(x => x.ConnectionId.Equals(connectionId));
            if (connection == null)
            {
                return;
            }
            _connections.Remove(connection);
        }
    }
}
