﻿/*
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

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GKGenetix.Core
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings fSerializerSettings;

        private static JsonSerializerSettings SerializerSettings
        {
            get {
                if (fSerializerSettings == null) {
                    var converters = new List<JsonConverter> { new StringEnumConverter { NamingStrategy = null } };
                    var resolver = new DefaultContractResolver();
                    fSerializerSettings = new JsonSerializerSettings {
                        ContractResolver = resolver,
                        Converters = converters,
                        NullValueHandling = NullValueHandling.Ignore,
                        MaxDepth = 255
                    };
                }
                return fSerializerSettings;
            }
        }


        public static string SerializeObject(object target)
        {
            return JsonConvert.SerializeObject(target, Formatting.Indented, SerializerSettings);
        }

        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, SerializerSettings);
        }


        public static void SerializeToFile(object target, string fileName)
        {
            using (var fs = File.CreateText(fileName)) {
                string js = JsonConvert.SerializeObject(target, Formatting.Indented, SerializerSettings);
                fs.Write(js);
            }
        }

        public static T DeserializeFromFile<T>(string fileName)
        {
            using (var fs = File.OpenText(fileName)) {
                string content = fs.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(content, SerializerSettings);
            }
        }

        public static T DeserializeFromStream<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream)) {
                string content = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(content, SerializerSettings);
            }
        }
    }
}
