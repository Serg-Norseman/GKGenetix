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
        [STAThread]
        static void Main()
        {
#if NETCORE
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
#else
            Application.EnableVisualStyles();
#endif
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GKMainFrm());
        }
    }
}
