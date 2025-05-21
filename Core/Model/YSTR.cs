using System.Data;

namespace GenetixKit.Core.Model
{
    public class YSTR
    {
        public string Marker { get; }
        public string Value { get; }

        public YSTR(string marker, string value)
        {
            Marker = marker;
            Value = value;
        }

        public YSTR(IDataRecord values)
        {
            Marker = values.GetString(0);
            Value = values.GetString(1);
        }
    }
}
