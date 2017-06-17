using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowResizer
{
    static class Program
    {
        private const string AppGuid = "b3e7eb82-3db1-4f1b-9c3c-d67643ae0b00";

        [STAThread]
        static void Main()
        {
            using (var mutex = new Mutex(false, "Global\\" + AppGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("WindowResizer already running");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TrayContext());
            }

        }
    }
}
