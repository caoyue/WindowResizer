using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WindowResizer.UI.ViewModels.UserControls;

public class KeysVisualViewModel : ObservableObject
{
    private List<string> _hotKeys;

    public KeysVisualViewModel()
    {
        _hotKeys = new List<string>();
    }

    public KeysVisualViewModel(List<string> hotKeys)
    {
        _hotKeys = hotKeys;
    }

    public List<string> HotKeys
    {
        get => _hotKeys;
        set => SetProperty(ref _hotKeys, value);
    }
}
