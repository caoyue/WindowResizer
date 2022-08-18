using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;
using WindowResizer.Core.WindowControl;

namespace WindowResizer
{
    public class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private readonly KeyboardHook _hook = new KeyboardHook();
        private readonly SquirrelUpdater _updater;

        private static SettingForm _settingForm;

        private static WindowEventHandler _windowEventHandler;

        private const string ConfigFile = "WindowResizer.config.json";

        public TrayContext()
        {
            try
            {
                var roamingPath = Path.Combine(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(WindowResizer)), ConfigFile);
                var portablePath = Path.Combine(
                    Application.StartupPath, ConfigFile);
                ConfigLoader.SetPath(roamingPath, portablePath);
                ConfigLoader.Load();
            }
            catch (Exception e)
            {
                var message = $"Config file load failed.\nPath: {ConfigLoader.ConfigPath}";
                Log.Append($"{message}\nException: {e}");
                ShowMessageBox(message);
            }

            try
            {
                RegisterHotkey();
            }
            catch (Exception e)
            {
                var message = "Register hotkey failed.";
                Log.Append($"{message}\nException: {e}");
                ShowMessageBox(message);
            }

            _trayIcon = new NotifyIcon
            {
                Icon = Resources.AppIcon,
                ContextMenu =
                    new ContextMenu(new[]
                    {
                        new MenuItem("Setting", OnSetting),
                        new MenuItem("Exit", OnExit),
                    }),
                Visible = true,
                Text = nameof(WindowResizer)
            };

            _trayIcon.DoubleClick += OnSetting;

            _windowEventHandler = new WindowEventHandler(OnWindowCreated);
            _windowEventHandler.AddWindowCreateHandle();

