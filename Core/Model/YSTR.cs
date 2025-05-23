using System.Data;

namespace GenetixKit.Core.Model
{
    public class YSTR : ITableRow
    {
        public string Marker { get; private set; }
        public string Value { get; private set; }


        public YSTR()
        {
        }

        public YSTR(string marker, string value)
        {
            Marker = marker;
            Value = value;
        }

        public void Load(IDataRecord values)
        {
            Marker = values.GetString(0);
            Value = values.GetString(1);
        }
    }
}
