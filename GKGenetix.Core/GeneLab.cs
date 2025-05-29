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

namespace GKGenetix.Core
{
    public enum Relationship
    {
        Sibling,
        Child,
        Parent,
        GrandChild,
        GrandParent,
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
        Step6,
        Step7,
        Step8,
        Step9,
        Step10,
        Step11,
        Step12,
        Step13,
        Step14,
    }

    public static class GeneLab
    {
        public static char GetComplementaryNucleotide(char n)
        {
            if (n == 'A') {
                n = 'T'; // A -> T
            } else if (n == 'T') {
                n = 'A'; // T -> A
            } else if (n == 'C') {
                n = 'G'; // C -> G
            } else if (n == 'G') {
                n = 'C'; // G -> C
            }
            return n;
        }

        public static int ConvertToSteps(float cm)
        {
            if (cm >= 2800) return 1;
            if (cm >= 1400) return 2;
            if (cm >= 700) return 3;
            if (cm >= 350) return 4;
            if (cm >= 175) return 5;
            if (cm >= 87) return 6;
            if (cm >= 43) return 7;
            if (cm >= 22) return 8;
            if (cm >= 11) return 9;
            if (cm >= 5) return 10;
            if (cm > 0) return 11;
            return 100;
        }
    }
}
