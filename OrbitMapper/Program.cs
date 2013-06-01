using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace OrbitMapper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try{
                Application.Run(new MainForms());
            }
            catch (Exception e)
            {
                EventSource.output(e.Message);
                EventSource.output(e.StackTrace);
                EventSource.output(e.InnerException.Message);
                EventSource.output(e.InnerException.StackTrace);
                new DebugForms().ShowDialog();
            }
        }
    }
}
