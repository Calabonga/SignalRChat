using Prism.Mvvm;

namespace Calabonga.Chat.WpfClient.ViewModels
{
    /// <summary>
    /// Shell ViewModel
    /// </summary>
    public class ShellViewModel : BindableBase {
 
        #region property DisplayName
 
        /// <summary>
        /// Represent DisplayName property
        /// </summary>
        public string DisplayName {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }
 
        /// <summary>
        /// Backing field for property DisplayName
        /// </summary>
        private string _displayName = "WPF Chat Client";
 
        #endregion
 
    }
}
