namespace GenetixKit.Core.Model
{
    internal class MDMapRow
    {
        public string MapLocus;
        public string Starting;
        public string Ending;
        public string bpLength;
        public string Shorthand;
        public string Description;

        public MDMapRow(string mapLocus, string starting, string ending, string bpLength, string shorthand, string description)
        {
            MapLocus = mapLocus;
            Starting = starting;
            Ending = ending;
            this.bpLength = bpLength;
            Shorthand = shorthand;
            Description = description;
        }
    }
}
