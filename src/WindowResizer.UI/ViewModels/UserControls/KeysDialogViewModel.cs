using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace WindowResizer.UI.ViewModels.UserControls;

public class KeysDialogViewModel : ObservableObject
{
    private readonly List<string> _hotKeys;

    public KeysDialogViewModel()
    {
        _hotKeys = new List<string>();
    }

    public KeysDialogViewModel(List<string> hotKeys)
    {
        _hotKeys = hotKeys;
    }

    public KeysVisualViewModel HotKeys => new(_hotKeys);
}
