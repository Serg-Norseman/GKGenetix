using System.Collections.Generic;

namespace GenetixKit.Core.Model
{
    public class DNARec
    {
        public DNARec(List<SingleSNP> atdna, List<string> ydna, List<string> mtdna)
        {
            this.atdna = atdna;
            this.ydna = ydna;
            this.mtdna = mtdna;
        }

        public List<SingleSNP> atdna { get; private set; } // 0
        public List<string> ydna { get; private set; } // 1
        public List<string> mtdna { get; private set; } // 2
    }
}
