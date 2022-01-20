using WindowResizer.UI.ViewModels;

namespace WindowResizer.UI.Views
{
    public partial class AboutPage
    {
        private readonly AboutViewModel _dataContext;

        public AboutPage()
        {
            InitializeComponent();
            _dataContext = new AboutViewModel();
            this.DataContext = _dataContext;
        }
    }
}
