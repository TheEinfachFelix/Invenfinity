using LabelMaker.Models.Bin;
using LabelMaker.Models.Label;
using LabelMaker.Models.Part;
using LabelMaker.Rendering;
using LabelMaker.Services;
using LabelMaker.Templates.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelMaker
{
    public class LabelMakerControll
    {
        public string AssetPath { get; set; }
        public LabelMakerControll(string assetPath) 
        { 
            this.AssetPath = assetPath;
        }

        public DrawingImage test(string path, BinDataModel bin, PartDataModel part, int height)
        {
            var data = JsonTemplateLoader.LoadJson(path);

            LabelRoot label = Converter.ToLabel(AssetPath, data, bin, part);

            LabelMakerControll var = new(AssetPath);
            return new DrawingImage(label.BuildVector(height));

        }
    }
}
