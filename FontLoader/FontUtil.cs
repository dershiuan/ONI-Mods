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
                if (config.Local)
                {
                    var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets", config.Filename);
                    font = AssetBundle.LoadFromFile(path).LoadAllAssets<TMP_FontAsset>()[0];
                }
                else
                {
                    var path = Path.Combine(Application.streamingAssetsPath, "fonts", config.Filename);
                    font = AssetBundle.LoadFromFile(path).LoadAllAssets<TMP_FontAsset>()[0];
                }
                AssetBundle.UnloadAllAssetBundles(false);

                font.fontInfo.Scale = config.Scale;
            }
            catch (Exception e)
            {
                Debug.Log($"Load font failure...Exception: {e.Message}");
            }

            return font;
        }
    }
}