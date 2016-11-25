using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using _3DMusicVis2.Setting.Visualizer;
using _3DMusicVis2._3DHelper;

namespace _3DMusicVis2.RenderFrame
{
    internal static class _3DSampleRenderer
    {
        private static _2DMusicVisRenderFrame _renderer;

        private static Grid _myGrid;

        public static void Initialise(GraphicsDevice device, ContentManager manager)
        {
            if (_myGrid != null) return;

            _renderer =
                new _2DMusicVisRenderFrame
                {
                    Render = Draw,
                    UpdateRenderer = UpdateRenderer,
                    ClearColor = Color.Transparent,
                    ColorMode = ColorMode.SideEqualsCenter,
                    FadeOutColor = Color.Black,
                    ForeGroundColor = Color.White,
                    HightMultiplier = 1.5f
                };

            _myGrid = new Grid(manager);
        }

        public static void Dispose()
        {
            _myGrid.Dispose();
        }

        public static void UpdateRenderer(ReadOnlyCollection<float> samples)
        {
            var length = (int)(samples.Count / 25);
            var foregroundColors = new float[length, length];

            for (var x = 0; x < length; x++)
            {
                for (var y = 0; y < length; y++)
                {
                    foregroundColors[x, y] = samples[x] * Game1.VIRTUAL_RESOLUTION.Height / 16;
                }
            }

            _myGrid.Update(foregroundColors);
        }

        public static void Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings,
            ref RenderTarget2D tex)
        {
            device.SetRenderTarget(tex);
            device.Clear(_renderer.ClearColor);
            Game1.SpriteBatch.Begin();

            _myGrid.Draw(device, cam, settings);

            Game1.SpriteBatch.End();

            device.SetRenderTarget(Game1.DEFAULT_RENDERTARGET);
        }

        public static Texture2D Draw(GraphicsDevice device, GameTime gameTime, Camera cam, DrawMode settings)
        {
            var toRet = new RenderTarget2D(device, Game1.VIRTUAL_RESOLUTION.Width,
                Game1.VIRTUAL_RESOLUTION.Height, true,
                device.DisplayMode.Format, DepthFormat.Depth24);

            Draw(device, gameTime, cam, settings, ref toRet);

            return toRet;
            //device.Clear(ClearOptions.Draw | ClearOptions.DepthBuffer, _renderer.ClearColor, 1.0f, 0);
            //using (SpriteBatch sprite = new SpriteBatch(device))
            //{
            //    sprite.Begin();
            //    sprite.Draw(shadowMap, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 0.4f, SpriteEffects.None, 1);
            //    sprite.End();
            //}
        }
    }
}
