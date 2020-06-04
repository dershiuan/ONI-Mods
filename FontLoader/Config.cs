using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace FontLoader.Config
{
    public class FontConfig
    {
        public bool local { get; set; }
        public string filename { get; set; }

        public FontConfig(bool local, string path)
        {
            this.local = local;
            this.filename = filename;
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
            FontConfig config;
            try
            {
                using (var r = new StreamReader(_executingAssemblyPath))
                {
                    var json = r.ReadToEnd();
                    config = JsonConvert.DeserializeObject<FontConfig>(json);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Load config failure...Excption: {e.Message}");
                Debug.Log($"Use default config.");
                config = new FontConfig(true, "font");
            }
            return config;
        }
    }
}