            _updater = new SquirrelUpdater(ConfirmUpdate, ShowTooltips);
            Update();
        }

        private void Update()
        {
            _ = _updater.Update();
        }

        private bool ConfirmUpdate(string message)
        {
            var res = MessageBox.Show(message, "WindowResizer Update",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            return res == DialogResult.OK;
        }

        private void OnExit(object sender, EventArgs e)
        {
            _settingForm?.Close();
            _windowEventHandler.RemoveWindowCreateHandle();
            ConfigLoader.Save();
            _trayIcon.Dispose();
            _hook.Dispose();
            Environment.Exit(0);
        }

        private void OnSetting(object sender, EventArgs e)
        {
            if (_settingForm == null)
            {
                _settingForm = new SettingForm(_hook);
            }

            _settingForm.Show();
        }

        private void RegisterHotkey()
        {
            if (!ConfigLoader.Config.SaveKey.ValidateKeys())
            {
                ShowMessageBox("Save window hotkeys not valid.");
            }

            if (!ConfigLoader.Config.RestoreKey.ValidateKeys())
            {
                ShowMessageBox("Restore window hotkeys not valid.");
            }

            _hook.RegisterHotKey(ConfigLoader.Config.SaveKey.GetModifierKeys(), ConfigLoader.Config.SaveKey.GetKey());
            _hook.RegisterHotKey(ConfigLoader.Config.RestoreKey.GetModifierKeys(),
                ConfigLoader.Config.RestoreKey.GetKey());
            _hook.RegisterHotKey(ConfigLoader.Config.RestoreAllKey.GetModifierKeys(),
                ConfigLoader.Config.RestoreAllKey.GetKey());
            _hook.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            try
            {
                if (ConfigLoader.Config.DisableInFullScreen && Resizer.IsForegroundFullScreen())
                {
                    return;
                }

                if (e.Modifier == ConfigLoader.Config.RestoreAllKey.GetModifierKeys()
                    && e.Key == ConfigLoader.Config.RestoreAllKey.GetKey())
                {
                    var windows = Resizer.GetOpenWindows();
                    windows.Reverse();
                    foreach (var window in windows)
                    {
                        ResizeWindow(window);
                    }
                }
                else
                {
                    var handle = Resizer.GetForegroundHandle();

                    if (e.Modifier == ConfigLoader.Config.SaveKey.GetModifierKeys() &&
                        e.Key == ConfigLoader.Config.SaveKey.GetKey())
                    {
                        UpdateOrSaveWindowSize(handle);
                    }
                    else if (e.Modifier == ConfigLoader.Config.RestoreKey.GetModifierKeys() &&
                             e.Key == ConfigLoader.Config.RestoreKey.GetKey())
                    {
                        ResizeWindow(handle, true);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = "An error occurred, check the log file for more details.";
                ShowTooltips(message, 2, 2000);
                Log.Append($"Exception: {exception}");
            }
        }

        private void OnWindowCreated(IntPtr handle)
        {
            if (Resizer.IsWindowVisible(handle))
            {
                ResizeWindow(handle, false, true);
            }
        }

        private void ResizeWindow(IntPtr handle, bool tips = false, bool auto = false)
        {
            if (Resizer.IsChildWindow(handle))
            {
                return;
            }

            var process = Resizer.GetRealProcess(handle);
            if (process == null || !TryGetProcessName(process, out string processName, tips))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(processName)) return;

            var title = process.MainWindowTitle;
            var match = GetMatchWindowSize(ConfigLoader.Config.WindowSizes, processName, title, auto);
            if (!match.NoMatch)
            {
                MoveMatchWindow(match, handle);
            }
            else
            {
                if (tips)
                {
                    var titleStr = string.IsNullOrWhiteSpace(title) ? "" : $"({title})";
                    ShowTooltips($"No saved settings for <{processName}>{titleStr}.", 1, 2000);
                }
            }
        }

        private void UpdateOrSaveWindowSize(IntPtr handle)
        {
            var process = Resizer.GetRealProcess(handle);
            if (process is null || !TryGetProcessName(process, out string processName))
            {
                return;
            }

            var title = process.MainWindowTitle;
            var match = GetMatchWindowSize(ConfigLoader.Config.WindowSizes, processName, title);
            var state = Resizer.GetWindowState(handle);
            UpdateOrSaveConfig(match, processName, title, Resizer.GetRect(handle), state);
        }

        private bool TryGetProcessName(Process process, out string processName, bool tips = true)
        {
            try
            {
                processName = process.MainModule?.ModuleName;
                return true;
            }
            catch (Exception e)
            {
                if (tips)
                {
                    var message = $"Unable to resize process <{process.ProcessName}>, elevated privileges may be required.";
                    ShowTooltips(message, 2, 1500);
                    Log.Append($"{message}\nException: {e}");
                }

                processName = null;
                return false;
            }
        }

        private static MatchWindowSize GetMatchWindowSize(
            BindingList<WindowSize> windowSizes,
            string processName,
            string title,
            bool auto = false)
        {
            var windows = windowSizes.Where(w =>
                                         w.Name.Equals(processName, StringComparison.OrdinalIgnoreCase))
                                     .ToList();

            if (auto)
            {
                windows = windows.Where(w => w.AutoResize).ToList();
            }

            return new MatchWindowSize
            {
                FullMatch = windows.FirstOrDefault(w => w.Title == title),
                PrefixMatch = windows.FirstOrDefault(w =>
                    w.Title.StartsWith("*") && w.Title.Length > 1 && title.EndsWith(w.Title.TrimStart('*'))),
                SuffixMatch = windows.FirstOrDefault(w =>
                    w.Title.EndsWith("*") && w.Title.Length > 1 && title.StartsWith(w.Title.TrimEnd('*'))),
                WildcardMatch = windows.FirstOrDefault(w => w.Title.Equals("*"))
            };
        }

        private static void MoveMatchWindow(MatchWindowSize match, IntPtr handle)
        {
            if (match.FullMatch != null)
            {
                MoveWindow(handle, match.FullMatch);
                return;
            }

            if (match.PrefixMatch != null)
            {
                MoveWindow(handle, match.PrefixMatch);
                return;
            }

            if (match.SuffixMatch != null)
            {
                MoveWindow(handle, match.SuffixMatch);
                return;
            }

            if (match.WildcardMatch != null)
            {
                MoveWindow(handle, match.WildcardMatch);
            }
        }

        private static void MoveWindow(IntPtr handle, WindowSize match)
        {
            if (match.State == WindowState.Maximized)
            {
                Resizer.MaximizeWindow(handle);
            }
            else
            {
                Resizer.MoveWindow(handle, match.Rect);
            }
        }

        private static void UpdateOrSaveConfig(MatchWindowSize match, string processName, string title, Rect rect,
            WindowState state = WindowState.Normal)
        {
            if (string.IsNullOrWhiteSpace(processName)) return;

            if (match.NoMatch)
            {
                // Add a wildcard match for all titles
                InsertOrder(new WindowSize
                {
                    Name = processName, Title = "*", Rect = rect, State = state
                });
                if (!string.IsNullOrWhiteSpace(title))
                {
                    InsertOrder(new WindowSize
                    {
                        Name = processName, Title = title, Rect = rect, State = state
                    });
                }

                ConfigLoader.Save();
                return;
            }

            if (match.FullMatch != null)
            {
                match.FullMatch.Rect = rect;
                match.FullMatch.State = state;
            }
            else if (!string.IsNullOrWhiteSpace(title))
            {
                InsertOrder(new WindowSize
                {
                    Name = processName, Title = title, Rect = rect, State = state
                });
            }

            if (match.SuffixMatch != null)
            {
                match.SuffixMatch.Rect = rect;
                match.SuffixMatch.State = state;
            }

            if (match.PrefixMatch != null)
            {
                match.PrefixMatch.Rect = rect;
                match.PrefixMatch.State = state;
            }

            if (match.WildcardMatch != null)
            {
                match.WildcardMatch.Rect = rect;
                match.WildcardMatch.State = state;
            }
            else
            {
                InsertOrder(new WindowSize
                {
                    Name = processName, Title = "*", Rect = rect, State = state
                });
            }

            ConfigLoader.Save();
        }

        private static void InsertOrder(WindowSize item)
        {
            var list = ConfigLoader.Config.WindowSizes;
            var backing = list.ToList();
            backing.Add(item);
            var index = backing.OrderBy(l => l.Name).ThenBy(l => l.Title).ToList().IndexOf(item);
            list.Insert(index, item);
        }

        private static void ShowMessageBox(string message, string title = nameof(WindowResizer), MessageBoxIcon icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        private void ShowTooltips(string message, int tipIcon, int mSeconds) =>
            _trayIcon.ShowBalloonTip(mSeconds, nameof(WindowResizer), message, (ToolTipIcon)tipIcon);
    }
}
