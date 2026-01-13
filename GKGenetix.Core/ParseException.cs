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
    public class ParseException : Exception
    {
        public ParseException(string message, params object[] args) : base(string.Format(message, args))
        {
        }
    }
}
