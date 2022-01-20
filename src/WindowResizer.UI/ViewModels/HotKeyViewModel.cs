using Microsoft.Toolkit.Mvvm.ComponentModel;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;
using WindowResizer.UI.ViewModels.UserControls;

namespace WindowResizer.UI.ViewModels
{
    public class HotKeyViewModel : ObservableObject
    {
        private readonly Config _config;

        public HotKeyViewModel()
        {
            _config = ConfigLoader.Config;
        }

        private bool _disableInFullscreen;

        public KeysVisualViewModel SaveKeys => new(_config.SaveKey.ToKeys());

        public KeysDialogViewModel SaveKeysDialog => new(_config.SaveKey.ToKeys());

        public KeysVisualViewModel RestoreKeys => new(_config.RestoreKey.ToKeys());

        public KeysDialogViewModel RestoreKeysDialog => new(_config.RestoreKey.ToKeys());

        public KeysVisualViewModel RestoreAllKeys => new(_config.RestoreAllKey.ToKeys());

        public KeysDialogViewModel RestoreAllKeysDialog => new(_config.RestoreAllKey.ToKeys());

        public bool DisableInFullscreen
        {
            get
            {
                _disableInFullscreen = _config.DisableInFullScreen;
                return _disableInFullscreen;
            }
            set
            {
                SetProperty(ref _disableInFullscreen, value);
                _config.DisableInFullScreen = _disableInFullscreen;
                ConfigLoader.Save();
            }
        }
    }
}
