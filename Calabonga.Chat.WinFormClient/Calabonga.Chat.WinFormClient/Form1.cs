using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calabonga.Chat.WinFormClient.TokenHelper;
using Microsoft.AspNetCore.SignalR.Client;

namespace Calabonga.Chat.WinFormClient
{
    public partial class Form1 : Form
    {
        private HubConnection? _connection;

        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonGetToken_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxUserName.Text))
            {
                listBoxMessages.Items.Add("User name not provided!");
                return;
            }

            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                listBoxMessages.Items.Add("Password not provided!");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                var token = await TokenLoader.RequestToken(textBoxUserName.Text, textBoxPassword.Text, AppData.ServerTokenUrl);

                if (token == null)
                {
                    listBoxMessages.Items.Add("Cannot obtain token from IdentityServer!");
                    return;
                }

                labelAccessToken.Text = token.AccessToken;
                buttonConnect.Visible = true;
                buttonConnect.Enabled = true;
                buttonDisconnect.Visible = true;
                buttonDisconnect.Enabled = false;
                Cursor = Cursors.Default;
            }
            catch (Exception exception)
            {
                listBoxMessages.Items.Add(exception.Message);
                Cursor = Cursors.Default;
            }
        }

        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            await ConnectToChatAsync();
        }

        private async Task ConnectToChatAsync()
        {
            Cursor = Cursors.WaitCursor;
            _connection = new HubConnectionBuilder()
                .WithUrl(AppData.ChatServerUrl, options =>
                {
                    // custom Token Provider if you needed
                    // options.AccessTokenProvider =

                    options.Headers.Add("Authorization", $"Bearer {labelAccessToken.Text}");
                })
                .WithAutomaticReconnect()
                .Build();


            _connection.On<IEnumerable<string>>("UpdateUsersAsync", users =>
            {
                listBoxUsers.Items.Clear();
                foreach (var user in users)
                {
                    listBoxUsers.Items.Add(user);
                }
            });

            _connection.On<string, string>("SendMessageAsync", (user, text) =>
            {
                var item = $"{user} says {text}";
                listBoxMessages.Items.Add(item);
            });

            try
            {
                await _connection!.StartAsync();
                panelSendMessage.Visible = true;
                buttonDisconnect.Enabled = true;
                buttonConnect.Enabled = false;
                Cursor = Cursors.Default;
            }
            catch (Exception exception)
            {
                listBoxMessages.Items.Add(exception.Message);
                Cursor = Cursors.Default;
            }
        }

        private async Task DisconnectFromChatAsync()
        {
            if (_connection != null)
            {
                await _connection.StopAsync();
                await _connection.DisposeAsync();
                listBoxMessages.Items.Clear();
                listBoxUsers.Items.Clear();
                panelSendMessage.Visible = false;
                buttonDisconnect.Enabled = false;
                buttonConnect.Enabled = true;
            }
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(textBoxMessageText.Text))
            {
                listBoxMessages.Items.Add("Message text not provided!");
                Cursor = Cursors.Default;
                return;
            }

            if (_connection == null)
            {
                Cursor = Cursors.Default;
                return;
            }

            if (string.IsNullOrEmpty(textBoxUserName.Text))
            {
                Cursor = Cursors.Default;
                listBoxMessages.Items.Add("User name not provided!");
                return;
            }

            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                Cursor = Cursors.Default;
                listBoxMessages.Items.Add("Password not provided!");
                return;
            }

            await _connection.SendAsync("SendMessageAsync", textBoxUserName.Text, textBoxMessageText.Text);
            Cursor = Cursors.Default;
        }

        private async void buttonDisconnect_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            await DisconnectFromChatAsync();
            Cursor = Cursors.Default;
        }
    }
}
