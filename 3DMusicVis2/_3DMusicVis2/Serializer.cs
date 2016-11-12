#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Serializer.cs
// Date - created:2016.10.23 - 14:56
// Date - current: 2016.11.11 - 09:51

#endregion

#region Usings

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace _3DMusicVis2
{
    internal static class SettingsManager
    {
        public const string SETTINGS_DIR = "Settings";
        public const string SETTINGS_EXT = "set";

        public const string OPTIONS_EXT = "opt";
        public const string OPTIONS_DIR = "Settings";

        public static List<T> Load<T>(string directory, string extension)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                return new List<T>();
            }

            var toRet = new List<T>();
            var reader = new BinaryFormatter();

            foreach (var set in Directory.GetFiles(directory, "*" + "." + extension))
            {
                using (var file = File.OpenRead(set))
                {
                    toRet.Add((T) reader.Deserialize(file));

                    // Clean exit
                    file.Close();
                    file.Dispose();
                }
            }

            return toRet;
        }

        public static void Save<T>(T setting, string directory, string name, string extension)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var file = File.OpenWrite(directory + "\\" + name + "." + extension))
            {
                var writer = new BinaryFormatter();
                writer.Serialize(file, setting);

                // Clean exit
                file.Close();
                file.Dispose();
            }
        }

        public static void Delete(string directory, string name, string extension)
        {
            if (!File.Exists(directory + "\\" + name + "." + extension)) return;

            File.Delete(directory + "\\" + name + "." + extension);
        }
    }
}