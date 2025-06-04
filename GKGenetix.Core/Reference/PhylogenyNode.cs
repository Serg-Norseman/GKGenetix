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

namespace GKGenetix.Core.Reference
{
    // Phylogeny, phylogenetic tree (https://en.wikipedia.org/wiki/Phylogenesis)
    public class PhylogenyNode<T> where T : PhylogenyNode<T>
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

        public T Parent;
        public string Name;
        public string Markers;
        public int Depth;

        public int Status;

        protected PhylogenyNode(T parent, string name, string markers)
        {
            Parent = parent;
            Name = name;
            Markers = markers.Replace(" ", "");

            if (parent == null)
                Depth = 1;
            else Depth = parent.Depth + 1;
        }
    }


    public class MtDNAPhylogenyNode : PhylogenyNode<MtDNAPhylogenyNode>
    {
        public MtDNAPhylogenyNode(MtDNAPhylogenyNode parent, string name, string markers = "") : base(parent, name, markers)
        {
        }
    }


    public class ISOGGYTreeNode : PhylogenyNode<ISOGGYTreeNode>
    {
        public ISOGGYTreeNode(ISOGGYTreeNode parent, string name, string markers = "") : base(parent, name, markers)
        {
        }
    }
}
