/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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


    public sealed class PMHighlight
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public int State { get; set; }

        public PMHighlight(int start, int length, int state)
        {
            Start = start;
            Length = length;
            State = state;
        }
    }
}
