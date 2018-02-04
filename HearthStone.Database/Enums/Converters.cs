using System.Linq;


namespace HearthStone.Database.Enums
{
	public static class Converters
	{
		public static BnetGameType GetBnetGameType(GameType gameType, FormatType format)
		{
			switch(gameType)
			{
				case GameType.GT_UNKNOWN:
					return BnetGameType.BGT_UNKNOWN;
				case GameType.GT_VS_AI:
					return BnetGameType.BGT_VS_AI;
				case GameType.GT_VS_FRIEND:
					return BnetGameType.BGT_FRIENDS;
				case GameType.GT_TUTORIAL:
					return BnetGameType.BGT_TUTORIAL;
				case GameType.GT_ARENA:
					return BnetGameType.BGT_ARENA;
				case GameType.GT_TEST:
					return BnetGameType.BGT_TEST1;
				case GameType.GT_RANKED:
					return format == FormatType.FT_STANDARD ? BnetGameType.BGT_RANKED_STANDARD : BnetGameType.BGT_RANKED_WILD;
				case GameType.GT_CASUAL:
					return format == FormatType.FT_STANDARD? BnetGameType.BGT_CASUAL_STANDARD : BnetGameType.BGT_CASUAL_WILD;
				case GameType.GT_TAVERNBRAWL:
					return BnetGameType.BGT_TAVERNBRAWL_PVP;
				case GameType.GT_TB_1P_VS_AI:
					return BnetGameType.BGT_TAVERNBRAWL_1P_VERSUS_AI;
				case GameType.GT_TB_2P_COOP:
					return BnetGameType.BGT_TAVERNBRAWL_2P_COOP;
				case GameType.GT_FSG_BRAWL:
					return BnetGameType.BGT_FSG_BRAWL_VS_FRIEND;
				case GameType.GT_FSG_BRAWL_1P_VS_AI:
					return BnetGameType.BGT_FSG_BRAWL_1P_VERSUS_AI;
				case GameType.GT_FSG_BRAWL_2P_COOP:
					return BnetGameType.BGT_FSG_BRAWL_2P_COOP;
				case GameType.GT_FSG_BRAWL_VS_FRIEND:
					return BnetGameType.BGT_FSG_BRAWL_VS_FRIEND;
				default:
					return BnetGameType.BGT_UNKNOWN;
			}
		}

		public static bool IsBrawl(GameType gameType)
		{
			return new[]
			{
				GameType.GT_TAVERNBRAWL,
				GameType.GT_TB_1P_VS_AI,
				GameType.GT_TB_2P_COOP,
				GameType.GT_FSG_BRAWL,
				GameType.GT_FSG_BRAWL_1P_VS_AI,
				GameType.GT_FSG_BRAWL_2P_COOP,
				GameType.GT_FSG_BRAWL_VS_FRIEND
			}.Contains(gameType);
		}
	}
}
