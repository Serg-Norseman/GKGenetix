/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System.Collections.Generic;
using System.Linq;

namespace GKGenetix.Core.Reference
{
    public sealed class Haplogroup
    {
        public string Name { get; set; }

        public List<Haplogroup> Children { get; set; }

        public List<HaplogroupMutation> Mutations { get; set; }

        public Haplogroup()
        {
            Children = new List<Haplogroup>();
            Mutations = new List<HaplogroupMutation>();
        }

        public override string ToString()
        {
            return Children.Any() ? string.Format("{0} -> {1}", Name, string.Join(", ", Children.Select(x => x.Name))) : Name;
        }
    }
}
