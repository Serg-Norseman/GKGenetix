/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;

namespace GKGenetix.Core
{
    public sealed class DNATestInfo
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public char Sex { get; set; }
        public string FileReference { get; set; }
    }


    public interface ITestProvider
    {
        IList<DNATestInfo> RequestTests();
    }
}
