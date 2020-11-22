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
			CardSet.PROMO,
			CardSet.HOF,
			CardSet.NAXX,
			CardSet.GVG,
			CardSet.BRM,
			CardSet.LOE,
			CardSet.TGT,
			CardSet.OG,
			CardSet.KARA,
			CardSet.GANGS
		};

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
