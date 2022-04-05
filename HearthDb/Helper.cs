using HearthDb.Enums;
using System.Text.RegularExpressions;

namespace HearthDb
{
	public static class Helper
	{
		public static Regex AtCounterRegex = new Regex(@"\|4\(([^,)]+),\s*([^,)]+)\)");

		public static Regex ProgressRegex = new Regex(@"\(@\/\d+\)");

		public static CardSet[] WildSets =
		{
			CardSet.BRM, CardSet.LOE, CardSet.TGT, CardSet.HOF,
			CardSet.FP1, CardSet.PE1, CardSet.PROMO,
			CardSet.KARA, CardSet.OG, CardSet.GANGS,
			CardSet.UNGORO, CardSet.ICECROWN, CardSet.LOOTAPALOOZA,
			CardSet.GILNEAS, CardSet.BOOMSDAY, CardSet.TROLL,
			CardSet.DEMON_HUNTER_INITIATE,
		};

		public static CardSet[] ClassicSets = { CardSet.VANILLA };

		public static string[] SpellstoneStrings =
		{
			CardIds.Collectible.Druid.LesserJasperSpellstone,
			CardIds.Collectible.Mage.LesserRubySpellstone,
			CardIds.Collectible.Paladin.LesserPearlSpellstone,
			CardIds.Collectible.Priest.LesserDiamondSpellstone,
			CardIds.Collectible.Rogue.LesserOnyxSpellstone,
			CardIds.Collectible.Shaman.LesserSapphireSpellstone,
			CardIds.Collectible.Warlock.LesserAmethystSpellstone,
			CardIds.NonCollectible.Neutral.TheDarkness_TheDarkness
		};
	}
}
