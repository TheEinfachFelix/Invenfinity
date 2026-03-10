using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Models.Part;
using LabelMaker.Templates.Json;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LabelMaker.Services
{
    internal static class Converter
    {
        public static LabelRoot ToLabel(string assetPath, JsonTemplate template, BinDataModel bin, PartDataModel part)
        {
            LabelRoot root = new(bin.SlotLableLength);

            foreach (var element in template.elements)
            {
                string resolvedValue = ReplacePlaceholders(element.value, part);

                switch (element.type)
                {
                    case var _ when element.type == LabelElementImage.Name:
                        var path = Path.Combine(assetPath, element.value.Replace("{", "").Replace("}", "").Trim(), resolvedValue+".svg");
                        var reader = new FileSvgReader(new WpfDrawingSettings());
                        var drawing = reader.Read(path);
                        root.Elements.Add(new LabelElementImage(drawing, element.minWidthMm, element.padding, element.minScale ?? 0.5, element.maxScale));
                        break;
                    case var _ when element.type == LabelElementQrCode.Name:
                        root.Elements.Add(new LabelElementQrCode(resolvedValue, element.minWidthMm, element.padding, element.minScale ?? 0.5, element.maxScale));
                        break;
                    case var _ when element.type == LabelElementText.Name:
                        root.Elements.Add(new LabelElementText(resolvedValue, element.minWidthMm, element.padding, element.minScale ?? 0.5, element.maxScale, element.splitChar));
                        break;
                    default:
                        throw new Exception($"Ungültiger Elementtyp: {element.type}");
                }
            }
            return root;
        }

        private static readonly Regex PlaceholderRegex = new(@"\{(.*?)\}", RegexOptions.Compiled);
        private static string ReplacePlaceholders(string text, PartDataModel part)
        {
            return PlaceholderRegex.Replace(text, match =>
            {
                string propName = match.Groups[1].Value.Trim();
                var prop = typeof(PartDataModel).GetProperty(propName)
                           ?? throw new ArgumentException($"Property '{propName}' nicht gefunden.");

                var value = prop.GetValue(part) ?? throw new Exception($"Property '{propName}' ist null.");
                return value.ToString();
            });
        }

        public static double mmtoUnits(double mm)
        {
            return mm * 96 / 25.4; 
        }
        public static double UnitsToMm(double units)
        {
            return units * 25.4 / 96;
        }
    }
}
