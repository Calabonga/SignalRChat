using System;
using System.Collections.Generic;
using System.Linq;

namespace Calabonga.Chat.API.Web.Hubs
{
    /// <summary>
    /// Users manager for chat
    /// </summary>
    public class ChatManager
    {
        public List<ChatUser> Users { get; } = new();

        public void ConnectUser(string userName, string connectionId)
        {
            var userAlreadyExists = GetConnectedUserByName(userName);
            if (userAlreadyExists != null)
            {
                userAlreadyExists.AppendConnection(connectionId);
                return;
            }

            var user = new ChatUser(userName);
            user.AppendConnection(connectionId);
            Users.Add(user);
        }

        /// <summary>
        /// Disconnect user from connection.
        /// If we found the connection is last, than we remove user from user list.
        /// </summary>
        /// <param name="connectionId"></param>
        public bool DisconnectUser(string connectionId)
        {
            var userExists = GetConnectedUserById(connectionId);
            if (userExists == null)
            {
                return false;
            }

            if (!userExists.Connections.Any())
            {
                return false; // should never happen, but...
            }

            var connectionExists = userExists.Connections.Select(x => x.ConnectionId).First().Equals(connectionId);
            if (!connectionExists)
            {
                return false; // should never happen, but...
            }

            if (userExists.Connections.Count() == 1)
            {
                Users.Remove(userExists);
                return true;
            }

            userExists.RemoveConnection(connectionId);
            return false;
        }

        /// <summary>
        /// Returns <see cref="ChatUser"/> by connectionId if connection found
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        private ChatUser? GetConnectedUserById(string connectionId) =>
            Users
                .FirstOrDefault(x => x.Connections.Select(c => c.ConnectionId)
                .Contains(connectionId));

        /// <summary>
        /// Returns <see cref="ChatUser"/> by userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private ChatUser? GetConnectedUserByName(string userName) =>
            Users
                .FirstOrDefault(x => string.Equals(
                    x.UserName, 
                    userName, 
                    StringComparison.CurrentCultureIgnoreCase));
    }
}
