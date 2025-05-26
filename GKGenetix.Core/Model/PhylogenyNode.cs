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

using System.Collections.Generic;

namespace GKGenetix.Core.Model
{
    // Phylogeny, phylogenetic tree (https://en.wikipedia.org/wiki/Phylogenesis)
    public class PhylogenyNode<T>
    {
        private List<T> fChildren = null;

        public List<T> Children
        {
            get {
                if (fChildren == null) {
                    fChildren = new List<T>();
                }

                return fChildren;
            }
        }

        public string Name;
        public string Markers;
    }


    public class MtDNAPhylogenyNode : PhylogenyNode<MtDNAPhylogenyNode>
    {
        public MtDNAPhylogenyNode(string name, string markers = "")
        {
            Name = name;
            Markers = markers;
        }
    }


    public class ISOGGYTreeNode : PhylogenyNode<ISOGGYTreeNode>
    {
        public ISOGGYTreeNode(string name, string markers = "")
        {
            Name = name;
            Markers = markers;
        }
    }
}
