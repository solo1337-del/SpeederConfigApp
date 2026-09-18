using System;
using System.Globalization;
using System.Windows.Media;

namespace SpeederConfigApp.Models
{
    public static class ColorConverterHelper
    {
        /// <summary>
        /// Converts an RGB integer (e.g. 16711680 for Red) to WPF Color.
        /// (R << 16) | (G << 8) | B
        /// </summary>
        public static Color IntToColor(int rgbValue)
        {
            if (rgbValue <= 0)
            {
                return Colors.Transparent;
            }

            byte r = (byte)((rgbValue >> 16) & 0xFF);
            byte g = (byte)((rgbValue >> 8) & 0xFF);
            byte b = (byte)(rgbValue & 0xFF);

            return Color.FromRgb(r, g, b);
        }

        /// <summary>
        /// Converts WPF Color to RGB integer.
        /// </summary>
        public static int ColorToInt(Color color)
        {
            return (color.R << 16) | (color.G << 8) | color.B;
        }

        /// <summary>
        /// Parses a decimal string (or hex #RRGGBB) to integer.
        /// </summary>
        public static int ParseColorString(string? text, int fallback = 0)
        {
            if (string.IsNullOrWhiteSpace(text))
                return fallback;

            text = text.Trim();

            if (text.StartsWith("#"))
            {
                text = text.Substring(1);
                if (int.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int hexVal))
                {
                    return hexVal;
                }
            }

            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intVal))
            {
                return intVal;
            }

            return fallback;
        }

        /// <summary>
        /// Formats integer to hex string #RRGGBB.
        /// </summary>
        public static string IntToHex(int rgbValue)
        {
            return $"#{rgbValue:X6}";
        }
    }
}
