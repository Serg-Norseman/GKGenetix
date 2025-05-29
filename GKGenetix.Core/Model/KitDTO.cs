/*
 *  "GKGenetix", the simple DNA analysis kit.
 *  Copyright (C) 2022-2025 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GKGenetix".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Data;

namespace GKGenetix.Core.Model
{
    public class KitDTO
    {
        public string KitNo { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public bool Disabled { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }
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
            Longitude = values.GetInt32(4);
            Latitude = values.GetInt32(5);
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
                string xy = Longitude + ":" + Latitude;
                if (xy == "0:0")
                    xy = "Unknown";
                Location = xy;
            }
        }
    }
}
