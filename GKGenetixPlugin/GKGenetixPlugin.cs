/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2022-2023 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
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
using System.Reflection;
using GKCore;
using GKCore.Design.Graphics;
using GKCore.Interfaces;
using GKCore.Plugins;
using GKGenetix.UI;

[assembly: AssemblyTitle("GKGenetixPlugin")]
[assembly: AssemblyDescription("GEDKeeper Genetix plugin")]
[assembly: AssemblyProduct("GEDKeeper")]
[assembly: AssemblyCopyright("Copyright © 2022-2023 by Sergey V. Zhdanovskih")]
[assembly: AssemblyVersion("0.2.0.0")]
[assembly: AssemblyCulture("")]

namespace GKGenetixPlugin
{
    public enum CLS
    {
        Title = 1,
        DNAAnalysis,
        DNAInheritanceTest,
    }


    public abstract class GenetixPlugin : OrdinaryPlugin
    {
        private IImage fIcon;

        public override IImage Icon { get { return fIcon; } }
        public override PluginCategory Category { get { return PluginCategory.Common; } }

        public override bool Startup(IHost host)
        {
            bool result = base.Startup(host);
            try {
                fIcon = AppHost.GfxProvider.LoadResourceImage(this.GetType(), "GKGenetixPlugin.Resources.GKGenetix.png");
            } catch (Exception ex) {
                Logger.WriteError("GenetixPlugin.Startup()", ex);
                result = false;
            }
            return result;
        }
    }


    public class DNAAnalysisPlugin : GenetixPlugin
    {
        private string fDisplayName = "DNAAnalysisPlugin";
        private ILangMan fLangMan;

        public override string DisplayName { get { return fDisplayName; } }
        public override ILangMan LangMan { get { return fLangMan; } }

        private DNAAnalysis fForm;

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                CloseForm();
            }
            base.Dispose(disposing);
        }

        internal void CloseForm()
        {
            if (fForm != null) {
                fForm.Close();
                fForm = null;
            }
        }

        public override void Execute()
        {
            if (fForm == null) {
                fForm = new DNAAnalysis();
                fForm.Show();
            } else {
                CloseForm();
            }
        }

        public override void OnLanguageChange()
        {
            try {
                fLangMan = Host.CreateLangMan(this);
                fDisplayName = fLangMan.LS(CLS.DNAAnalysis);

                //if (fForm != null) fForm.SetLocale();
            } catch (Exception ex) {
                Logger.WriteError("DNAAnalysisPlugin.OnLanguageChange()", ex);
            }
        }

        public override bool Shutdown()
        {
            bool result = true;
            try {
                CloseForm();
            } catch (Exception ex) {
                Logger.WriteError("DNAAnalysisPlugin.Shutdown()", ex);
                result = false;
            }
            return result;
        }
    }


#if !ETO

    public class DNAInheritanceTestPlugin : GenetixPlugin
    {
        private string fDisplayName = "DNAInheritanceTestPlugin";
        private ILangMan fLangMan;

        public override string DisplayName { get { return fDisplayName; } }
        public override ILangMan LangMan { get { return fLangMan; } }

        private DNAInheritanceTest fForm;

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                CloseForm();
            }
            base.Dispose(disposing);
        }

        internal void CloseForm()
        {
            if (fForm != null) {
                fForm.Close();
                fForm = null;
            }
        }

        public override void Execute()
        {
            if (fForm == null) {
                fForm = new DNAInheritanceTest();
                fForm.Show();
            } else {
                CloseForm();
            }
        }

        public override void OnLanguageChange()
        {
            try {
                fLangMan = Host.CreateLangMan(this);
                fDisplayName = fLangMan.LS(CLS.DNAInheritanceTest);

                //if (fForm != null) fForm.SetLocale();
            } catch (Exception ex) {
                Logger.WriteError("DNAInheritanceTestPlugin.OnLanguageChange()", ex);
            }
        }

        public override bool Shutdown()
        {
            bool result = true;
            try {
                CloseForm();
            } catch (Exception ex) {
                Logger.WriteError("DNAInheritanceTestPlugin.Shutdown()", ex);
                result = false;
            }
            return result;
        }
    }

#endif

}
