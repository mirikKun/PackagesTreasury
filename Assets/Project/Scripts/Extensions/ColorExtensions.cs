using System;
using UnityEngine;

namespace Project.Scripts.Extensions
{
    public static class ColorExtensions
    {
     
        public static Color SetAlpha(this Color color, float alpha)
            => new(color.r, color.g, color.b, alpha);
        
        public static Color With(this Color color,float? red, float? green, float? blue, float? alpha)
            => new Color(red ?? color.r, green ?? color.g, blue ?? color.b, alpha ?? color.a);

        
        

        /// <summary>
        /// Converts a Color to a hexadecimal string.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>A hexadecimal string representation of the color.</returns>
        public static string ToHex(this Color color)
            => $"#{ColorUtility.ToHtmlStringRGBA(color)}";

        /// <summary>
        /// Converts a hexadecimal string to a Color.
        /// </summary>
        /// <param name="hex">The hexadecimal string to convert.</param>
        /// <returns>The Color represented by the hexadecimal string.</returns>
        public static Color FromHex(this string hex) {
            if (ColorUtility.TryParseHtmlString(hex, out Color color)) {
                return color;
            }

            throw new ArgumentException("Invalid hex string", nameof(hex));
        }

        /// <summary>
        /// Blends two colors with a specified ratio.
        /// </summary>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <param name="ratio">The blend ratio (0 to 1).</param>
        /// <returns>The blended color.</returns>
        public static Color Blend(this Color color1, Color color2, float ratio) {
            ratio = Mathf.Clamp01(ratio);
            return new Color(
                color1.r * (1 - ratio) + color2.r * ratio,
                color1.g * (1 - ratio) + color2.g * ratio,
                color1.b * (1 - ratio) + color2.b * ratio,
                color1.a * (1 - ratio) + color2.a * ratio
            );
        }
        
        public static Color Invert(this Color color)
            => new(1 - color.r, 1 - color.g, 1 - color.b, color.a);
    }
}