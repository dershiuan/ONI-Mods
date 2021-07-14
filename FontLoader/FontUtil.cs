using System;
using System.IO;
using FontLoader.Config;
using TMPro;
using UnityEngine;

namespace FontLoader.Utils
{
    public static class FontUtil
    {
        public static TMP_FontAsset LoadFontAsset(FontConfig config)
        {
            TMP_FontAsset font = null;
            AssetBundle ab = null;

            try
            {
                var path = Path.Combine(ConfigManager.Instance.configPath, "Assets", config.Filename);
                ab = AssetBundle.LoadFromFile(path);

                if (ab == null) {
                    Debug.LogWarning("[FontLoader] Unable to load font asset.");
                    return null;
                }
                
                font = ab.LoadAllAssets<TMP_FontAsset>()[0];
                font.fontInfo.Scale = config.Scale;
            }
            catch (Exception e)
            {
                Debug.LogError($"[FontLoader] {e.Message}");
            }

            AssetBundle.UnloadAllAssetBundles(false);
            return font;
        }
    }
}