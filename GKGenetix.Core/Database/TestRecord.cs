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
using GKGenetix.Core.Model;

namespace GKGenetix.Core.Database
{
    public class TestRecord : IDataRecord
    {
        public string KitNo { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public bool Disabled { get; set; }
        public int Lng { get; set; }
        public int Lat { get; set; }
        public DateTime LastModified { get; set; }
        public int Reference { get; set; }
        public int RoH_Status { get; set; }


        public string Location { get; set; }


        public TestRecord()
        {
        }

        public void PrepareValues()
        {
            if (Sex == "U")
                Sex = "Unknown";
            else if (Sex == "M")
                Sex = "Male";
            else if (Sex == "F")
                Sex = "Female";

            string xy = Lng + ":" + Lat;
            if (xy == "0:0")
                xy = "Unknown";
            Location = xy;
        }
    }
}
