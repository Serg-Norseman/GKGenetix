/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace GKGenetix.Core
{
    public class Utilities
    {
        public static Stream LoadResourceGZipStream(string gzName)
        {
            var resStream = LoadResourceStream(gzName);
            return new GZipStream(resStream, CompressionMode.Decompress);
        }

        public static Stream LoadResourceStream(string resName)
        {
            Assembly assembly = typeof(Utilities).Assembly;
            Stream resStream = assembly.GetManifestResourceStream("GKGenetix.Core.Resources." + resName);
            return resStream;
        }

        /// <summary>
        /// Opens a browser window at SNPedia for this SNP.
        /// </summary>
        public static void OpenSNPedia(string rsID)
        {
            Process.Start("http://www.snpedia.com/index.php/" + rsID);
        }

        public static byte[] GUnzip2Bytes(byte[] bytes)
        {
            using (var stmInput = new MemoryStream(bytes))
            using (var stmOutput = new MemoryStream()) {
                using (var gs = new GZipStream(stmInput, CompressionMode.Decompress)) {
                    CopyTo(gs, stmOutput);
                }
                return stmOutput.ToArray();
            }
        }

        public static Stream GUnzip2Stream(byte[] bytes)
        {
            var stmOutput = new MemoryStream();
            using (var stmInput = new MemoryStream(bytes)) {
                using (var gs = new GZipStream(stmInput, CompressionMode.Decompress)) {
                    CopyTo(gs, stmOutput);
                }
                stmOutput.Seek(0, SeekOrigin.Begin);
                return stmOutput;
            }
        }

        public static void GUnzipFile(string infile, string outfile)
        {
            using (var msi = new FileStream(infile, FileMode.Open))
            using (var mso = new FileStream(outfile, FileMode.Create)) {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    CopyTo(gs, mso);
                }
                mso.Close();
            }
        }

        private static void CopyTo(GZipStream src, Stream dest)
        {
            byte[] bytes = new byte[4096 * 4];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private static NumberFormatInfo numberFormat;
        private static char numberSeparator;

        public static double ParseFloat(string str, char defaultSeparator = '.', double defaultValue = double.NaN)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            if (numberFormat == null || numberSeparator != defaultSeparator) {
                numberSeparator = defaultSeparator;

                numberFormat = new NumberFormatInfo();
                numberFormat.NumberDecimalSeparator = string.Empty + defaultSeparator;
                numberFormat.NumberGroupSeparator = " ";
            }

            if (!double.TryParse(str, NumberStyles.Float, numberFormat, out double result)) {
                result = defaultValue;
            }
            return result;
        }

        public static T[] MergeArrays<T>(bool refValues, params T[][] arrays)
        {
            int totalLength = 0;
            for (int i = 0; i < arrays.Length; i++) {
                T[] array = arrays[i];
                totalLength += array.Length;
            }

            var combinedArray = new T[totalLength];

            int offset = 0;
            for (int i = 0; i < arrays.Length; i++) {
                T[] array = arrays[i];

                if (refValues) {
                    Array.Copy(array, 0, combinedArray, offset, array.Length);
                } else {
                    Buffer.BlockCopy(array, 0, combinedArray, offset, array.Length);
                }

                offset += array.Length;
            }

            return combinedArray;
        }

        public static string RemoveDuplicates(string value)
        {
            string[] tmp = value.Split(",".ToCharArray()).Distinct().ToArray();
            return string.Join(", ", tmp);
        }
    }
}
