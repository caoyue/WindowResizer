using System.Windows;
using WindowResizer.UI.Controls;
using WindowResizer.UI.ViewModels;
using WPFUI.Controls;

namespace WindowResizer.UI.Views
{
    public partial class HotKeysPage
    {
        private HotKeyViewModel _dataContext;

        public HotKeysPage()
        {
            InitializeComponent();
            _dataContext = new HotKeyViewModel();
            DataContext = _dataContext;
        }

        private void ShowKeysDialog(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            var c = new KeysDialogControl();
            var dialog = (((ContainerWindow)Application.Current.MainWindow)!).RootDialog;
            c.DataContext = button.Tag switch
            {
                "save" => _dataContext.SaveKeysDialog,
                "restore" => _dataContext.RestoreKeysDialog,
                "restoreAll" => _dataContext.RestoreAllKeysDialog,
                _ => c.DataContext
            };
            dialog.Content = c;
            dialog.Show = true;
        }
    }
}
