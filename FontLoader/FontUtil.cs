using System;
using System.IO;
using System.Reflection;
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
            try
            {
                if (config.local)
                {
                    var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets", config.filename);
                    font = AssetBundle.LoadFromFile(path).LoadAllAssets<TMP_FontAsset>()[0];
                }
                else
                {
                    var path = Path.Combine(Application.streamingAssetsPath, "fonts", config.filename);
                    font = AssetBundle.LoadFromFile(path).LoadAllAssets<TMP_FontAsset>()[0];
                }
                AssetBundle.UnloadAllAssetBundles(false);

            }
            catch (Exception e)
            {
                Debug.Log($"Load font failure...Exception: {e.Message}");
            }

            return font;
        }
    }
}