#region License

// Copyright (c) 2016, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: TemplateManager.cs
// Date - created:2016.10.23 - 16:24
// Date - current: 2016.10.26 - 18:31

#endregion

#region Usings

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using _3DMusicVis2.Setting.Visualizer;

#endregion

namespace _3DMusicVis2.Manager
{
    internal static class TemplateManager
    {
        public static Setting.Visualizer.Setting GetSaveOnlySamples()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "SamplesOnly",
                Shaders = ShaderMode.ScanLine,
                BackgroundColor = Color.Black,
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        Trans = new Transformation {Position = new Vector2(0, -.3f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Blue, Mode = Setting.Visualizer.ColorMode.Breath}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        Trans = new Transformation {Position = new Vector2(0, -.15f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Red}
                    },
                    new SettingsBundle
                    {
                        Trans = new Transformation {Position = new Vector2(0, 0), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Yellow, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        Trans = new Transformation {Position = new Vector2(0, .15f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Red}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        Trans = new Transformation {Position = new Vector2(0, .3f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Blue, Mode = Setting.Visualizer.ColorMode.Breath}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting GetTest101()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "Test101",
                //Shaders = (ShaderMode.Bloom | ShaderMode.Liquify | ShaderMode.ScanLine),
                BackgroundColor = Color.Black,
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(0, 0), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Blue}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(0, 0),
                                Scale = Vector2.One
                            },
                        Color = new ColorSetting {Color = Color.Green, Mode = Setting.Visualizer.ColorMode.Rainbow, Negate = true},
                        VerticalMirror = true
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(0, 0),
                                Scale = Vector2.One,
                                Rotation = (float) Math.PI
                            },
                        Color = new ColorSetting {Color = Color.Green, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        Trans = new Transformation {Scale = Vector2.One, Position = new Vector2(0, 0)},
                        Color = new ColorSetting {Color = Color.Red}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        Trans = new Transformation {Position = new Vector2(0, .1f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Violet}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Dashed,
                        Trans = new Transformation {Position = new Vector2(0, -.1f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.Brown}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting GetFrequencyOnly()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "FrequencyOnly",
                //Shaders = (ShaderMode.Bloom | ShaderMode.Liquify | ShaderMode.ScanLine),
                BackgroundColor = Color.White,
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(0, .01f),
                                Scale = new Vector2(.9f, 1),
                                Rotation = (float) Math.PI
                            },
                        Color = new ColorSetting {Color = Color.Black /*, Mode = Setting.Visualizer.ColorMode.Rainbow*/}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(0f, .01f), Scale = new Vector2(.9f, 1)},
                        Color =
                            new ColorSetting {Color = Color.DarkGray /*, Mode = Setting.Visualizer.ColorMode.Rainbow*/}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting GetStriker()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "Striker",
                Shaders = ShaderMode.Bloom | ShaderMode.Liquify | ShaderMode.ScanLine,
                BackgroundColor = Color.Black,
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(.05f, 0f),
                                Scale = new Vector2(.9f, 1),
                                Rotation = (float) Math.PI
                            },
                        Color = new ColorSetting {Color = Color.Black, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        IsFrequency = false,
                        Trans = new Transformation {Position = new Vector2(0, 0f), Scale = new Vector2(1, 1)},
                        Color = new ColorSetting {Color = Color.White}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(.05f, 0f), Scale = new Vector2(.9f, 1)},
                        Color = new ColorSetting {Color = Color.DarkGray, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting GetDebug()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "Debug",
                //Shaders = (ShaderMode.Bloom | ShaderMode.Liquify | ShaderMode.ScanLine),
                BackgroundColor = Color.Black,
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(0, .5f), Scale = new Vector2(1f, 1f), Rotation = (float)Math.PI/4},
                        Color = new ColorSetting {Color = Color.White, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting GetYingYang()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "YingYang",
                Shaders = /*(*/ShaderMode.Bloom /*| ShaderMode.Liquify | ShaderMode.ScanLine)*/,
                BackgroundColor = Color.Black,
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(0, .75f), Scale = new Vector2(1f, .5f)},
                        Color = new ColorSetting {Color = Color.White, Mode = Setting.Visualizer.ColorMode.Rainbow},
                        HorizontalMirror = true
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(0, -.25f), Scale = new Vector2(1f, 1f)},
                        Color = new ColorSetting {Color = Color.White, Mode = Setting.Visualizer.ColorMode.Rainbow, Negate = true},
                        VerticalMirror = true,
                        HorizontalMirror = true
                    }
                }
            };
        }

        public static void SaveDefaultTemplates()
        {
            FastSave(GetSaveOnlySamples());
            FastSave(GetTest101());
            FastSave(GetFrequencyOnly());
            FastSave(GetStriker());
            FastSave(GetYingYang());

#if (DEBUG)
            FastSave(GetDebug());
#endif
        }

        public static void FastSave(Setting.Visualizer.Setting setting)
        {
            SettingsManager.Save(setting, SettingsManager.SETTINGS_DIR, setting.SettingName,
                SettingsManager.SETTINGS_EXT);
        }
    }
}