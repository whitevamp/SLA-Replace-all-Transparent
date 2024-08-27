using System;
using System.Linq;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using System.Collections.Generic;
//
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Environments;
using CommandLine;
using Noggog;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using DynamicData.Kernel;
using System.Reflection;
using System.Text.RegularExpressions;
using Mutagen.Bethesda.Plugins.Records;

namespace BaboKeywordPatcher
{
   public class BaboSettings
    {
        public bool ArmorPrettyDefault { get; set; }
        public bool ArmorEroticDefault { get; set; }
        public bool EroticDresses { get; set; }
        public HashSet<ModKey> ModsToPatch { get; set; } = new HashSet<ModKey>();
        public HashSet<ModKey> ModsToNotPatch { get; set; } = new HashSet<ModKey>();
    }

    public class Program
    {
        public static Lazy<BaboSettings> _settings = null!;
        public static BaboSettings Settings => _settings.Value;

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                /* .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetAutogeneratedSettings(
                    nickname: "Settings",
                    path: "settings.json",
                    out _settings, true)
                    //createIfMissing: true)
                .SetTypicalOpen(GameRelease.SkyrimSE, "BaboKeywords.esp")
                .Run(args); */
				.AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "BaboKeywords.esp")
                .SetAutogeneratedSettings(
                            nickname: "Settings",
                            path: "settings.json",
                            out _settings,
                            true)
                .Run(args);
        }

        public static IKeywordGetter LoadKeyword(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, string kwd)
        {
            if (!state.LinkCache.TryResolve<IKeywordGetter>(kwd, out var returnKwd))
            {
                throw new Exception($"Failed to load keyword: {kwd}");
            }
            return returnKwd;
        }

        public static bool StrMatch(string name, string comparator)
        {
            return name.IndexOf(comparator, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool StrMatchCS(string name, string comparator)
        {
            return name.IndexOf(comparator) >= 0;
        }

        public static bool IsDeviousRenderedItem(string name)
        {
            return StrMatch(name, "scriptinstance") || StrMatch(name, "rendered");
        }

        public static void LoadKeywords(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            // Load all keywords here
            // Example:
            SLA_ArmorHarness = LoadKeyword(state, "SLA_ArmorHarness");

            try
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpendex");
            }
            catch
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpandex");
            }

            SLA_ArmorTransparent = LoadKeyword(state, "SLA_ArmorTransparent");
            SLA_BootsHeels = LoadKeyword(state, "SLA_BootsHeels");
            SLA_VaginalDildo = LoadKeyword(state, "SLA_VaginalDildo");
            SLA_AnalPlug = LoadKeyword(state, "SLA_AnalPlug");
            SLA_PiercingClit = LoadKeyword(state, "SLA_PiercingClit");
            SLA_PiercingNipple = LoadKeyword(state, "SLA_PiercingNipple");
            SLA_ArmorPretty = LoadKeyword(state, "SLA_ArmorPretty");
            EroticArmor = LoadKeyword(state, "EroticArmor");
            SLA_ArmorBondage = LoadKeyword(state, "SLA_ArmorBondage");
            SLA_AnalPlugTail = LoadKeyword(state, "SLA_AnalPlugTail");
            SLA_AnalBeads = LoadKeyword(state, "SLA_AnalPlugBeads");
            SLA_VaginalBeads = LoadKeyword(state, "SLA_VaginalBeads");
            SLA_ArmorRubber = LoadKeyword(state, "SLA_ArmorRubber");
            // Not in SexLabAroused Redux V28b SSE Modified by BakaFactory(2020 11 17)
			//SLA_BraArmor = LoadKeyword(state, "SLA_BraArmor");
            SLA_ThongT = LoadKeyword(state, "SLA_ThongT");
            SLA_PantiesNormal = LoadKeyword(state, "SLA_PantiesNormal");
            SLA_HasLeggings = LoadKeyword(state, "SLA_HasLeggings");
            SLA_HasStockings = LoadKeyword(state, "SLA_HasStockings");
            SLA_MiniSkirt = LoadKeyword(state, "SLA_MiniSkirt");
            SLA_ArmorHalfNakedBikini = LoadKeyword(state, "SLA_ArmorHalfNakedBikini");
			//Added Keywords. From SexLabAroused Redux V28b SSE Modified by BakaFactory(2020 11 17)
			SLA_FullSkirt = LoadKeyword(state, "SLA_FullSkirt");
            SLA_ArmorCurtain = LoadKeyword(state, "SLA_ArmorCurtain");
            SLA_ArmorPartBottom = LoadKeyword(state, "SLA_ArmorPartBottom");
            SLA_HasSleeves = LoadKeyword(state, "SLA_HasSleeves");
            SLA_MicroSkirt = LoadKeyword(state, "SLA_MicroSkirt");
            SLA_Brabikini = LoadKeyword(state, "SLA_Brabikini");
            SLA_ArmorHalfNaked = LoadKeyword(state, "SLA_ArmorHalfNaked");
            SLA_ArmorFemaleOnly = LoadKeyword(state, "SLA_ArmorFemaleOnly");
            SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpendex");
            SLA_PiercingBelly = LoadKeyword(state, "SLA_PiercingBelly");
            SLA_PiercingVulva = LoadKeyword(state, "SLA_PiercingVulva");
            SLA_ThongGstring = LoadKeyword(state, "SLA_ThongGstring");
            SLA_MicroHotpants = LoadKeyword(state, "SLA_MicroHotpants");
            SLA_PantsNormal = LoadKeyword(state, "SLA_PantsNormal");
            SLA_ArmorIllegal = LoadKeyword(state, "SLA_ArmorIllegal");
            SLA_KillerHeels = LoadKeyword(state, "SLA_KillerHeels");
            SLA_ThongCString = LoadKeyword(state, "SLA_ThongCString");
            SLA_ThongLowleg = LoadKeyword(state, "SLA_ThongLowleg");
            SLA_ArmorHarness = LoadKeyword(state, "SLA_ArmorHarness");
            SLA_Earrings = LoadKeyword(state, "SLA_Earrings");
            SLA_PiercingNose = LoadKeyword(state, "SLA_PiercingNose");
            SLA_PiercingLips = LoadKeyword(state, "SLA_PiercingLips");
            SLA_ShowgirlSkirt = LoadKeyword(state, "SLA_ShowgirlSkirt");
            SLA_PelvicCurtain = LoadKeyword(state, "SLA_PelvicCurtain");
            SLA_ArmorPartTop = LoadKeyword(state, "SLA_ArmorPartTop");
            SLA_ArmorLewdLeotard = LoadKeyword(state, "SLA_ArmorLewdLeotard");
            SLA_ImpossibleClothes = LoadKeyword(state, "SLA_ImpossibleClothes");
            SLA_ArmorCapeMini = LoadKeyword(state, "SLA_ArmorCapeMini");
            SLA_ArmorCapeFull = LoadKeyword(state, "SLA_ArmorCapeFull");
            SLA_PastiesNipple = LoadKeyword(state, "SLA_PastiesNipple");
            SLA_PastiesCrotch = LoadKeyword(state, "SLA_PastiesCrotch");
        }

        private static void AddTag(Armor armorEditObj, IKeywordGetter tag)
        {
            if (armorEditObj.Keywords == null)
            {
                armorEditObj.Keywords = new ExtendedList<IFormLinkGetter<IKeywordGetter>>();
            }

            if (!armorEditObj.Keywords.Contains(tag))
            {
                armorEditObj.Keywords.Add(tag);
            }
        }

        public static void ParseName(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, IArmorGetter armor, string name)
        {
            bool matched = false;
            var armorEditObj = state.PatchMod.Armors.GetOrAddAsOverride(armor);

            if (armorEditObj == null)
            {
                Console.WriteLine($"Armor is null for {name}");
                return;
            }

            if (StrMatch(name, "harness") || StrMatch(name, "corset") || StrMatch(name, "StraitJacket") ||
                StrMatch(name, "hobble") || StrMatch(name, "tentacles") ||
                StrMatch(name, "slave") || StrMatch(name, "chastity") || StrMatch(name, "cuff") || StrMatch(name, "binder") ||
                StrMatch(name, "yoke") || StrMatch(name, "mitten"))
            {
                matched = true;
                AddTag(armorEditObj, SLA_ArmorTransparent);
            }

            // Additional parsing rules here...

            if (Settings.ArmorPrettyDefault && !matched && (StrMatch(name, "armor") || StrMatch(name, "cuiras") || StrMatch(name, "robes")))
            {
                matched = true;
                AddTag(armorEditObj, SLA_ArmorPretty);
            }
            else if (Settings.ArmorEroticDefault && !matched && (StrMatch(name, "armor") || StrMatch(name, "cuiras") || StrMatch(name, "robes")))
            {
                matched = true;
                AddTag(armorEditObj, EroticArmor);
            }

            if (matched)
            {
                state.PatchMod.Armors.Set(armorEditObj);
            }
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
			HashSet<string> MASTER_MODS = new HashSet<string>()
            {
                "SexLabAroused.esm",
            };
			
            var modsToPatch = Settings.ModsToPatch;
            var modsToNotPatch = Settings.ModsToNotPatch;

            if (modsToNotPatch.Any())
            {
                Console.WriteLine($"Blacklist:\n{string.Join("\n", modsToNotPatch)}");
            }

            var shortenedLoadOrder = modsToPatch.Any() 
                ? state.LoadOrder.PriorityOrder.Where(mod => modsToPatch.Contains(mod.ModKey) && !modsToNotPatch.Contains(mod.ModKey)).ToList()
                : state.LoadOrder.PriorityOrder.Where(mod => !modsToNotPatch.Contains(mod.ModKey)).ToList();
				//add
            /* Console.WriteLine($"Found mods:\n{string.Join("\n", shortenedLoadOrder.Reverse())}");
            var shortenedLoadOrderFuller = modsToPatch.Any() ? state.LoadOrder.ListedOrder.Where(mod =>
                modsToPatch.Contains(mod.ModKey) || MASTER_MODS.Contains(mod.ModKey.ToString())
                ) : state.LoadOrder.ListedOrder;
				
			var idLinkCache = shortenedLoadOrderFuller.ToImmutableLinkCache<ISkyrimMod, ISkyrimModGetter>(LinkCachePreferences.Default);

            var consts = new UDImportantConstantsFound(Settings.IMPORTANTCONSTANTS, idLinkCache);

            Console.WriteLine($"===========================Starting patching==========================="); */

            LoadKeywords(state);

            foreach (var armor in shortenedLoadOrder.SelectMany(mod => mod.Mod?.Armors?.Where(a => a.Name != null)))
            {
                ParseName(state, armor, armor.Name!.ToString());
            }
        }

        // Keyword variables (placeholders)
        public static IKeywordGetter? EroticArmor;
        public static IKeywordGetter? SLA_ArmorHarness;
        public static IKeywordGetter? SLA_ArmorSpendex;
        public static IKeywordGetter? SLA_ArmorTransparent;
        public static IKeywordGetter? SLA_BootsHeels;
        public static IKeywordGetter? SLA_VaginalDildo;
        public static IKeywordGetter? SLA_AnalPlug;
        public static IKeywordGetter? SLA_PiercingClit;
        public static IKeywordGetter? SLA_PiercingNipple;
        public static IKeywordGetter? SLA_ArmorPretty;
        public static IKeywordGetter? SLA_ArmorBondage;
        public static IKeywordGetter? SLA_AnalPlugTail;
        public static IKeywordGetter? SLA_AnalBeads;
        public static IKeywordGetter? SLA_VaginalBeads;
        public static IKeywordGetter? SLA_ArmorRubber;
        // Not in SexLabAroused Redux V28b SSE Modified by BakaFactory(2020 11 17)
		//public static IKeywordGetter? SLA_BraArmor;
        public static IKeywordGetter? SLA_ThongT;
        public static IKeywordGetter? SLA_PantiesNormal;
        public static IKeywordGetter? SLA_HasLeggings;
        public static IKeywordGetter? SLA_HasStockings;
        public static IKeywordGetter? SLA_MiniSkirt;
        public static IKeywordGetter? SLA_ArmorHalfNakedBikini;
		//Added Keywords. From SexLabAroused Redux V28b SSE Modified by BakaFactory(2020 11 17)
		public static IKeywordGetter? SLA_PastiesNipple;
		public static IKeywordGetter? SLA_PastiesCrotch;
		public static IKeywordGetter? SLA_ShowgirlSkirt;
		public static IKeywordGetter? SLA_PelvicCurtain;
		public static IKeywordGetter? SLA_ArmorPartTop;
		public static IKeywordGetter? SLA_ArmorLewdLeotard;
		public static IKeywordGetter? SLA_ImpossibleClothes;
		public static IKeywordGetter? SLA_ArmorCapeMini;
		public static IKeywordGetter? SLA_ArmorCapeFull;
		public static IKeywordGetter? SLA_ArmorHarness;
		public static IKeywordGetter? SLA_Earrings;
		public static IKeywordGetter? SLA_PiercingNose;
		public static IKeywordGetter? SLA_PiercingLips;
		public static IKeywordGetter? SLA_PiercingBelly;
		public static IKeywordGetter? SLA_PiercingVulva;
		public static IKeywordGetter? SLA_ThongGstring;
		public static IKeywordGetter? SLA_MicroHotpants;
		public static IKeywordGetter? SLA_PantsNormal;
		public static IKeywordGetter? SLA_ArmorIllegal;
		public static IKeywordGetter? SLA_KillerHeels;
		public static IKeywordGetter? SLA_ThongCString;
		public static IKeywordGetter? SLA_ThongLowleg;
		public static IKeywordGetter? SLA_ArmorFemaleOnly;
		public static IKeywordGetter? SLA_ArmorSpendex;
		public static IKeywordGetter? SLA_Brabikini;
		public static IKeywordGetter? SLA_ArmorHalfNaked;
		public static IKeywordGetter? SLA_HasSleeves;
		public static IKeywordGetter? SLA_MicroSkirt;
		public static IKeywordGetter? SLA_FullSkirt;
		public static IKeywordGetter? SLA_ArmorCurtain;
		public static IKeywordGetter? SLA_ArmorPartBottom;
    }
}
