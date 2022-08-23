using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WindowResizer.UI.Services;
using ServiceProvider = WindowResizer.UI.Services.ServiceProvider;

namespace WindowResizer.UI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            SetupTrayIcon();
        }

        private void SetupTrayIcon()
        {
            var trayContext = ServiceProvider.GetService<ITrayContext>();

            if (trayContext != null)
            {
                trayContext.Initialize();
                trayContext.ClickHandler = () =>
                    ServiceProvider.GetService<INotificationService>()
                        ?.ShowNotification(nameof(WindowResizer), "Welcome to WindowResizer!");
            }
        }
    }
}
