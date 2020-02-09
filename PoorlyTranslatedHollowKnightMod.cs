using HarmonyLib;
using Language;
using Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace PoorlyTranslatedHollowKnight
{
    public class PoorlyTranslatedHollowKnightMod : Mod
    {
        /// <summary>
        /// Represents this Mod's instance.
        /// </summary>
        internal static PoorlyTranslatedHollowKnightMod Instance;

        /// <summary>
        /// Fetches the Mod Version From AssemblyInfo.AssemblyVersion
        /// </summary>
        /// <returns>Mod's Version</returns>
        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        MethodBase switchLanguage = typeof(Language.Language).GetMethod("SwitchLanguage", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new Type[] { typeof(string) }, null);
        MethodBase redirectedSwitchLanguage = typeof(PoorlyTranslatedHollowKnightMod).GetMethod("BadTranslateSwitchLanguage", BindingFlags.Public | BindingFlags.Instance);

        /// <summary>
        /// Called after the class has been constructed.
        /// </summary>
        public override void Initialize() {
            Log("Initializing");

            List<string> availLanguages = ReflectionUtil.GetPrivateStaticField<List<string>>(typeof(Language.Language), "availableLanguages");
            Instance = this;
            var harmony = new Harmony("com.foolishdave.poorlytranslatedhk");
            harmony.PatchAll();

            Log("Initialized");
        }

        
    }
}
