namespace GenetixKit.Core.Model
{
    public interface ISNPHeader
    {
        string RSID { get; }
        string Chromosome { get; }
        int Position { get; }
    }
}
