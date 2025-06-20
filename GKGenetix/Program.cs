﻿/*
 *  "GKGenetix", the simple DNA analysis kit.
 *  Copyright (C) 2022-2025 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GKGenetix".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
