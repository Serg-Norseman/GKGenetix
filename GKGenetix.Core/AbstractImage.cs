/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace GKGenetix.Core
{
    public abstract class AbstractImage : IDisposable
    {
        public AbstractImage()
        {
        }

        public abstract void Dispose();

        public abstract void SetSize(int width, int height);

        public abstract void SetPen(int alpha, int red, int green, int blue, float width);

        public abstract void DrawLine(float x1, float y1, float x2, float y2);
    }
}
