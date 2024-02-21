using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PractiCod2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }

        private HtmlHelper()
        {
            var jsonContent1 = File.ReadAllText("HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(jsonContent1);
            string jsonContent2 = File.ReadAllText("HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(jsonContent2);
        }
    }
}
