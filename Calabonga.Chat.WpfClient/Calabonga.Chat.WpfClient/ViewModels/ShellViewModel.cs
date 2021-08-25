using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using Calabonga.Chat.ConsoleClient.TokenHelper;
using Calabonga.Chat.WpfClient.TokenHelper;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using Prism.Mvvm;

namespace Calabonga.Chat.WpfClient.ViewModels
{
    /// <summary>
    /// Shell ViewModel
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        #region Fields

        const string ChatUrl = "https://localhost:20001/chat";
        private HubConnection _connection = null!;

        #endregion

        public ShellViewModel()
        {
            Initialize();
        }

        #region Properties

        #region property IsAuthenticated

        /// <summary>
        /// Represent IsAuthenticated property
        /// </summary>
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                SetProperty(ref _isAuthenticated, value);
                ConnectCommand.RaiseCanExecuteChanged();
                DisconnectCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Backing field for property IsAuthenticated
        /// </summary>
        private bool _isAuthenticated;

        #endregion

        #region property DisplayName

        /// <summary>
        /// Represent DisplayName property
        /// </summary>
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);

        }

        /// <summary>
        /// Backing field for property DisplayName
        /// </summary>
        private string _displayName = "WPF-Client for SignalR Chat";

        #endregion

        #region property UserName

        /// <summary>
        /// Represent UserName property
        /// </summary>
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Backing field for property UserName
        /// </summary>
        private string _userName = "user1@yopmail.com";

        #endregion

        #region property AuthServerUrl

        /// <summary>
        /// Represent AuthServerUrl property
        /// </summary>
        public string AuthServerUrl
        {
            get => _authServerUrl;
            set => SetProperty(ref _authServerUrl, value);
        }

        /// <summary>
        /// Backing field for property AuthServerUrl
        /// </summary>
        private string _authServerUrl = "https://localhost:10001";

        #endregion

        #region property ChatServerUrl

        /// <summary>
        /// Represent ChatServerUrl property
        /// </summary>
        public string ChatServerUrl
        {
            get => _chatServerUrl;
            set => SetProperty(ref _chatServerUrl, value);
        }

        /// <summary>
        /// Backing field for property ChatServerUrl
        /// </summary>
        private string _chatServerUrl = "https://localhost:20001";

        #endregion

        #region property AccessToken

        /// <summary>
        /// Represent AccessToken property
        /// </summary>
        public string? AccessToken
        {
            get => _accessToken;
            set => SetProperty(ref _accessToken, value);
        }

        /// <summary>
        /// Backing field for property AccessToken
        /// </summary>
        private string? _accessToken;

        #endregion

        #region property UserList

        /// <summary>
        /// Represent UserList property
        /// </summary>
        public ObservableCollection<string> UserList
        {
            get => _userList;
            set => SetProperty(ref _userList, value);
        }

        /// <summary>
        /// Backing field for property UserList
        /// </summary>
        private ObservableCollection<string> _userList = new();

        #endregion

        #region property IsConnected

        /// <summary>
        /// Represent IsConnected property
        /// </summary>
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                SetProperty(ref _isConnected, value);
                ConnectCommand.RaiseCanExecuteChanged();
                DisconnectCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Backing field for property IsConnected
        /// </summary>
        private bool _isConnected;

        #endregion

        #region property MessageList

        /// <summary>
        /// Represent MessageList property
        /// </summary>
        public ObservableCollection<string> MessageList
        {
            get => _messageList;
            set => SetProperty(ref _messageList, value);
        }

        /// <summary>
        /// Backing field for property MessageList
        /// </summary>
        private ObservableCollection<string> _messageList = new();

        #endregion

        #region property MessageText

        /// <summary>
        /// Represent MessageText property
        /// </summary>
        public string? MessageText
        {
            get => _messageText;
            set
            {
                SetProperty(ref _messageText, value);
                SendCommand!.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Backing field for property MessageText
        /// </summary>
        private string? _messageText;

        #endregion

        #endregion

        #region Commands

        #region LoginCommand

        /// <summary>
        /// The Login DelegateCommand
        /// </summary>
        public DelegateCommand<PasswordBox> LoginCommand { get; private set; } = null!;

        /// <summary>
        /// Validation for availability to execute LoginCommand
        /// </summary>
        private bool LoginCommandCanExecute(PasswordBox passwordBox)
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }

        /// <summary>
        /// Execute method for LoginCommand
        /// </summary>
        private async void LoginCommandExecute(PasswordBox passwordBox)
        {
            var password = passwordBox.Password;
            if (string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            try
            {
                var token = await GetToken(UserName, password, $"{AuthServerUrl}/connect/token");
                if (token is null)
                {
                    return;
                }
                AccessToken = token.AccessToken;
                IsAuthenticated = !string.IsNullOrWhiteSpace(token.AccessToken);

            }
            catch (Exception exception)
            {
                MessageList.Add(exception.Message);
            }
        }

        #endregion // LoginCommand region

        #region ClearTokenCommand

        /// <summary>
        /// The ClearToken DelegateCommand
        /// </summary>
        public DelegateCommand ClearTokenCommand { get; private set; } = null!;

        /// <summary>
        /// Execute method for ClearTokenCommand
        /// </summary>
        private async void ClearTokenCommandExecute()
        {
            AccessToken = null;
            IsAuthenticated = false;
            await DisconnectFromChatAsync();
        }

        #endregion // ClearTokenCommand region

        #region ConnectCommand

        /// <summary>
        /// The Connect DelegateCommand
        /// </summary>
        public DelegateCommand ConnectCommand { get; private set; } = null!;

        private bool ConnectCommandCanExecute()
        {
            return IsAuthenticated && !IsConnected;
        }

        /// <summary>
        /// Execute method for ConnectCommand
        /// </summary>
        private async void ConnectCommandExecute()
        {
            await ConnectToChatAsync();
        }

        #endregion // ConnectCommand region

        #region DisconnectCommand

        /// <summary>
        /// The Disconnect DelegateCommand
        /// </summary>
        public DelegateCommand DisconnectCommand { get; private set; } = null!;

        /// <summary>
        /// Execute method for DisconnectCommand
        /// </summary>
        private async void DisconnectCommandExecute()
        {
            await DisconnectFromChatAsync();
        }

        #endregion // DisconnectCommand region

        #region SendCommand

        /// <summary>
        /// The Send DelegateCommand
        /// </summary>
        public DelegateCommand? SendCommand { get; private set; }

        /// <summary>
        /// Validation for availability to execute SendCommand
        /// </summary>
        private bool SendCommandCanExecute()
        {
            return IsConnected
                   && !string.IsNullOrWhiteSpace(AccessToken)
                   && !string.IsNullOrWhiteSpace(UserName)
                   && !string.IsNullOrWhiteSpace(MessageText);
        }

        /// <summary>
        /// Execute method for SendCommand
        /// </summary>
        private async void SendCommandExecute()
        {
            await SendMessageAsync(UserName, MessageText);
            MessageText = string.Empty;
        }

        #endregion // SendCommand region

        #endregion

        #region privates

        private void Initialize()
        {
            LoginCommand = new DelegateCommand<PasswordBox>(LoginCommandExecute, LoginCommandCanExecute);
            ClearTokenCommand = new DelegateCommand(ClearTokenCommandExecute).ObservesCanExecute(() => IsAuthenticated);
            ConnectCommand = new DelegateCommand(ConnectCommandExecute, ConnectCommandCanExecute);
            DisconnectCommand = new DelegateCommand(DisconnectCommandExecute).ObservesCanExecute(() => IsConnected);
            SendCommand = new DelegateCommand(SendCommandExecute, SendCommandCanExecute);
        }

        private async Task<SecurityToken?> GetToken(string userName, string password, string serverUrl)
        {
            var token = await TokenLoader.RequestToken(userName, password, serverUrl);

            if (!string.IsNullOrWhiteSpace(token?.AccessToken))
            {
                return token;
            }

            MessageList.Add("Cannot obtain token from IdentityServer");
            return null;

        }

        private async Task SendMessageAsync(string userName, string? message)
        {
            await _connection.SendAsync("SendMessageAsync", userName, message);
        }

        private async Task ConnectToChatAsync()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(ChatUrl, options =>
                {
                    // custom Token Provider if you needed
                    // options.AccessTokenProvider =

                    options.Headers.Add("Authorization", $"Bearer {AccessToken}");
                })
                .WithAutomaticReconnect()
                .Build();


            _connection.On<IEnumerable<string>>("UpdateUsersAsync", users =>
            {
                UserList = new ObservableCollection<string>(users);
            });

            _connection.On<string, string>("SendMessageAsync", (user, message) =>
            {
                var item = $"{user} says {message}";
                MessageList.Add(item);
            });

            try
            {
                await _connection.StartAsync();
                IsConnected = true;
            }
            catch (Exception exception)
            {
                MessageList.Add(exception.Message);
            }
        }

        private async Task DisconnectFromChatAsync()
        {
            if (IsConnected)
            {
                await _connection.StopAsync();
                IsConnected = false;
                await _connection.DisposeAsync();
                UserList = new ObservableCollection<string>();
                MessageList = new ObservableCollection<string>();
            }
        }

        #endregion
    }
}
