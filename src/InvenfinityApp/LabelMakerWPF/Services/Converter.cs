using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Models.Part;
using LabelMaker.Templates.Json;
using LabelMakerWPF.Models.Label.Elements;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace LabelMaker.Services
{
    internal static class Converter
    {
        public static PartLabelRoot ToLabel(string assetPath, JsonTemplate template, BinDataModel bin)
        {
            PartLabelRoot root = new(bin.TotalLableLength);
            string newPath = Path.Combine(assetPath, template.requirements.AssetType);
            List<ILabelElement> list = [];
            foreach (var part in bin.Parts)
            {
                list.AddRange(toLabelElements(template.partElement, bin, part, newPath));
            }
            root.elements = list;
            
            return root;
        }
        public static List<ILabelElement> toLabelElements(List<LayoutItem> items, BinDataModel bin, PartDataModel part, string assetPath)
        {
            List<ILabelElement> outp = [];
            foreach (var element in items)
            {
                string resolvedValue = ReplacePlaceholders(element.value, part);

                switch (element.type)
                {
                    case var _ when element.type == LabelElementImage.Name:
                        var path = Path.Combine(assetPath, part.Typename + "_" + resolvedValue + ".svg");
                        var reader = new FileSvgReader(new WpfDrawingSettings());
                        var drawing = reader.Read(path);
                        outp.Add(LabelElementImage.GenerateElement(element, drawing));
                        break;
                    case var _ when element.type == LabelElementQrCode.Name:
                        outp.Add(LabelElementQrCode.GenerateElement(element, resolvedValue));
                        break;
                    case var _ when element.type == LabelElementText.Name:
                        outp.Add(LabelElementText.GenerateElement(element, resolvedValue));
                        break;
                    case var _ when element.type == LabelElementGroupe.Name:
                        outp.Add(LabelElementGroupe.GenerateElement(element, bin, part, assetPath));
                        break;
                    case var _ when element.type == LabelElementStack.Name:
                        outp.Add(LabelElementStack.GenerateElement(element, bin, part, assetPath));
                        break;
                    case var _ when element.type == LabelElementPart.Name:
                        outp.Add(LabelElementPart.GenerateElement(element));
                        break;
                    default:
                        throw new Exception($"Ungültiger Elementtyp: {element.type}");
                }
            }
            return outp;
        }

        private static readonly Regex PlaceholderRegex = new(@"\{(.*?)\}", RegexOptions.Compiled);
        private static string ReplacePlaceholders(string? text, PartDataModel part)
        {
            if (text == null) return "";
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
