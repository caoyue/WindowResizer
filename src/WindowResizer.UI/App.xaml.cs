using Microsoft.AspNetCore.Components;
using Microsoft.Maui.ApplicationModel;
using WindowResizer.UI.Pages;

namespace WindowResizer.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        public static void HandleAppActions(AppAction appAction)
        {
            //App.Current.Dispatcher.Dispatch(async () =>
            //{
            //    if (appAction.Id.Equals("Settings", StringComparison.OrdinalIgnoreCase))
            //    {
            //        await Shell.Current.GoToAsync("counter");
            //    }
            //});
        }
    }
}
