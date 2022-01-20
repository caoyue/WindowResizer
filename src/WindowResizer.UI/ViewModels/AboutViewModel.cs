using System.Diagnostics;
using System.Reflection;
using WPFUI.Common;

namespace WindowResizer.UI.ViewModels
{
    internal class AboutViewModel : BaseViewModel
    {
        private static readonly string _version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        public string RepoUrl => "https://github.com/caoyue/WindowResizer";

        public string Version => _version;

        public RelayCommand GoToPageCommand => new(GoToPage);

        public RelayCommand CheckUpdateCommand => new(CheckUpdate);

        private void CheckUpdate()
        {
        }

        private void GoToPage()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = RepoUrl,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
