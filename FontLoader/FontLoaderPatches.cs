using System;
using System.IO;
using System.Reflection;
using System.Text;
using FontLoader.Config;
using FontLoader.Utils;
using Harmony;
using TMPro;
using UnityEngine;

namespace FontLoader
{
    public class FontLoaderPatches
    {
        private static readonly string ns = MethodBase.GetCurrentMethod().DeclaringType.Namespace;
        private static FontConfig fc;
        private static TMP_FontAsset font;

        private static void SwapFont(string target)
        {
            LocText[] locTexts = Resources.FindObjectsOfTypeAll<LocText>();
            Func<LocText, bool> condition = tmp => tmp != null && tmp.font.name != font.name;
            Action<LocText> action = tmp => tmp.font = font;

            if (locTexts != null)
            {
                Debug.Log($"{ns} {target} Swaping LocText font.");
                locTexts.DoIf(condition, action);
            }
            else
                Debug.Log($"{ns} {target} No LocTexts need to swap.");
        }

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                Debug.Log($"{ns} OnLoad.");

                fc = ConfigManager.Instance.LoadConfigFile();
                font = FontUtil.LoadFontAsset(fc);
                if (font != null)
                    TMP_Settings.fallbackFontAssets.Add(font);
            }
        }

        [HarmonyPatch(typeof(Localization))]
        [HarmonyPatch(nameof(Localization.LoadLocalTranslationFile))]
        public static class Localization_LoadLocalTranslationFile_Patch
        {
            public static bool Prefix(Localization.SelectedLanguageType source, string path)
            {
                try
                {
                    Debug.Log($"{ns} Swap Localization font to {font.name}.");
                    var lines = Localization.ExtractTranslatedStrings(File.ReadAllLines(path, Encoding.UTF8), false);
                    Localization.OverloadStrings(lines);
                    Localization.SwapToLocalizedFont(font.name);
                    return false;
                }
                catch (Exception ex)
                {
                    DebugUtil.LogWarningArgs(new object[] { ex });
                    return true;
                }
            }
        }


        [HarmonyPatch(typeof(MainMenu))]
        [HarmonyPatch("OnPrefabInit")]
        public static class MainMenu_OnPrefabInit_Patch
        {
            public static void Prefix()
            {
                try
                {
                    SwapFont("MainMenu");
                }
                catch (Exception ex)
                {
                    DebugUtil.LogWarningArgs(new object[] { ex });
                }
            }
        }

        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch("OnPrefabInit")]
        public static class Game_OnPrefabInit_Patch
        {
            public static void Prefix()
            {
                try
                {
                    SwapFont("Game");
                }
                catch (Exception ex)
                {
                    DebugUtil.LogWarningArgs(new object[] { ex });
                }
            }
        }

        [HarmonyPatch(typeof(NameDisplayScreen))]
        [HarmonyPatch(nameof(NameDisplayScreen.AddNewEntry))]
        public static class NameDisplayScreen_AddNewEntry_Patch
        {
            public static void Postfix(NameDisplayScreen __instance, GameObject representedObject)
            {
                var targetEntry = __instance.entries.Find(entry => entry.world_go == representedObject);
                if (targetEntry != null && targetEntry.display_go != null)
                {
                    var txt = targetEntry.display_go.GetComponentInChildren<LocText>();
                    if (txt != null && txt.font.name != font.name)
                        txt.font = font;
                }

            }
        }

        [HarmonyPatch(typeof(CodexScreen))]
        [HarmonyPatch("OnActivate")]
        public static class Game_OnActivate_Patch
        {
            public static void Prefix()
            {
                try
                {
                    SwapFont("CodexScreen");
                }
                catch (Exception ex)
                {
                    DebugUtil.LogWarningArgs(new object[] { ex });
                }
            }
        }

    }
}
