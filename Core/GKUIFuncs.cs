/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GenetixKit.Forms;

namespace GenetixKit.Core
{
    internal static class GKUIFuncs
    {
        public static void setStatus(string message)
        {
            Program.GGKitFrmMainInst.setStatusMessage(message);
        }

        public static void setProgress(int percent)
        {
            Program.GGKitFrmMainInst.setProgress(percent);
        }

        public static void enableSave()
        {
            Program.GGKitFrmMainInst.enableSave();
        }

        public static void disableSave()
        {
            Program.GGKitFrmMainInst.disableSave();
        }

        public static void enableMenu()
        {
            Program.GGKitFrmMainInst.MainMenuStrip.Enabled = true;
        }

        public static void disableMenu()
        {
            Program.GGKitFrmMainInst.MainMenuStrip.Enabled = false;
        }


        public static void enableToolbar()
        {
            Program.GGKitFrmMainInst.enableToolbar();
        }

        public static void disableToolbar()
        {
            Program.GGKitFrmMainInst.disableToolbar();
        }

        public static void hideAllMdiChildren()
        {
            Program.GGKitFrmMainInst.hideAllChildren("");
        }

        public static void SaveInfoFromActiveMdiChild()
        {
            Form mdifrm = Program.GGKitFrmMainInst.ActiveMdiChild;
            if (mdifrm.Name == "NewEditKitFrm")
                ((NewEditKitFrm)mdifrm).Save();
            else if (mdifrm.Name == "SettingsFrm")
                ((SettingsFrm)mdifrm).Save();
            else if (mdifrm.Name == "OneToOneCmpFrm")
                ((OneToOneCmpFrm)mdifrm).Save();
            else if (mdifrm.Name == "QuickEditKit")
                ((QuickEditKit)mdifrm).Save();
            else if (mdifrm.Name == "MtPhylogenyFrm")
                ((MtPhylogenyFrm)mdifrm).Save();
        }

        public static void enable_EnableKitToolbarBtn()
        {
            Program.GGKitFrmMainInst.enable_EnableKitToolbarBtn();
        }

        public static void disable_EnableKitToolbarBtn()
        {
            Program.GGKitFrmMainInst.disable_EnableKitToolbarBtn();
        }

        public static void enableDeleteKitToolbarBtn()
        {
            Program.GGKitFrmMainInst.enableDeleteKitToolbarBtn();
        }

        public static void disableDeleteKitToolbarBtn()
        {
            Program.GGKitFrmMainInst.disableDeleteKitToolbarBtn();
        }


        public static void enable_DisableKitToolbarBtn()
        {
            Program.GGKitFrmMainInst.enable_DisableKitToolbarBtn();
        }

        public static void disable_DisableKitToolbarBtn()
        {
            Program.GGKitFrmMainInst.disable_DisableKitToolbarBtn();
        }
        ///
        public static void disableKit()
        {
            Form mdifrm = Program.GGKitFrmMainInst.ActiveMdiChild;
            if (mdifrm.Name == "NewEditKitFrm")
                ((NewEditKitFrm)mdifrm).Disable();
            else if (mdifrm.Name == "QuickEditKit")
                ((QuickEditKit)mdifrm).Disable();
        }
        public static void enableKit()
        {
            Form mdifrm = Program.GGKitFrmMainInst.ActiveMdiChild;
            if (mdifrm.Name == "NewEditKitFrm")
                ((NewEditKitFrm)mdifrm).Enable();
            else if (mdifrm.Name == "QuickEditKit")
                ((QuickEditKit)mdifrm).Enable();
        }
        public static void deleteKit()
        {
            Form mdifrm = Program.GGKitFrmMainInst.ActiveMdiChild;
            if (mdifrm.Name == "NewEditKitFrm")
                ((NewEditKitFrm)mdifrm).Delete();
            else if (mdifrm.Name == "QuickEditKit")
                ((QuickEditKit)mdifrm).Delete();
        }

        public static string sqlSafe(string text)
        {
            text = Regex.Replace(text, "[^A-Za-z0-9 ]", " ");
            return text;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream()) {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn)) {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        public static Process execute(string file1, string file2, string diff_work_dir)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WorkingDirectory = diff_work_dir;
            p.StartInfo.FileName = diff_work_dir + "diff.exe";
            p.StartInfo.Arguments = file1 + " " + file2;
            p.Start();
            return p;
        }
    }
}
