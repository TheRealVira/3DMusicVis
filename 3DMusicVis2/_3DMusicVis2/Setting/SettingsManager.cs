#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: SettingsManager.cs
// Date - created:2016.09.18 - 10:23
// Date - current: 2016.09.19 - 15:38

#endregion

#region Usings

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#endregion

namespace _3DMusicVis2.Setting
{
    internal static class SettingsManager
    {
        private const string SETTINGS_DIR = "Settings";
        private const string SETTINGS_EXT = ".set";

        public static List<Setting> LoadSettings()
        {
            if (!Directory.Exists(SETTINGS_DIR))
            {
                Directory.CreateDirectory(SETTINGS_DIR);
                return new List<Setting>();
            }

            var toRet = new List<Setting>();
            var reader = new BinaryFormatter();

            foreach (var set in Directory.GetFiles(SETTINGS_DIR, "*" + SETTINGS_EXT))
            {
                using (var file = File.OpenRead(set))
                {
                    toRet.Add((Setting) reader.Deserialize(file));

                    // Clean exit
                    file.Close();
                    file.Dispose();
                }
            }

            return toRet;
        }

        public static void SaveSetting(Setting setting)
        {
            if (!Directory.Exists(SETTINGS_DIR))
            {
                Directory.CreateDirectory(SETTINGS_DIR);
            }

            using (var file = File.OpenWrite(SETTINGS_DIR + "\\" + setting.SettingName + SETTINGS_EXT))
            {
                var writer = new BinaryFormatter();
                writer.Serialize(file, setting);

                // Clean exit
                file.Close();
                file.Dispose();
            }
        }
    }
}