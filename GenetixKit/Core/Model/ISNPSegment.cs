namespace GenetixKit.Core.Model
{
    public interface ISNPSegment
    {
        string Chromosome { get; }
        int StartPosition { get; }
        int EndPosition { get; }
        double SegmentLength_cm { get; }
        int SNPCount { get; }
    }
}
