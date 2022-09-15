using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WindowResizer.Common.Windows;
using WindowResizer.Configuration;
using WindowResizer.Core.Shortcuts;
using WindowResizer.Core.WindowControl;
using WindowResizer.Utils;
using static WindowResizer.Utils.WindowUtils;

namespace WindowResizer
{
    public class TrayContext : ApplicationContext
    {
        private static SettingForm _settingForm;
        private readonly NotifyIcon _trayIcon;
        private readonly SquirrelUpdater _updater;

        private readonly KeyboardHook _hook = new KeyboardHook();
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

            RegisterHotkeys();
            _trayIcon = BuildTrayIcon();
            SettingWindowInit();

            ProfilesEventsHandle();
            WindowsEventHandle();

            if (ConfigFactory.Current.CheckUpdate)
            {
                _updater = new SquirrelUpdater(ConfirmUpdate, (message, tipIcon, seconds) =>
                {
                    ShowTooltips(message, (ToolTipIcon)tipIcon, seconds);
                });
                Update();
            }
        }

        #region tray

        private NotifyIcon BuildTrayIcon()
        {
            ContextMenu.Items.Clear();
            ContextMenu.RenderMode = ToolStripRenderMode.System;
            ContextMenu.BackColor = SystemColors.Window;
            ContextMenu.ForeColor = SystemColors.ControlText;
            ContextMenu.ShowCheckMargin = false;
            ContextMenu.ShowImageMargin = true;

            ContextMenu.Font = Helper.ChangeFontSize(ContextMenu.Font, 10F);
            var imageSize = (int)Math.Round(ContextMenu.Font.Height * 0.9);
            ContextMenu.ImageScalingSize = new Size(imageSize, imageSize);
            BuildContextMenu();

            var trayIcon = new NotifyIcon
            {
                Icon = Resources.AppIcon, Visible = true, ContextMenuStrip = ContextMenu, Text = BuildTrayToolTips(),
            };

            trayIcon.DoubleClick += OnSetting;
            return trayIcon;
        }

        private string BuildTrayToolTips()
        {
            return $"{nameof(WindowResizer)}\nv{Application.ProductVersion}\nProfile: {ConfigFactory.Current.ProfileName}";
        }

        private static readonly ContextMenuStrip ContextMenu = new ContextMenuStrip();

        private void BuildContextMenu()
        {
            ContextMenu.Items.Clear();

            foreach (var c in ConfigFactory.Profiles.Configs)
            {
                var isCurrent = c.ProfileId.Equals(ConfigFactory.Current.ProfileId, StringComparison.Ordinal);
                var image = isCurrent ? Resources.CheckIcon : null;
                var m = new ToolStripMenuItem(c.ProfileName, image?.ToBitmap(),
                    (s, e) => OnProfileChange(c.ProfileId));
                SetMenuStyle(m);
                ContextMenu.Items.Add(m);
            }

            ContextMenu.Items.Add(new ToolStripSeparator());
            var item = new ToolStripMenuItem("Setting", Resources.SettingIcon.ToBitmap(), OnSetting);
            SetMenuStyle(item);
            ContextMenu.Items.Add(item);
            item = new ToolStripMenuItem("Exit", Resources.ExitIcon.ToBitmap(), OnExit);
            SetMenuStyle(item);
            ContextMenu.Items.Add(item);
        }

        private static void SetMenuStyle(ToolStripItem m)
        {
            m.Margin = ContextMenuPadding;
            m.MouseEnter += (s, e) => ((ToolStripItem)s).ForeColor = Color.White;
            m.MouseLeave += (s, e) => ((ToolStripItem)s).ForeColor = SystemColors.ControlText;
        }

        private static readonly Padding ContextMenuPadding= new Padding(6, 6, 6, 6);

        private void RebuildContextMenu()
        {
            _trayIcon.Text = BuildTrayToolTips();
            BuildContextMenu();
        }

        private void OnExit(object sender, EventArgs e)
        {
            _settingForm?.Close();
            _windowEventHandler?.Dispose();
            _trayIcon?.Dispose();
            _hook?.Dispose();
            Environment.Exit(0);
        }

        private void OnSetting(object sender, EventArgs e)
        {
            if (_settingForm == null)
            {
                SettingWindowInit();
            }

            _settingForm?.Show();
        }

        #endregion

        #region events

        private void ProfilesEventsHandle()
        {
            ConfigFactory.Profiles.ProfileEvents.ProfileAdd += (i, n) => RebuildContextMenu();
            ConfigFactory.Profiles.ProfileEvents.ProfileSwitch += i => RebuildContextMenu();
            ConfigFactory.Profiles.ProfileEvents.ProfileRename += (i, n) => RebuildContextMenu();
            ConfigFactory.Profiles.ProfileEvents.ProfileRemove += i => RebuildContextMenu();
            ConfigFactory.Current.WindowSizes.ListChanged += (s, e) => WindowsEventHandle();
        }

        private void WindowsEventHandle()
        {
            var autoSizeEnable = ConfigFactory.Current.WindowSizes.Any(i => i.AutoResize);
            if (_windowEventHandler == null && autoSizeEnable)
            {
                _windowEventHandler = new WindowEventHandler(OnWindowCreated);
                _windowEventHandler.AddWindowCreateHandle();
            }
            else if (_windowEventHandler != null && !autoSizeEnable)
            {
                _windowEventHandler?.Dispose();
                _windowEventHandler = null;
            }
        }

        private void OnWindowCreated(IntPtr handle)
        {
            if (Resizer.IsWindowVisible(handle))
            {
                ResizeWindow(handle, false, true);
            }
        }

        private void OnProfileChange(string profileId)
        {
            ConfigFactory.ProfileSwitch(profileId);
        }

        private void ReloadConfig(string message)
        {
            _hook.UnRegisterHotKey();
            RegisterHotkeys();
            ShowTooltips(message, ToolTipIcon.Info, 2000);
        }

        #endregion

        #region update

        private void Update()
        {
            _ = _updater.Update();
        }

        private static bool ConfirmUpdate(string message)
        {
            var res = MessageBox.Show(message, $"{nameof(WindowResizer)} Update",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            return res == DialogResult.OK;
        }

        #endregion

        #region hotkeys

        private void RegisterHotkeys()
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

        #endregion

        #region window resize

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

            var place = Resizer.GetPlacement(handle);

            // workaround: use GetWindowRect to get actual window coordinates
            place.Rect = place.WindowState == WindowState.Maximized ? place.Rect : Resizer.GetRect(handle);
            UpdateOrSaveConfig(match, processName, windowTitle, place);
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

        #endregion

        private void SettingWindowInit()
        {
            _settingForm = new SettingForm(_hook);
            _settingForm.ConfigReload += ReloadConfig;
        }

        private void ShowTooltips(string message, ToolTipIcon tipIcon, int mSeconds) =>
            _trayIcon.ShowBalloonTip(mSeconds, nameof(WindowResizer), message, tipIcon);
    }
}
