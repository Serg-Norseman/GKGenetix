using System;
using System.Windows.Forms;
using GenetixKit.Forms;

namespace GenetixKit
{
    static class Program
    {
        public static GKMainFrm GGKitFrmMainInst = null;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GGKitFrmMainInst = new GKMainFrm();
            Application.Run(GGKitFrmMainInst);
        }
    }
}
