using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using WindowResizer.Configuration;
using WPFUI.Controls;

namespace WindowResizer.UI.Views
{
    public partial class ContainerWindow
    {
        public ContainerWindow()
        {
            WPFUI.Background.Manager.Apply(this);

            InitializeComponent();

            RootTitleBar.CloseActionOverride = CloseActionOverride;
            SetTheme();
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                ConfigLoader.Load();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"WindowResizer: config load failed! Exception: {e.Message}");
            }
        }

        private void CloseActionOverride(TitleBar titleBar, Window window)
        {
            Hide();
        }

        private void RootNavigation_OnLoaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate("hotkeys");
        }

        private void RootDialog_Click(object sender, RoutedEventArgs e)
        {
            RootDialog.Show = false;
        }

        private void RootDialog_RightButtonClick(object sender, RoutedEventArgs e)
        {
            RootDialog.Show = false;
        }

        private void RootNavigation_OnNavigated(object sender, RoutedEventArgs e)
        {
            var page = (sender as NavigationFluent)?.PageNow;
            switch (page)
            {
                case "configs":
                {
                    RootDialog.Show = true;
                    break;
                }
            }
        }

        private void TitleBar_OnMinimizeClicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Minimize button clicked");
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem menuItem))
            {
                return;
            }

            string tag = menuItem.Tag.ToString();
            switch (tag)
            {
                case "setting":
                {
                    Show();
                    break;
                }

                case "exit":
                {
                    Application.Current.Shutdown();
                    break;
                }
            }
        }

        private void RootTitleBar_OnNotifyIconClick(object sender, RoutedEventArgs e)
        {
            Show();
            if (!IsActive)
            {
                Activate();
            }

            SetTheme();
        }

        private void RootNavigation_OnNavigatedForward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigated forward");
        }

        private void RootNavigation_OnNavigatedBackward(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigated backward");
        }

        private void SetTheme()
        {
            var windowHandle = new WindowInteropHelper(this).Handle;
            Background = Brushes.Transparent;
            WPFUI.Background.Manager.Apply(WPFUI.Background.BackgroundType.Auto, windowHandle);
        }
    }
}
