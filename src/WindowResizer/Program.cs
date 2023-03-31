using System;
using System.Threading;
using System.Windows.Forms;
using WindowResizer.Utils;

namespace WindowResizer
{
    static class Program
    {
        private const string AppGuid = "b3e7eb82-3db1-4f1b-9c3c-d67643ae0b00";

        [STAThread]
        static void Main()
        {
            Core.Dpi.Utils.SetDpiAwareness();

            using (var mutex = new Mutex(false, "Global\\" + AppGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show($"{App.Name} already running.", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.ThreadException += Application_ThreadException;
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                Application.Run(new TrayContext());
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ShowError(e.Exception.ToString());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowError(e.ExceptionObject.ToString());
        }

        private static void ShowError(string error)
        {
            Log.Append(error);
            MessageBox.Show("An error occurred, check the log file for more details.", App.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
