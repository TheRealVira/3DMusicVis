using Microsoft.Xna.Framework;

namespace _3DMusicVis2
{
    public static class MyMath
    {
        public static float Abs(this float value)
        {
            return value > 0 ? value : -value;
        }

        public static float Normalize(this float value, float min, float max) => (value - min) / (max - min);

        public static Color Negate(this Color color)
        {
            return new Color(255-color.R,255-color.G,255-color.B);
        }

        public static Color HalfNegate(this Color color)
        {
            return new Color(127-color.R, 127 - color.G, 127 - color.B);
        }
    }
}
