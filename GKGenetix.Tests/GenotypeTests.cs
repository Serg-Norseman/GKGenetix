/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
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
