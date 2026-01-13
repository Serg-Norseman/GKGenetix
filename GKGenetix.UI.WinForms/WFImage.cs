/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Drawing;
using GKGenetix.Core;

namespace GKGenetix.UI
{
    internal class WFImage : AbstractImage
    {
        private Image img;
        private Graphics g;
        private Pen pen;

        public Image Value { get { return img; } }

        public override void Dispose()
        {
            if (pen != null) pen.Dispose();
            if (g != null) g.Dispose();
        }

        public override void SetSize(int width, int height)
        {
            img = new Bitmap(width, height);
            g = Graphics.FromImage(img);
        }

        public override void SetPen(int alpha, int red, int green, int blue, float width)
        {
            if (pen != null) pen.Dispose();

            pen = new Pen(Color.FromArgb(alpha, red, green, blue), width);
        }

        public override void DrawLine(float x1, float y1, float x2, float y2)
        {
            g.DrawLine(pen, x1, y1, x2, y2);
        }
    }
}
