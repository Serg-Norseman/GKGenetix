/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;

namespace GKGenetix.Core.Database
{
    public class TestRecord : IDataRecord
    {
        public string KitNo { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public bool Disabled { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }
        public DateTime LastModified { get; set; }
        public int Reference { get; set; }
        public int RoH_Status { get; set; }


        public string Location
        {
            get {
                string xy = Lng.ToString("#0.000000") + ":" + Lat.ToString("#0.000000");
                return xy;
            }
        }


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
        }
    }
}
