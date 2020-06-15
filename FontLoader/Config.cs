using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace FontLoader.Config
{
    public class FontConfig
    {
        public bool Local { get; set; }
        public string Filename { get; set; }
        public string Code { get; set; }
        public bool LeftToRight { get; set; }
        public float Scale { get; set; }

        public FontConfig(bool local, string filename, string code, bool leftToRight, float scale)
        {
            this.Local = local;
            this.Filename = filename;
            this.Code = code;
            this.LeftToRight = leftToRight;
            this.Scale = scale;
        }
    }

    public class ConfigManager
    {
        private static readonly object _lock = new Object();
        private static ConfigManager instance;
        private readonly string _executingAssemblyPath;
        private readonly string _fileName;

        ConfigManager(string executingAssemblyPath = null, string fileName = "config.json")
        {
            if (executingAssemblyPath == null)
                executingAssemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);

            this._executingAssemblyPath = executingAssemblyPath;
            this._fileName = fileName;
        }

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

        public FontConfig LoadConfigFile()
        {
            FontConfig fc;
            try
            {
                using (var r = new StreamReader(_executingAssemblyPath))
                {
                    var json = r.ReadToEnd();
                    fc = JsonConvert.DeserializeObject<FontConfig>(json);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Load config failure...Excption: {e.Message}");
                Debug.Log($"Use default config.");
                fc = new FontConfig(true, "font", "zh", true, 1);
            }
            return fc;
        }
    }

}