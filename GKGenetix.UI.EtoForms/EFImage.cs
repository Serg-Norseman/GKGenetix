/*
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

using Eto.Drawing;
using GKGenetix.Core;

namespace GKGenetix.UI
{
    internal class EFImage : AbstractImage
    {
        private Bitmap img;
        private Graphics g;
        private Pen pen;

        public Bitmap Value { get { return img; } }

        public override void Dispose()
        {
            if (pen != null) pen.Dispose();
            if (g != null) g.Dispose();
        }

        public override void SetSize(int width, int height)
        {
            img = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            g = new Graphics(img);
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
