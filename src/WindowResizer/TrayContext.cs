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
using static WindowResizer.Base.WindowUtils;

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
                var roamingPath = Path.Combine(Helper.GeApplicationDataPath(), ConfigFile);
                var portablePath = Path.Combine(Application.StartupPath, ConfigFile);
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

            Log.SetLogPath(
                ConfigFactory.PortableMode
                    ? Application.StartupPath
                    : Helper.GeApplicationDataPath());

            RegisterHotkeys();
            _trayIcon = BuildTrayIcon();
            SetIconMode();
            SettingWindowInit();

            ProfilesEventsHandle();
            WindowsEventHandle();

            ToastRegister();

            SystemStartup();

            if (!App.IsRunningAsUwp && ConfigFactory.Current.CheckUpdate && !ConfigFactory.PortableMode)
            {
                _updater = new SquirrelUpdater(ConfirmUpdate, (message, tipsLevel, seconds) =>
                {
                    Toast.ShowToast(
                        title: $"{nameof(WindowResizer)} Update",
                        message: message,
                        actionLevel: (Toast.ActionLevel)(tipsLevel),
                        tray: _trayIcon,
                        expired: seconds);
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

        private static string BuildTrayToolTips()
        {
            return $"{nameof(WindowResizer)}\nv{Application.ProductVersion}\nProfile: {ConfigFactory.Current.ProfileName}";
        }

        private static void ToastRegister()
        {
            Toast.OnStart(action =>
            {
                switch (action)
                {
                    case Toast.ActionType.OpenProcessSetting:
                    {
                        _settingForm.Invoke((MethodInvoker)delegate
                        {
                            _settingForm.ShowFront();
                            _settingForm.SwitchTab("ProcessesPage");
                        });
                        break;
                    }
                }
            });
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
            var item = new ToolStripMenuItem("Settings", Resources.SettingIcon.ToBitmap(), OnSetting);
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

        private static readonly Padding ContextMenuPadding = new Padding(6, 6, 6, 6);

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
            Toast.OnStop();
            Environment.Exit(0);
        }

        private void OnSetting(object sender, EventArgs e)
        {
            if (_settingForm == null)
            {
                SettingWindowInit();
            }

            SetIconMode();

            _settingForm?.ShowFront();
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
                ResizeWindow(handle, ConfigFactory.Current, null, null, true);
            }
        }

        private static void OnProfileChange(string profileId)
        {
            ConfigFactory.ProfileSwitch(profileId);
        }

        private void ReloadConfig(string message)
        {
            _hook.UnRegisterHotKey();
            RegisterHotkeys();

            Toast.ShowToast(
                title: "Config Reloaded",
                message: message,
                actionLevel: Toast.ActionLevel.Success,
                tray: _trayIcon,
                expired: 2000);
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

        #region startup

        private void SystemStartup()
        {
            if (App.IsRunningAsUwp)
            {
                return;
            }

            if (Utils.SystemStartup.StartupStatus())
            {
                Utils.SystemStartup.AddToStartup();
            }
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
            catch (Exception e)
            {
                var error = $"Register hotkey {hotkeys.ToKeysString()} failed.";
                Log.Append($"{error}: {e}");
                Helper.ShowMessageBox(error);
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
                    ResizeAllWindow(ConfigFactory.Current, null);
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
                            UpdateOrSaveWindowSize(window, ConfigFactory.Current, null);
                        }
                    }

                    if (ConfigFactory.Current.NotifyOnSaved)
                    {
                        Toast.ShowToast(
                            title: "Config Saved",
                            message: "Current processes saved.",
                            tray: _trayIcon,
                            actionLevel: Toast.ActionLevel.Success,
                            actionType: Toast.ActionType.OpenProcessSetting);
                    }

                    return;
                }

                keys = GetKeys(HotkeysType.Save);
                if (keys.KeysEqual(e.Modifier, e.Key))
                {
                    var window = Resizer.GetForegroundHandle();
                    UpdateOrSaveWindowSize(window, ConfigFactory.Current, OnGetProcessFailed, s =>
                    {
                        if (ConfigFactory.Current.NotifyOnSaved)
                        {
                            Toast.ShowToast(
                                title: "Config Saved",
                                message: $"Process <{s}> saved!", tray: _trayIcon,
                                actionLevel: Toast.ActionLevel.Success,
                                actionType: Toast.ActionType.OpenProcessSetting);
                        }
                    });

                    return;
                }

                keys = GetKeys(HotkeysType.Restore);
                if (keys.KeysEqual(e.Modifier, e.Key))
                {
                    var window = Resizer.GetForegroundHandle();
                    ResizeWindow(window, ConfigFactory.Current, OnGetProcessFailed, OnConfigNoMatch);
                }
            }
            catch (Exception exception)
            {
                const string message = "An error occurred.\nCheck the log file for more details.";
                Toast.ShowToast(
                    title: "An Error Occured",
                    message: message,
                    actionLevel: Toast.ActionLevel.Error,
                    tray: _trayIcon,
                    expired: 2000);
                Log.Append($"Exception: {exception}");
            }
        }

        #endregion

        private void OnConfigNoMatch(string processName, string windowTitle)
        {
            Toast.ShowToast(
                title: "No Operations Available",
                message: $"No saved settings for <{processName} :: {windowTitle}>.",
                actionLevel: Toast.ActionLevel.Info, tray: _trayIcon,
                expired: 2000);
        }

        private void OnGetProcessFailed(Process process, Exception e)
        {
            var message =
                $"Unable to resize process <{process.ProcessName}>, elevated privileges may be required.";
            Toast.ShowToast(
                title: "Operation Failed",
                message: message,
                actionLevel: Toast.ActionLevel.Warning,
                tray: _trayIcon,
                expired: 2000);
            Log.Append($"{message}\nException: {e}");
        }

        private void SettingWindowInit()
        {
            _settingForm = new SettingForm(_hook);

            _settingForm.Show();
            _settingForm.Hide();

            _settingForm.ConfigReload += ReloadConfig;
        }

        private void SetIconMode()
        {
            var darkMode = Core.Theme.ThemeDetect.IsDarkModeEnable();
            _trayIcon.Icon = darkMode ? Resources.AppIcon_light : Resources.AppIcon;
        }
    }
}
