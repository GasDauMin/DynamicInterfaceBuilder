using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace DynamicInterfaceBuilder.Core.Helpers
{
    public static class TypeConversionHelper
    {
        /// <summary>
        /// Converts a value to the specified target type with special handling for WPF types
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="targetType">The target type to convert to</param>
        /// <returns>The converted value or null if conversion failed</returns>
        public static object? ConvertValueToType(object value, Type targetType)
        {
            try
            {
                if (value == null)
                {
                    return null;
                }

                // Handle WPF Thickness
                if (targetType == typeof(Thickness) && value is string thicknessStr)
                {
                    return ParseThickness(thicknessStr);
                }
                // Handle WPF SolidColorBrush
                else if (targetType == typeof(SolidColorBrush) && value is string colorStr)
                {
                    return ParseBrush(colorStr);
                }
                // Handle WPF Color
                else if (targetType == typeof(Color) && value is string colorValueStr)
                {
                    return ParseColor(colorValueStr);
                }
                // Handle WPF FontWeight
                else if (targetType == typeof(FontWeight) && value is string fontWeightStr)
                {
                    return ParseFontWeight(fontWeightStr);
                }
                // Handle WPF FontFamily
                else if (targetType == typeof(FontFamily) && value is string fontFamilyStr)
                {
                    return new FontFamily(fontFamilyStr);
                }
                // Handle WPF HorizontalAlignment
                else if (targetType == typeof(HorizontalAlignment) && value is string hAlignStr)
                {
                    return Enum.Parse<HorizontalAlignment>(hAlignStr, true);
                }
                // Handle WPF VerticalAlignment
                else if (targetType == typeof(VerticalAlignment) && value is string vAlignStr)
                {
                    return Enum.Parse<VerticalAlignment>(vAlignStr, true);
                }
                // Handle Enums
                else if (targetType.IsEnum && value is string enumStr)
                {
                    return Enum.Parse(targetType, enumStr, true);
                }
                // Handle standard type conversion
                else
                {
                    return Convert.ChangeType(value, targetType);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error converting value {value} to type {targetType.Name}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Parses a string into a Thickness object
        /// </summary>
        /// <param name="thicknessStr">Comma-separated thickness values (1, 2 or 4 values)</param>
        /// <returns>A Thickness object</returns>
        public static Thickness ParseThickness(string thicknessStr)
        {
            var values = thicknessStr.Split(',')
                .Select(s => double.TryParse(s, out var v) ? v : 0)
                .ToArray();

            return values.Length switch
            {
                1 => new Thickness(values[0]),
                2 => new Thickness(values[0], values[1], values[0], values[1]),
                4 => new Thickness(values[0], values[1], values[2], values[3]),
                _ => new Thickness(0)
            };
        }

        /// <summary>
        /// Parses a string into a SolidColorBrush
        /// </summary>
        /// <param name="colorStr">Color string (name or hex)</param>
        /// <returns>A SolidColorBrush</returns>
        public static SolidColorBrush ParseBrush(string colorStr)
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(colorStr);
                return new SolidColorBrush(color);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine($"Invalid color format: {colorStr}");
                return Brushes.Black;
            }
        }

        /// <summary>
        /// Parses a string into a Color
        /// </summary>
        /// <param name="colorStr">Color string (name or hex)</param>
        /// <returns>A Color object</returns>
        public static Color ParseColor(string colorStr)
        {
            try
            {
                return (Color)ColorConverter.ConvertFromString(colorStr);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine($"Invalid color format: {colorStr}");
                return Colors.Black;
            }
        }

        /// <summary>
        /// Parses a string into a FontWeight
        /// </summary>
        /// <param name="fontWeightStr">Font weight string</param>
        /// <returns>A FontWeight object</returns>
        public static FontWeight ParseFontWeight(string fontWeightStr)
        {
            try
            {
                return (FontWeight)new System.Windows.FontWeightConverter().ConvertFromString(fontWeightStr)!;
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine($"Invalid font weight: {fontWeightStr}");
                return FontWeights.Normal;
            }
        }
    }
} 