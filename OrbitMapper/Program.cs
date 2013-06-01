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
            // Create the instance of DebugForm to log information in the background. It only bogs down CPU a little bit if logRealtime 
            // is set to true, either way crash information will be displayed when an unhandled exception occurs.
            DebugForms debugger = new DebugForms();
            if(DebugForms.logRealtime)
                debugger.Show();
            try{
                Application.Run(new MainForms());
            }
            catch (Exception e)
            {
                EventSource.output(e.Message);
                EventSource.output(e.StackTrace);
                if(e.InnerException != null){
                    EventSource.output(e.InnerException.Message);
                    EventSource.output(e.InnerException.StackTrace);
                }
                if(!DebugForms.logRealtime)
                    debugger.ShowDialog();
            }
        }
    }
}
