/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using GKGenetix.UI.Forms;

namespace GKGenetix
{
#if !NETCORE
    using System.Windows.Forms;
#else
    using Eto.Forms;
    using GKGenetix.UI;
#endif

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
#if !NETCORE
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GKMainFrm());
#else
            UIHelper.InitCommonStyles();
            var application = new Application();
            application.Run(new GKMainFrm());
#endif
        }
    }
}
