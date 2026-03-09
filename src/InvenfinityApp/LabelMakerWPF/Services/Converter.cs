using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Models.Part;
using LabelMaker.Templates.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Services
{
    internal static class Converter
    {
        public static LabelRoot ToLabel(string AssetPath, JsonTemplate template, BinDataModel bin, PartDataModel Part)
        {
            LabelRoot root = new(bin.SlotLableLength);
            foreach (var element in template.elements)
            {
                string name = element.value;
                string cleandedValue = element.value.Trim().Replace("{", "").Replace("}", "");
                if (name.StartsWith("{") && name.EndsWith("}"))
                {
                    var Prop = typeof(PartDataModel).GetProperty(cleandedValue) ?? throw new ArgumentException("invallide value");
                    var value = Prop.GetValue(Part) ?? throw new Exception("not found");
                    name = value.ToString() ?? throw new Exception("something went wrong");
                }

                switch (element.type)
                {
                    case var _ when element.type == LabelElementImage.Name:
                        root.Elements.Add(new LabelElementImage(AssetPath, cleandedValue, name, element.widthMm, element.padding));
                        break;
                    case var _ when element.type == LabelElementQrCode.Name:
                        root.Elements.Add(new LabelElementQrCode(name,  element.widthMm, element.padding));
                        break;
                    case var _ when element.type == LabelElementText.Name:
                        root.Elements.Add(new LabelElementText(name, element.widthMm, element.padding));
                        break;
                    default:
                        throw new Exception("invallide Type");
                }

            }
            return root;
        }
        public static double mmtoUnits(double mm)
        {
            return mm * 96 / 25.4; 
        }
    }
}
