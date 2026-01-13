/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GKGenetix.Core;
using GKGenetix.Core.Database;

namespace GKGenetix.UI
{
    public class GKWidget : UserControl
    {
        protected IKitHost _host;

        public event EventHandler Closing;

        protected GKWidget(IKitHost host)
        {
            _host = host;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                Closing?.Invoke(this, EventArgs.Empty);
            }
            base.Dispose(disposing);
        }

        public virtual void SetKit(IList<TestRecord> selectedKits)
        {
        }
    }
}
