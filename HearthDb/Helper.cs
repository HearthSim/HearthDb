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
			CardSet.LEGACY,
			CardSet.BRM, CardSet.LOE, CardSet.TGT, CardSet.HOF,
			CardSet.FP1, CardSet.PE1, CardSet.PROMO,
			CardSet.KARA, CardSet.OG, CardSet.GANGS,
			CardSet.UNGORO, CardSet.ICECROWN, CardSet.LOOTAPALOOZA,
			CardSet.GILNEAS, CardSet.BOOMSDAY, CardSet.TROLL,
			CardSet.DALARAN, CardSet.ULDUM, CardSet.WILD_EVENT, CardSet.DRAGONS, CardSet.YEAR_OF_THE_DRAGON,
			CardSet.BLACK_TEMPLE, CardSet.DEMON_HUNTER_INITIATE, CardSet.SCHOLOMANCE, CardSet.DARKMOON_FAIRE,
			CardSet.THE_BARRENS, CardSet.WAILING_CAVERNS, CardSet.STORMWIND, CardSet.ALTERAC_VALLEY,
			CardSet.THE_SUNKEN_CITY, CardSet.REVENDRETH, CardSet.RETURN_OF_THE_LICH_KING, CardSet.PATH_OF_ARTHAS,
			CardSet.WONDERS,
		};

		public static CardSet[] ClassicSets = { CardSet.VANILLA };

		public static string[] SpellstoneStrings =
		{
			CardIds.Collectible.Deathknight.LesserSpinelSpellstone,
			CardIds.Collectible.Demonhunter.LesserOpalSpellstone,
			CardIds.Collectible.Druid.LesserJasperSpellstoneCore,
			CardIds.Collectible.Druid.LesserJasperSpellstone,
			CardIds.Collectible.Mage.LesserRubySpellstone,
			CardIds.Collectible.Paladin.LesserPearlSpellstone,
			CardIds.Collectible.Priest.LesserDiamondSpellstoneCore,
			CardIds.Collectible.Priest.LesserDiamondSpellstone,
			CardIds.Collectible.Rogue.LesserOnyxSpellstone,
			CardIds.Collectible.Shaman.LesserSapphireSpellstone,
			CardIds.Collectible.Warlock.LesserAmethystSpellstoneCore,
			CardIds.Collectible.Warlock.LesserAmethystSpellstone,
			CardIds.NonCollectible.Neutral.TheDarkness_TheDarkness
		};
	}
}
