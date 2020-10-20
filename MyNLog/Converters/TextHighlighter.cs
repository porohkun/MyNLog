using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace MyNLog.Converters
{
    [System.Windows.Markup.ContentProperty(nameof(Brushes))]
    public class TextHighlighter : IValueConverter
    {
        static Brush DefaultBrush = new SolidColorBrush(Colors.Magenta);

        public string Pattern { get; set; }
        public List<Brush> Brushes { get; set; } = new List<Brush>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string input) || string.IsNullOrWhiteSpace(input))
                return null;

            var regex = new Regex(Pattern);
            var matches = regex.Matches(input);

            var parts = new List<ColoredPart>() { new ColoredPart() { BrushIndex = 0, Index = 0, Length = input.Length } };

            foreach (Match match in matches)
            {
                for (int g = 0; g < match.Groups.Count; g++)
                {
                    var group = match.Groups[g];
                    var part = new ColoredPart() { BrushIndex = g, Index = group.Index, Length = group.Length };

                    if (parts.Count == 0)
                        parts.Add(part);
                    else
                    {
                        for (int i = 0; i < parts.Count; i++)
                        {
                            var current = parts[i];
                            if (part.Index <= current.Index)
                            {
                                parts.Insert(i, part);
                                break;
                            }
                            else if (part.Index < current.Index + current.Length)
                            {
                                var next = new ColoredPart() { BrushIndex = current.BrushIndex, Index = part.Index, Length = current.Length - (part.Index - current.Index) };
                                current.Length = part.Index - current.Index;
                                parts.Insert(i + 1, part);
                                parts.Insert(i + 2, next);
                                break;
                            }
                            else if (parts.Count == i + 1)
                            {
                                parts.Add(part);
                                break;
                            }
                        }

                        var clearedParts = new List<ColoredPart>();
                        {
                            ColoredPart current = null;
                            foreach (var next in parts)
                            {
                                if (current != null && next.Index < current.Index + current.Length)
                                {
                                    next.Length -= current.Length - (next.Index - current.Index);
                                    if (next.Length <= 0)
                                        continue;
                                    next.Index = current.Index + current.Length;
                                }
                                current = next;
                                clearedParts.Add(current);
                            }
                        }

                        parts = clearedParts;
                    }
                }
            }

            return parts.Select(p => new Run(input.Substring(p.Index, p.Length)) { Foreground = GetBrush(p.BrushIndex) }).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        Brush GetBrush(int index)
        {
            if (Brushes.Count == 0)
                return DefaultBrush;
            if (Brushes.Count > index)
                return Brushes[index];
            return Brushes[0];
        }

        class ColoredPart
        {
            public int Index;
            public int Length;
            public int BrushIndex;
        }
    }
}
