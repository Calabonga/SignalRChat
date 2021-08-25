using System.Windows;
using Calabonga.Chat.WpfClient.Views;
using Prism.Ioc;

namespace Calabonga.Chat.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Used to register types with the container that will be used by your application.
        /// </summary>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        /// <summary>Creates the shell or main window of the application.</summary>
        /// <returns>The shell of the application.</returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }
    }
}
