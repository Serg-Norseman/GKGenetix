using System.Collections.Generic;

namespace GenetixKit.Core.Model
{
    // Phylogeny, phylogenetic tree (https://en.wikipedia.org/wiki/Phylogenesis)
    internal class PhylogenyNode<T>
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


    internal class MtDNAPhylogenyNode : PhylogenyNode<MtDNAPhylogenyNode>
    {
        public MtDNAPhylogenyNode(string name, string markers)
        {
            Name = name;
            Markers = markers;
        }
    }


    internal class ISOGGYTreeNode : PhylogenyNode<ISOGGYTreeNode>
    {
        public ISOGGYTreeNode(string name, string markers)
        {
            Name = name;
            Markers = markers;
        }
    }
}
