using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using JiraWorkLogUploader.Ui;
using Newtonsoft.Json;

namespace JiraWorkLogUploader.Config
{
    public class AppSettings
    {
        [Editor(typeof(ExcelFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ExcelFile { get; set; }

        [TypeConverter(typeof(GenericEnumerableConverter))]
        public string SheetName { get; set; }
        public JiraSetting[] Jiras { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public IEnumerable<string> ItemsFor_SheetName { get; set; }

        public static AppSettings From(string fileName)
        {
            var jsonText = File.ReadAllText(fileName);
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(jsonText);
            return appSettings;
        }

        public void SaveTo(string fileName)
        {
            var jsonText = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(fileName, jsonText);
        }
    }
}
