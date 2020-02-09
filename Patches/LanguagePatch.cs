using HarmonyLib;
using Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace PoorlyTranslatedHollowKnight.Patches
{
    [HarmonyPatch(typeof(Language.Language))]
    [HarmonyPatch("SwitchLanguage", new[] { typeof(string) })]
    class LanguagePatchSwitchLanguage
    {
        static bool Prefix(string langCode, ref bool __result)
        {
            PoorlyTranslatedHollowKnightMod.Instance.Log("Language changed to " + langCode);
            __result = BadTranslateSwitchLanguage(langCode);
            return false;
        }

        static bool BadTranslateSwitchLanguage(string code)
        {
            if (Platform.Current.IsPlayerPrefsLoaded)
            {
                Platform.Current.SharedData.SetString("M2H_lastLanguage", code);
                Platform.Current.SharedData.Save();
            }

            ReflectionUtil.SetPrivateStaticField(typeof(Language.Language), "currentLanguage", LanguageCode.EN);
            ReflectionUtil.SetPrivateStaticField(typeof(Language.Language), "currentEntrySheets", new Dictionary<string, Dictionary<string, string>>());
            Dictionary<string, Dictionary<string, string>> entrySheets = ReflectionUtil.GetPrivateStaticField<Dictionary<string, Dictionary<string, string>>>(typeof(Language.Language), "currentEntrySheets");
            PoorlyTranslatedHollowKnightMod.Instance.Log("Loading sheets.");
            foreach (string text in Language.Language.settings.sheetTitles)
            {
                entrySheets[text] = new Dictionary<string, string>();
                string languageFileContents = File.ReadAllText(Application.dataPath + "/Managed/Mods/BadTranslation/BAD_" + text + ".txt");
                try
                {
                    if (languageFileContents != string.Empty)
                    {
                        using (XmlReader xmlReader = XmlReader.Create(new StringReader(languageFileContents)))
                        {
                            while (xmlReader.ReadToFollowing("entry"))
                            {
                                xmlReader.MoveToFirstAttribute();
                                string value = xmlReader.Value;
                                xmlReader.MoveToElement();
                                string text2 = xmlReader.ReadElementContentAsString().Trim();
                                text2 = text2.UnescapeXML();
                                entrySheets[text][value] = text2;
                            }
                        }
                    }
                } catch (Exception e)
                {
                    PoorlyTranslatedHollowKnightMod.Instance.LogError("Threw exception reading xml: " + text + ".\n" + e.Message);
                }
            }
            PoorlyTranslatedHollowKnightMod.Instance.Log("Done loading sheets.");
            ReflectionUtil.SetPrivateStaticField(typeof(Language.Language), "currentEntrySheets", entrySheets);
            LocalizedAsset[] array = (LocalizedAsset[])UnityEngine.Object.FindObjectsOfType(typeof(LocalizedAsset));
            foreach (LocalizedAsset localizedAsset in array)
            {
                localizedAsset.LocalizeAsset();
            }
            try
            {
                ReflectionUtil.InvokePrivateStaticMethod(typeof(Language.Language), "SendMonoMessage", new object[] { "ChangedLanguage", new[] { code } });
            } catch (Exception e)
            {
                PoorlyTranslatedHollowKnightMod.Instance.LogError("Error in emitting mono message reflectively. " + e.Message);
            }

            PoorlyTranslatedHollowKnightMod.Instance.Log("Done loading in language.");
            return true;
        }
    }

    [HarmonyPatch(typeof(Language.Language))]
    [HarmonyPatch("SwitchLanguage", new[] { typeof(LanguageCode) })]
    class LanguagePatchSwitchLanguageCode
    {
        static bool Prefix(LanguageCode code, ref bool __result)
        {
            PoorlyTranslatedHollowKnightMod.Instance.Log("Language changed to " + code);
            __result = BadTranslateSwitchLanguage(code + String.Empty);
            return false;
        }

        static bool BadTranslateSwitchLanguage(string code)
        {
            if (Platform.Current.IsPlayerPrefsLoaded)
            {
                Platform.Current.SharedData.SetString("M2H_lastLanguage", code);
                Platform.Current.SharedData.Save();
            }

            ReflectionUtil.SetPrivateStaticField(typeof(Language.Language), "currentLanguage", LanguageCode.EN);
            ReflectionUtil.SetPrivateStaticField(typeof(Language.Language), "currentEntrySheets", new Dictionary<string, Dictionary<string, string>>());
            Dictionary<string, Dictionary<string, string>> entrySheets = ReflectionUtil.GetPrivateStaticField<Dictionary<string, Dictionary<string, string>>>(typeof(Language.Language), "currentEntrySheets");
            PoorlyTranslatedHollowKnightMod.Instance.Log("Loading sheets.");
            foreach (string text in Language.Language.settings.sheetTitles)
            {
                entrySheets[text] = new Dictionary<string, string>();
                string languageFileContents = File.ReadAllText(Application.dataPath + "/Managed/Mods/BadTranslation/BAD_" + text + ".txt");
                try
                {
                    if (languageFileContents != string.Empty)
                    {
                        using (XmlReader xmlReader = XmlReader.Create(new StringReader(languageFileContents)))
                        {
                            while (xmlReader.ReadToFollowing("entry"))
                            {
                                xmlReader.MoveToFirstAttribute();
                                string value = xmlReader.Value;
                                xmlReader.MoveToElement();
                                string text2 = xmlReader.ReadElementContentAsString().Trim();
                                text2 = text2.UnescapeXML();
                                entrySheets[text][value] = text2;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    PoorlyTranslatedHollowKnightMod.Instance.LogError("Threw exception reading xml: " + text + ".\n" + e.Message);
                }
            }
            PoorlyTranslatedHollowKnightMod.Instance.Log("Done loading sheets.");
            ReflectionUtil.SetPrivateStaticField(typeof(Language.Language), "currentEntrySheets", entrySheets);
            LocalizedAsset[] array = (LocalizedAsset[])UnityEngine.Object.FindObjectsOfType(typeof(LocalizedAsset));
            foreach (LocalizedAsset localizedAsset in array)
            {
                localizedAsset.LocalizeAsset();
            }
            try
            {
                ReflectionUtil.InvokePrivateStaticMethod(typeof(Language.Language), "SendMonoMessage", new object[] { "ChangedLanguage", new[] { code } });
            }
            catch (Exception e)
            {
                PoorlyTranslatedHollowKnightMod.Instance.LogError("Error in emitting mono message reflectively. " + e.Message);
            }

            PoorlyTranslatedHollowKnightMod.Instance.Log("Done loading in language.");
            return true;
        }
    }
}
