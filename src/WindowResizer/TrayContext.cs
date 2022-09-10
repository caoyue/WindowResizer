using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Common.Shortcuts;
using WindowResizer.Common.Windows;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;
using WindowResizer.Core.WindowControl;
using WindowResizer.Utils;

namespace WindowResizer
{
    public class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private readonly KeyboardHook _hook = new KeyboardHook();
        private readonly SquirrelUpdater _updater;

        private static SettingForm _settingForm;

        private static WindowEventHandler _windowEventHandler;

        private static readonly string ConfigFile = $"{nameof(WindowResizer)}.config.json";

        public TrayContext()
        {
            try
            {
                var roamingPath = Path.Combine(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(WindowResizer)),
                    ConfigFile);
                var portablePath = Path.Combine(
                    Application.StartupPath, ConfigFile);
                ConfigFactory.SetPath(roamingPath, portablePath);
                ConfigFactory.Load();
            }
            catch (Exception e)
            {
                var message = $"Config file {ConfigFactory.ConfigPath} load failed, use default configs.";
                Log.Append($"{message}\nException: {e}");
                Helper.ShowMessageBox(message, MessageBoxIcon.Warning);

                ConfigFactory.UseDefault();
                ConfigFactory.Save();
            }

            RegisterHotkey();

            _trayIcon = BuildTrayIcon();

            EventsHandle();

            _windowEventHandler = new WindowEventHandler(OnWindowCreated);
            _windowEventHandler.AddWindowCreateHandle();

