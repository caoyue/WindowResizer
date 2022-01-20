using System.Windows;

namespace WindowResizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            WPFUI.Theme.Manager.SetSystemTheme(true, true);
        }
    }
}
