using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GenetixKit
{
    static class Program
    {

        public static GGKitFrmMain GGKitFrmMainInst = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GGKitFrmMainInst = new GGKitFrmMain();
            Application.Run(GGKitFrmMainInst);
        }
    }
}
