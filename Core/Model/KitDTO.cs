using System;
using System.Data;

namespace GenetixKit.Core.Model
{
    internal class KitDTO
    {
        public string KitNo { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public bool Disabled { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public DateTime LastModified { get; set; }
        public int Reference { get; set; }
        public int RoH_Status { get; set; }


        public string Location { get; set; }


        // kit_no, name, sex, disabled, coalesce(x, 0), coalesce(y, 0), last_modified
        public KitDTO(IDataRecord values, bool convertSex, bool displayLocation)
        {
            KitNo = values.GetString(0);
            Name = values.GetString(1);
            Sex = values.GetString(2);
            Disabled = values.GetBoolean(3);
            X = values.GetInt32(4);
            Y = values.GetInt32(5);
            LastModified = values.GetDateTime(6);
            Reference = values.GetInt32(7);
            RoH_Status = values.GetInt32(8);

            if (convertSex) {
                if (Sex == "U")
                    Sex = "Unknown";
                else if (Sex == "M")
                    Sex = "Male";
                else if (Sex == "F")
                    Sex = "Female";
            }

            if (displayLocation) {
                string xy = X + ":" + Y;
                if (xy == "0:0")
                    xy = "Unknown";
                Location = xy;
            }
        }
    }
}
