#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: Serializer.cs
// Date - created:2016.12.10 - 09:37
// Date - current: 2017.04.14 - 20:16

#endregion

#region Usings

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

#endregion

namespace _3DMusicVis
{
    internal static class SettingsManager
    {
        public const string SETTINGS_DIR = "Settings";
        public const string SETTINGS_EXT = "set";

        public const string OPTIONS_EXT = "opt";
        public const string OPTIONS_DIR = "Settings";

        public const string SHADER_DIR = "Shader";

        public static List<T> Load<T>(string directory, string extension)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                return new List<T>();
            }

            var toRet = new List<T>();

            foreach (var set in Directory.GetFiles(directory, "*" + "." + extension))
                toRet.Add(Deserialize<T>(File.ReadAllBytes(set)));

            return toRet;
        }

        public static void Save<T>(T setting, string directory, string name, string extension)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllBytes(directory + "\\" + name + "." + extension, Serialize(setting));
            //using (var writer = new StreamWriter(directory + "\\" + name + "." + extension))
            //{
            //    writer.Write(Serialize(setting));

            //    // Clean exit
            //    writer.Close();
            //    writer.Dispose();
            //}
        }

        public static void Delete(string directory, string name, string extension)
        {
            if (!File.Exists(directory + "\\" + name + "." + extension)) return;

            File.Delete(directory + "\\" + name + "." + extension);
        }

        public static byte[] Serialize<T>(T obj)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var stream = new MemoryStream();
            using (var writer =
                XmlDictionaryWriter.CreateBinaryWriter(stream))
            {
                serializer.WriteObject(writer, obj);
            }
            return stream.ToArray();
        }

        public static T Deserialize<T>(byte[] data)
        {
            var serializer = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream(data))
            using (var reader =
                XmlDictionaryReader.CreateBinaryReader(
                    stream, XmlDictionaryReaderQuotas.Max))
            {
                return (T) serializer.ReadObject(reader);
            }
        }
    }
}