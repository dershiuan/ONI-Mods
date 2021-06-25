using System;
using System.Reflection;
using FontLoader.Config;
using FontLoader.Utils;
using HarmonyLib;
using TMPro;

namespace FontLoader
{
    public class FontLoaderPatches : KMod.UserMod2
    {
        private static readonly string ns = MethodBase.GetCurrentMethod().DeclaringType.Namespace;
        private static FontConfig fc;
        private static TMP_FontAsset font;
        
        public override void OnLoad(Harmony harmony)
        {
            Debug.Log($"{ns} OnLoad.");

            fc = ConfigManager.Instance.LoadConfigFile();
            font = FontUtil.LoadFontAsset(fc);

            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Localization))]
        [HarmonyPatch(nameof(Localization.GetLocale))]
        [HarmonyPatch(new Type[] { typeof(string[]) })]
        public static class Localization_GetLocale_Patch
        {
            public static void Postfix(ref Localization.Locale __result)
            {
                try
                {
                    if (font != null)
                    {
                        var Language = fc.Code == "zh" ? Localization.Language.Chinese : Localization.Language.Unspecified;
                        var Direction = fc.LeftToRight ? Localization.Direction.LeftToRight : Localization.Direction.RightToLeft;
                        __result = new Localization.Locale(Language, Direction, fc.Code, font.name);
                    }
                }
                catch (Exception ex)
                {
                    DebugUtil.LogWarningArgs(new object[] { ex });
                }
            }
        }

    }
}