            if (ConfigFactory.Current.CheckUpdate)
            {
                _updater = new SquirrelUpdater(ConfirmUpdate, (message, tipIcon, seconds) =>
                {
                    ShowTooltips(message, (ToolTipIcon)tipIcon, seconds);
                });
                Update();
            }
        }

        private NotifyIcon BuildTrayIcon()
        {
            var trayIcon = new NotifyIcon
            {
                Icon = Resources.AppIcon,
                ContextMenuStrip = BuildContextMenu(),
                Visible = true,
                Text = nameof(WindowResizer)
            };

            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    _trayIcon.ContextMenuStrip = BuildContextMenu();
                }
            };

            trayIcon.DoubleClick += OnSetting;
            return trayIcon;
        }

        private ContextMenuStrip BuildContextMenu()
        {
            var menu = new ContextMenuStrip();
            foreach (var c in ConfigFactory.Profiles.Configs)
            {
                var isCurrent = c.ProfileId.Equals(ConfigFactory.Current.ProfileId, StringComparison.Ordinal);
                var image = isCurrent ? Resources.CheckIcon : null;
                menu.Items.Add(new ToolStripMenuItem(c.ProfileName, image?.ToBitmap(),
                    (s, e) => OnProfileChange(c.ProfileId)));
            }

            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(new ToolStripMenuItem("Setting", Resources.SettingIcon.ToBitmap(), OnSetting));
            menu.Items.Add(new ToolStripMenuItem("Exit", Resources.ExitIcon.ToBitmap(), OnExit));
            return menu;
        }

        private void EventsHandle()
        {
            ConfigFactory.Profiles.ProfileEvents.ProfileAdd += (i, n) => RebuildContextMenu();
            ConfigFactory.Profiles.ProfileEvents.ProfileSwitch += i => RebuildContextMenu();
            ConfigFactory.Profiles.ProfileEvents.ProfileRename += (i, n) => RebuildContextMenu();
            ConfigFactory.Profiles.ProfileEvents.ProfileRemove += i => RebuildContextMenu();
        }

        private void RebuildContextMenu()
        {
            _trayIcon.ContextMenuStrip = BuildContextMenu();
        }

        private void OnProfileChange(string profileId)
        {
            ConfigFactory.ProfileSwitch(profileId);
        }

        private void Update()
        {
            _ = _updater.Update();
        }

        private static bool ConfirmUpdate(string message)
        {
            var res = MessageBox.Show(message, "WindowResizer Update",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            return res == DialogResult.OK;
        }

        private void OnExit(object sender, EventArgs e)
        {
            _settingForm?.Close();
            _windowEventHandler.RemoveWindowCreateHandle();
            ConfigFactory.Save();
            _trayIcon.Dispose();
            _hook.Dispose();
            Environment.Exit(0);
        }

        private void OnSetting(object sender, EventArgs e)
        {
            if (_settingForm == null)
            {
                _settingForm = new SettingForm(_hook);
                _settingForm.ConfigReload += ReloadConfig;
            }

            _settingForm.Show();
        }

        private void RegisterHotkey()
        {
            foreach (var type in Enum.GetValues(typeof(HotkeysType)).Cast<HotkeysType>())
            {
                RegisterHotkey(type);
            }

            _hook.KeyPressed += OnKeyPressed;
        }

        private void RegisterHotkey(HotkeysType type)
        {
            var hotkeys = GetKeys(type);
            if (hotkeys is null)
            {
                return;
            }

            if (!hotkeys.IsValid())
            {
                Helper.ShowMessageBox($"{type.ToString()} window hotkeys not valid.");
            }

            try
            {
                var id = _hook.RegisterHotKey(hotkeys.GetModifierKeys(), hotkeys.GetKey());
                App.RegisteredHotKeys[type] = id;
            }
            catch (Exception)
            {
                Helper.ShowMessageBox($"Register hotkey {hotkeys.ToKeysString()} failed.");
            }
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            try
            {
                if (ConfigFactory.Current.DisableInFullScreen && Resizer.IsForegroundFullScreen())
                {
                    return;
                }

                var keys = GetKeys(HotkeysType.RestoreAll);
                if (keys.KeysEqual(e.Modifier, e.Key))
                {
                    var windows = Resizer.GetOpenWindows();
                    windows.Reverse();
                    foreach (var window in windows)
                    {
                        if (Resizer.GetWindowState(window) != WindowState.Minimized)
                        {
                            ResizeWindow(window);
                        }
                    }

                    return;
                }

                keys = GetKeys(HotkeysType.SaveAll);
                if (keys.KeysEqual(e.Modifier, e.Key))
                {
                    var windows = Resizer.GetOpenWindows();
                    foreach (var window in windows)
                    {
                        if (Resizer.GetWindowState(window) != WindowState.Minimized)
                        {
                            UpdateOrSaveWindowSize(window);
                        }
                    }

                    return;
                }

                keys = GetKeys(HotkeysType.Save);
                if (keys.KeysEqual(e.Modifier, e.Key))
                {
                    var window = Resizer.GetForegroundHandle();
                    UpdateOrSaveWindowSize(window, true);
                    return;
                }

                keys = GetKeys(HotkeysType.Restore);
                if (keys.KeysEqual(e.Modifier, e.Key))
                {
                    var window = Resizer.GetForegroundHandle();
                    ResizeWindow(window, true);
                }
            }
            catch (Exception exception)
            {
                var message = "An error occurred.\nCheck the log file for more details.";
                ShowTooltips(message, ToolTipIcon.Error, 2000);
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

        private void ResizeWindow(IntPtr handle, bool showTips = false, bool onlyAuto = false)
        {
            if (!IsProcessAvailable(handle, out string processName, showTips))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(processName)) return;

            var windowTitle = Resizer.GetWindowTitle(handle) ?? string.Empty;
            var match = GetMatchWindowSize(ConfigFactory.Current.WindowSizes, processName, windowTitle, onlyAuto);
            if (!match.NoMatch)
            {
                MoveMatchWindow(match, handle);
            }
            else
            {
                if (showTips)
                {
                    ShowTooltips($"No saved settings for <{processName} :: {windowTitle}>.", ToolTipIcon.Info, 2000);
                }
            }
        }

        private void UpdateOrSaveWindowSize(IntPtr handle, bool showTips = false)
        {
            if (!IsProcessAvailable(handle, out string processName, showTips))
            {
                return;
            }

            var windowTitle = Resizer.GetWindowTitle(handle);
            var match = GetMatchWindowSize(ConfigFactory.Current.WindowSizes, processName, windowTitle);
            var state = Resizer.GetWindowState(handle);
            UpdateOrSaveConfig(match, processName, windowTitle, Resizer.GetRect(handle), state);
        }

        private bool IsProcessAvailable(IntPtr handle, out string processName, bool showTips = false)
        {
            processName = string.Empty;
            if (Resizer.IsChildWindow(handle))
            {
                return false;
            }

            var process = Resizer.GetRealProcess(handle);
            if (process is null || !TryGetProcessName(process, out processName, showTips))
            {
                return false;
            }

            return !Resizer.IsInvisibleProcess(processName);
        }

        private bool TryGetProcessName(Process process, out string processName, bool showTips = true)
        {
            try
            {
                processName = process.MainModule?.ModuleName;
                return true;
            }
            catch (Exception e)
            {
                if (showTips)
                {
                    var message =
                        $"Unable to resize process <{process.ProcessName}>, elevated privileges may be required.";
                    ShowTooltips(message, ToolTipIcon.Warning, 1500);
                    Log.Append($"{message}\nException: {e}");
                }

                processName = null;
                return false;
            }
        }

        private void ReloadConfig(string message)
        {
            _hook.UnRegisterHotKey();
            RegisterHotkey();
            ShowTooltips(message, ToolTipIcon.Info, 2000);
        }

        private void ShowTooltips(string message, ToolTipIcon tipIcon, int mSeconds) =>
            _trayIcon.ShowBalloonTip(mSeconds, nameof(WindowResizer), message, tipIcon);

        #region window resize

        private static MatchWindowSize GetMatchWindowSize(
            BindingList<WindowSize> windowSizes,
            string processName,
            string title,
            bool onlyAuto = false)
        {
            var windows = windowSizes.Where(w =>
                    w.Name.Equals(processName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (onlyAuto)
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
                InsertOrder(new WindowSize { Name = processName, Title = "*", Rect = rect, State = state });
                if (!string.IsNullOrWhiteSpace(title))
                {
                    InsertOrder(new WindowSize { Name = processName, Title = title, Rect = rect, State = state });
                }

                ConfigFactory.Save();
                return;
            }

            if (match.FullMatch != null)
            {
                match.FullMatch.Rect = rect;
                match.FullMatch.State = state;
            }
            else if (!string.IsNullOrWhiteSpace(title))
            {
                InsertOrder(new WindowSize { Name = processName, Title = title, Rect = rect, State = state });
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
                InsertOrder(new WindowSize { Name = processName, Title = "*", Rect = rect, State = state });
            }

            ConfigFactory.Save();
        }

        private static void InsertOrder(WindowSize item)
        {
            var list = ConfigFactory.Current.WindowSizes;
            var backing = list.ToList();
            backing.Add(item);
            var index = backing.OrderBy(l => l.Name).ThenBy(l => l.Title).ToList().IndexOf(item);
            list.Insert(index, item);
        }


        private static Hotkeys GetKeys(HotkeysType type) =>
            ConfigFactory.Current.GetKeys(type);

        #endregion
    }
}
