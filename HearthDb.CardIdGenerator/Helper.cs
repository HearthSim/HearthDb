#region

using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using static HearthDb.Enums.CardSet;

#endregion

namespace HearthDb.CardIdGenerator
{
	internal class Helper
	{
		internal static readonly Dictionary<string, string> SpecialPrefixes = new Dictionary<string, string>
		{
			{"NAX1h_01", "Heroic"},
			{"NAX1h_03", "Heroic"},
			{"NAX1h_04", "Heroic"},
			{"BRMA09_2t", "BRS"},
			{"BRMA09_2Ht", "BRS"},
			{"BRMA13_3", "BWL"},
			{"BRMA13_3H", "BWL"},
			{"BRMA17_2", "HiddenLab"},
			{"BRMA17_2H", "HiddenLab"},
			{"LOEA01_01", "Temple"},
			{"LOEA02_01", "Temple"},
			{"LOEA02_01h", "Temple"},
			{"LOEA04_27", "Uldaman"},
			{"LOEA05_01", "Uldaman"},
			{"LOEA08_01", "TRC"},
			{"LOEA08_01h", "TRC"},
			{"LOEA09_1", "TRC"},
			{"LOEA09_1H", "TRC"},
			{"LOEA10_1", "Hall"},
			{"LOEA10_1H", "Hall"},
			{"LOEA12_1", "Hall"},
			{"LOEA12_1H", "Hall"},
			{"LOEA13_1", "Hall"},
			{"LOEA13_1H", "Hall"},
			{"LOEA14_1", "LOEA14"},
			{"LOEA14_1H", "LOEA14"},
			{"LOEA15_1", "LOEA15"},
			{"LOEA15_1H", "LOEA15"},
			{"LOEA16_1", "LOEA16"},
			{"LOEA16_17", "LOEA16"},
			{"LOEA16_18", "LOEA16"},
			{"LOEA16_18H", "LOEA16"},
			{"LOEA16_19", "LOEA16"},
			{"LOEA16_19H", "LOEA16"},
			{"LOEA16_1H", "LOEA16"},
			{"LOEA16_21", "LOEA16"},
			{"LOEA16_21H", "LOEA16"},
			{"LOEA16_22", "LOEA16"},
			{"LOEA16_22H", "LOEA16"},
			{"LOEA16_23", "LOEA16"},
			{"LOEA16_23H", "LOEA16"},
			{"LOEA16_24", "LOEA16"},
			{"LOEA16_24H", "LOEA16"},
			{"LOEA16_25", "LOEA16"},
			{"LOEA16_25H", "LOEA16"},
			{"LOEA16_26", "LOEA16"},
			{"LOEA16_26H", "LOEA16"},
			{"LOEA16_27", "LOEA16"},
			{"LOEA16_27H", "LOEA16"}
		};

		internal static string GetSetName(CardSet set)
		{
			switch(set)
			{
				case CardSet.CORE:
					return "Basic";
				case CardSet.EXPERT1:
					return "Classic";
				case CardSet.PROMO:
					return "Promotion";
				case CardSet.FP1:
					return "CurseOfNaxxramas";
				case CardSet.PE1:
					return "GoblinsVsGnomes";
				case CardSet.BRM:
					return "BlackrockMountain";
				case CardSet.TGT:
					return "TheGrandTournament";
				case CardSet.LOE:
					return "LeagueOfExplorers";
				default:
					return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(set.ToString().ToLower());
			}
		}

		public static string GetSetAbbreviation(Enums.CardSet set)
		{
			switch(set)
			{
				case FP1:
					return "NAXX";
				case PE1:
					return "GVG";
				case TGT:
					return "TGT";
				case EXPERT1:
					return "Expert1";
				case MISSIONS:
					return "Missions";
				case INVALID:
					return "Invalid";
				case TEST_TEMPORARY:
					return "Test";
				case DEMO:
					return "Demo";
				case NONE:
					return "None";
				case CHEAT:
					return "Cheat";
				case BLANK:
					return "Blank";
				case DEBUG_SP:
					return "Debug";
				case PROMO:
					return "Promo";
				case CREDITS:
					return "Credits";
				case HERO_SKINS:
					return "HeroSkins";
				case TB:
					return "TavernBrawl";
				case SLUSH:
					return "Slush";
				default:
					return set.ToString();
			}
		}

		public static string GetBaseId(string id)
		{
			if(string.IsNullOrEmpty(id))
				return id;
			if(id.StartsWith("LOE_019t"))
				return "LOE_079";
			var match = Regex.Match(id, @"(?<base>(.*_\d+[hH]?)).*");
			return match.Success ? match.Groups["base"].Value : id;
		}
	}
}