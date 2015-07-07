using UnityEngine;

namespace UnityExtensions
{

    public static class ColorExtension
    {
        public static Color Darken(this Color color, float by = .2f)
        {
            return new Color(Mathf.Max(0f, color.r - by), Mathf.Max(0f, color.g - by), Mathf.Max(0f, color.b - by), color.a);
        }

        public static Color Lighten(this Color color, float by = .2f)
        {
            return new Color(Mathf.Min(1f, color.r + by), Mathf.Min(1f, color.g + by), Mathf.Min(1f, color.b + by), color.a);
        }
    }
}