using System;
using GKGenetix.Core;

namespace DNAInheritanceTest
{
    class Program : IDisplay
    {
        static void Main(string[] args)
        {
            var d1 = FileFormats.ReadAncestryDNAFile(@"../../../temp/test1.txt");
            var d2 = FileFormats.ReadAncestryDNAFile(@"../../../temp/test2.txt");

            Analytics.Compare(d1, d2, new Program());

            Console.ReadKey();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
