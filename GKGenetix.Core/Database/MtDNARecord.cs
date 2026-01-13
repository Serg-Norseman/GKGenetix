/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

namespace GKGenetix.Core.Database
{
    public class MtDNARecord : IDataRecord
    {
        public string Mutations { get; set; }
        public string Fasta { get; set; }
    }
}
