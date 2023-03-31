using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WindowResizer.Utils
{
    internal static class Toast
    {
        private const string ActionKey = "Action";

        public static void ShowToast(
            string message,
            string title = "",
            int expired = 3000,
            NotifyIcon tray = null,
            ActionLevel actionLevel = ActionLevel.Info,
            ActionType actionType = ActionType.None
        )
        {
            if (Helper.IsWindows10Greater())
            {
                Send(title, message, actionLevel, actionType, expired);
            }
            else if (tray != null)
            {
                var icon = (ToolTipIcon)((int)actionLevel);
                tray.ShowBalloonTip(expired, title, message, icon);
            }
        }

        public static void Send(string title, string content, ActionLevel actionLevel, ActionType actionType, int expired = 3000)
        {
            var builder =
                new ToastContentBuilder()
                    .SetToastDuration(ToastDuration.Short)
                    .SetToastScenario(ToastScenario.Default)
                    .AddText(title)
                    .AddText(content);

            switch (actionType)
            {
                case ActionType.OpenProcessSetting:
                {
                    builder.AddButton(new ToastButton()
                                      .SetContent("Open Settings")
                                      .AddArgument(ActionKey, actionType)
                                      .SetBackgroundActivation());
                    break;
                }
            }

            string imageUri;
            switch (actionLevel)
            {
                case ActionLevel.Info:
                case ActionLevel.Warning:
                case ActionLevel.Error:
                {
                    imageUri = GetDialogImageUri(actionLevel.ToString().ToLower());
                    break;
                }

                default:
                {
                    imageUri = GetDialogImageUri("success");
                    break;
                }
            }

            builder
                .AddAppLogoOverride(new Uri(imageUri), ToastGenericAppLogoCrop.Circle)
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMilliseconds(expired);
                });
        }

        private static string GetDialogImageUri(string name) =>
            Path.GetFullPath($@"Resources\dialog\{name}.png");

        private static bool _toastRegistered;

        public static void OnStart(Action<ActionType> action)
        {
            if (_toastRegistered)
            {
                return;
            }

            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                var a = args.FirstOrDefault(i => i.Key.Equals(ActionKey));
                if (!string.IsNullOrEmpty(a.Value))
                {
                    action?.Invoke((ActionType)int.Parse(a.Value));
                }
            };
            _toastRegistered = true;
        }

        public static void OnStop()
        {
            ToastNotificationManagerCompat.Uninstall();
        }

        public static void Clear()
        {
            ToastNotificationManagerCompat.History.Clear();
        }

        public enum ActionType
        {
            None,
            OpenProcessSetting,
        }

        public enum ActionLevel
        {
            Success,
            Info,
            Warning,
            Error,
        }
    }
}
