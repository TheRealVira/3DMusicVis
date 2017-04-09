#region License

// Copyright (c) 2017, Vira
// All rights reserved.
// Solution: 3DMusicVis
// Project: 3DMusicVis
// Filename: TemplateManager.cs
// Date - created:2016.12.10 - 09:45
// Date - current: 2017.04.09 - 14:10

#endregion

#region Usings

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using _3DMusicVis.Setting.Visualizer;

#endregion

namespace _3DMusicVis.Manager
{
    internal static class TemplateManager
    {
        public static Setting.Visualizer.Setting GetSaveOnlySamples()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "SamplesOnly",
                Shaders = ShaderMode.ScanLine,
                BackgroundColor = new ColorSetting{Color = Color.Black},
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
                BackgroundColor = new ColorSetting
                {
                    Color = Color.Black
                    },
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        IsFrequency = true,
                        Is3D = true,
                        HowIDraw = DrawMode.Dashed,
                        Trans = new Transformation {Position = new Vector2(0, 0f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.White}
                    },
                    new SettingsBundle
                    {
                        Is3D = true,
                        HowIDraw = DrawMode.Dashed,
                        Trans = new Transformation {Position = new Vector2(0, 0f), Scale = Vector2.One},
                        Color = new ColorSetting {Color = Color.White}
                    },
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
                        Color =
                            new ColorSetting
                            {
                                Color = Color.Green,
                                Mode = Setting.Visualizer.ColorMode.Rainbow,
                                Negate = true
                            },
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
                Shaders = /*(*/ ShaderMode.Bloom /*| ShaderMode.Liquify | ShaderMode.ScanLine)*/,
                BackgroundColor = new ColorSetting
                {
                    Color = Color.Black
                    },
                BackgroundImage = new ImageSetting
                {
                    ImageFileName = "heart.png",
                    Tint = Color.White,
                    Mode = ImageMode.Vibrate,
                    Offset = new Vector2(0, -.1f)
                },
                ForegroundImage = new ImageSetting
                {
                    ImageFileName = "determination.png",
                    Tint = Color.White,
                    Mode = ImageMode.None,
                    Offset = new Vector2(0, .4f)
                },
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
                        Color =
                            new ColorSetting
                            {
                                Color = Color.White,
                                Mode = Setting.Visualizer.ColorMode.Rainbow,
                                Negate = true
                            },
                        VerticalMirror = true,
                        HorizontalMirror = true
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting GetStriker()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "Striker",
                Shaders = ShaderMode.Blur | ShaderMode.Bloom | ShaderMode.Liquify | ShaderMode.ScanLine,
                BackgroundColor = new ColorSetting
                {
                    Color = Color.Black,
                    Mode = Setting.Visualizer.ColorMode.Static
                    },
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
                        Color = new ColorSetting {Color = Color.Red, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        IsFrequency = false,
                        Trans = new Transformation {Position = new Vector2(0, 0f), Scale = new Vector2(1, 1)},
                        Color = new ColorSetting {Color = Color.Red}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(.05f, 0f), Scale = new Vector2(.9f, 1)},
                        Color = new ColorSetting {Color = Color.Red, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting GetDebug()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "Debug",
                Shaders = ShaderMode.Blur,
                //Shaders = (ShaderMode.Bloom | ShaderMode.Liquify | ShaderMode.ScanLine),
                BackgroundColor = new ColorSetting
                {
                    Color = Color.Black
                    },
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(0, .5f),
                                Scale = new Vector2(1f, 1f),
                                Rotation = (float) Math.PI / 4
                            },
                        Color = new ColorSetting {Color = Color.White, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting Get3DFreqVis()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "3DFreqVis",
                // Shaders = (ShaderMode.Bloom | ShaderMode.ScanLine),
                BackgroundColor = new ColorSetting
                {
                    Color = Color.Black
                    },
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Connected,
                        IsFrequency = true,
                        Is3D = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(0, 0f),
                                Scale = new Vector2(1f, 1f)
                            },
                        Color = new ColorSetting {Color = Color.White, Mode = Setting.Visualizer.ColorMode.Rainbow}
                    }
                }
            };
        }

        public static Setting.Visualizer.Setting Get3DSampVis()
        {
            return new Setting.Visualizer.Setting
            {
                SettingName = "3DSampVis",
                Shaders = (ShaderMode.Bloom | ShaderMode.ScanLine),
                BackgroundColor = new ColorSetting
                {
                    Color = Color.Black
                    },
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Dashed,
                        Is3D = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(0, 0f),
                                Scale = new Vector2(1f, 1f)
                            },
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
                //Shaders = (ShaderMode.Bloom | ShaderMode.Liquify | ShaderMode.ScanLine),
                BackgroundColor = new ColorSetting
                {
                    Color = Color.White
                    },
                ForegroundImage = new ImageSetting
                {
                    ImageFileName = "yingyang.png",
                    Tint = Color.White,
                    Mode = ImageMode.Rotate | ImageMode.Vibrate | ImageMode.ReverseOnBeat | ImageMode.HoverRender,
                    ReverseRotation = true,
                    RotationSpeedMutliplier = 20f,
                    RotationNotice = .7f
                },
                Bundles = new List<SettingsBundle>
                {
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans =
                            new Transformation
                            {
                                Position = new Vector2(.05f, -.05f),
                                Scale = new Vector2(.9f, 1),
                                Rotation = (float) Math.PI
                            },
                        Color = new ColorSetting {Color = Color.Black /*, Mode = Setting.Visualizer.ColorMode.Rainbow*/}
                    },
                    new SettingsBundle
                    {
                        HowIDraw = DrawMode.Blocked,
                        IsFrequency = true,
                        Trans = new Transformation {Position = new Vector2(.05f, .05f), Scale = new Vector2(.9f, 1)},
                        Color =
                            new ColorSetting {Color = Color.DarkGray /*, Mode = Setting.Visualizer.ColorMode.Rainbow*/}
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
            FastSave(Get3DFreqVis());
            FastSave(Get3DSampVis());

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