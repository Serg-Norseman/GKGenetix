using System.Data;

namespace GenetixKit.Core.Model
{
    internal interface ITableRow
    {
        void Load(IDataRecord values);
    }
}
