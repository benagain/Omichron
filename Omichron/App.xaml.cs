using System.Windows;

namespace Omichron
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public AppBootstrapper AppBootstrapper { get; } = new AppBootstrapper();
    }
}
