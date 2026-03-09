using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabelMaker.Templates.Json
{
    internal static class JsonTemplateLoader
    {
        public static JsonTemplate LoadJson(string path)
        {
            // Load File
            var data = File.ReadAllText(path);
            // Load Json
            return JsonConvert.DeserializeObject<JsonTemplate>(data) ?? throw new Exception("Could not parse Json");
        }
    }
}
