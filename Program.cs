/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

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
