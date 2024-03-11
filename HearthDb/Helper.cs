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
			CardSet.DALARAN, CardSet.ULDUM, CardSet.WILD_EVENT, CardSet.DRAGONS, CardSet.YEAR_OF_THE_DRAGON,
			CardSet.BLACK_TEMPLE, CardSet.DEMON_HUNTER_INITIATE, CardSet.SCHOLOMANCE, CardSet.DARKMOON_FAIRE,
			CardSet.THE_BARRENS, CardSet.WAILING_CAVERNS, CardSet.STORMWIND, CardSet.ALTERAC_VALLEY
		};

		public static CardSet[] ClassicSets = { CardSet.VANILLA };

		public static string[] SpellstoneStrings =
		{
			CardIds.Collectible.Deathknight.LesserSpinelSpellstone,
			CardIds.Collectible.Demonhunter.LesserOpalSpellstone,
			CardIds.Collectible.Druid.LesserJasperSpellstoneInvalid,
			CardIds.Collectible.Druid.LesserJasperSpellstoneLOOTAPALOOZA,
			CardIds.Collectible.Mage.LesserRubySpellstone,
			CardIds.Collectible.Paladin.LesserPearlSpellstone,
			CardIds.Collectible.Priest.LesserDiamondSpellstoneInvalid,
			CardIds.Collectible.Priest.LesserDiamondSpellstoneLOOTAPALOOZA,
			CardIds.Collectible.Rogue.LesserOnyxSpellstone,
			CardIds.Collectible.Shaman.LesserSapphireSpellstone,
			CardIds.Collectible.Warlock.LesserAmethystSpellstoneInvalid,
			CardIds.Collectible.Warlock.LesserAmethystSpellstoneLOOTAPALOOZA,
			CardIds.NonCollectible.Neutral.TheDarkness_TheDarkness
		};
	}
}
