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

using System.Runtime.InteropServices;
using GKGenetix.Core.Model;
using NUnit.Framework;

namespace GKGenetix.Tests
{
    [TestFixture]
    public class GenotypeTests
    {
        [Test]
        public void Test_Size()
        {
            Assert.AreEqual(4, Marshal.SizeOf(typeof(Genotype)));
        }

        [Test]
        public void Test_New()
        {
            var instance = new Genotype("AT");
            Assert.AreEqual('A', instance.A1);
            Assert.AreEqual('T', instance.A2);

            instance = new Genotype('G', 'C');
            Assert.AreEqual('G', instance.A1);
            Assert.AreEqual('C', instance.A2);

            instance = new Genotype("G");
            Assert.AreEqual('G', instance.A1);
            Assert.AreEqual('0', instance.A2);
        }
    }
}
