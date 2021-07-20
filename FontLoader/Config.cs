using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace FontLoader.Config
{
    public class FontConfig
    {
        public string Filename { get; set; }
        public string Code { get; set; }
        public bool LeftToRight { get; set; }
        public float Scale { get; set; }

        public FontConfig(string filename, string code, bool leftToRight, float scale)
        {
            this.Filename = filename;
            this.Code = code;
            this.LeftToRight = leftToRight;
            this.Scale = scale;
        }
    }

    public class ConfigManager
    {
        private readonly string ns = MethodBase.GetCurrentMethod().DeclaringType.Namespace;
        private static readonly object _lock = new Object();
        private static ConfigManager instance;

        public static ConfigManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new ConfigManager();
                    }
                    return instance;
                }
            }
        }

        public static string GetRootPath() {
            var location = Assembly.GetExecutingAssembly().Location;
            string rootPath = null;

            if (location.Contains("archived_versions")) 
            {
                rootPath = Directory.GetParent(location).Parent.Parent.FullName;
            }
            else
            {
                rootPath = Directory.GetParent(location).FullName;
            }
            return rootPath;
        }

        public FontConfig LoadConfigFile()
        {
            FontConfig fc;
            try
            {
                using (var r = new StreamReader(Path.Combine(GetRootPath(), "config.json")))
                {
                    var json = r.ReadToEnd();
                    fc = JsonConvert.DeserializeObject<FontConfig>(json);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"[{ns}] Load config failure...Excption: {e.Message}");
                Debug.Log($"[{ns}] Use default config.");
                fc = new FontConfig("font", "zh", true, 1);
            }
            return fc;
        }
    }

}