#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HearthDb.CardDefs;
using HearthDb.Enums;
using static HearthDb.Enums.GameTag;

#endregion

namespace HearthDb
{
	public class Card
	{
		internal Card(Entity entity)
		{
			Entity = entity;
		}

		public Entity Entity { get; }

		public string Id => Entity.CardId;

		public int DbfId => Entity.DbfId;

		public string Name => GetLocName(DefaultLanguage);

		public string Text => GetLocText(DefaultLanguage);

		public string FlavorText => GetLocFlavorText(DefaultLanguage);

		public CardClass Class => (CardClass)Entity.GetTag(CLASS);

		public Rarity Rarity => (Rarity)Entity.GetTag(RARITY);

		public CardType Type => (CardType)Entity.GetTag(CARDTYPE);

		public Race Race => (Race)Entity.GetTag(CARDRACE);

		public CardSet Set
		{
			get
			{
				// HACK to fix missing set value on Hall of Fame cards
				if(new[]
				{
					CardIds.Collectible.Mage.IceBlock,
					CardIds.Collectible.Neutral.ColdlightOracle,
					CardIds.Collectible.Neutral.MoltenGiant,

					//2019
					CardIds.Collectible.Druid.Naturalize,
					CardIds.Collectible.Warlock.Doomguard,
					CardIds.Collectible.Paladin.DivineFavor,
					CardIds.Collectible.Neutral.BakuTheMooneater,
					CardIds.Collectible.Neutral.GennGreymane,
					CardIds.Collectible.Druid.GloomStag,
					CardIds.Collectible.Mage.BlackCat,
					CardIds.Collectible.Priest.GlitterMoth,
					CardIds.Collectible.Shaman.MurksparkEel,

					//2020
					CardIds.Collectible.Priest.AuchenaiSoulpriest,
					CardIds.Collectible.Priest.HolyFire,
					CardIds.Collectible.Priest.Shadowform,
					CardIds.Collectible.Priest.ProphetVelen,
					CardIds.Collectible.Priest.DivineSpirit,
					CardIds.Collectible.Priest.NorthshireCleric,
					CardIds.Collectible.Neutral.AcolyteOfPain,
					CardIds.Collectible.Neutral.Spellbreaker,
					CardIds.Collectible.Neutral.MindControlTech,
					CardIds.Collectible.Neutral.MountainGiant,
					CardIds.Collectible.Neutral.LeeroyJenkins,
				}.Contains(Id))
					return CardSet.HOF;
				return (CardSet)Entity.GetTag(CARD_SET);
			}
		}

		private static Dictionary<string, string> SpellstoneStrings = new Dictionary<string, string>() { {"LOOT_043", "GAMEPLAY_AMETHYST_SPELLSTONE_%d" },
			{ "LOOT_051", "GAMEPLAY_JASPER_SPELLSTONE_%d" },
			{ "LOOT_064", "GAMEPLAY_SAPPHIRE_SPELLSTONE_%d" },
			{ "LOOT_091", "GAMEPLAY_PEARL_SPELLSTONE_%d" },
			{ "LOOT_103", "GAMEPLAY_RUBY_SPELLSTONE_%d" },
			{ "LOOT_503", "GAMEPLAY_ONYX_SPELLSTONE_%d"},
			{ "LOOT_507", "GAMEPLAY_DIAMOND_SPELLSTONE_%d"},
			{ "LOOT_526d", "GAMEPLAY_LOOT_526d_DARKNESS_%d"} };

		private Regex atCounterRegex = new Regex(@"\|4\(([^,)]+),\s*([^,)]+)\)");

		private static Regex ProgressRegex = new Regex(@"\(@\/\d+\)"); // => Helper.cs

		public Faction Faction => (Faction)Entity.GetTag(FACTION);

		public int Cost => Entity.GetTag(COST);

		public int Attack => Entity.GetTag(ATK);

		public int Health => Entity.GetTag(HEALTH);

		public int Durability => Entity.GetTag(DURABILITY);

		public int Armor => Entity.GetTag(ARMOR);

		public string[] Mechanics
		{
			get
			{
				var mechanics = Dictionaries.Mechanics.Keys.Where(mechanic => Entity.GetTag(mechanic) > 0).Select(x => Dictionaries.Mechanics[x]);
				var refMechanics =
					Dictionaries.ReferencedMechanics.Keys.Where(mechanic => Entity.GetReferencedTag(mechanic) > 0)
								.Select(x => Dictionaries.ReferencedMechanics[x]);
				return mechanics.Concat(refMechanics).ToArray();
			}
		}

		public string ArtistName => Entity.GetInnerValue(ARTISTNAME);

		public string[] EntourageCardIds => Entity.EntourageCards.Select(x => x.CardId).ToArray();

		public Locale DefaultLanguage { get; set; } = Locale.enUS;

		public bool Collectible => Convert.ToBoolean(Entity.GetTag(COLLECTIBLE));

		public string GetLocName(Locale lang) => Entity.GetLocString(CARDNAME, lang);

		public string GetLocText(Locale lang)
		{
			var text = Entity.GetLocString(CARDTEXT_INHAND, lang)?.Replace("_", "\u00A0").Trim();
			if(text == null)
				return null;
			var count = text.Contains("@");

			if (!count && !text.Contains("|4"))
				return text;

			int num = 0;

			if (SpellstoneStrings.ContainsKey(Entity.CardId))
				return text.Replace("@", "");

			if (Entity.GetTag(TAG_SCRIPT_DATA_NUM_1) != 0)
				num = Entity.GetTag(TAG_SCRIPT_DATA_NUM_1);
			else if (Entity.GetTag(SCORE_VALUE_1) != 0)
				num = Entity.GetTag(SCORE_VALUE_1);
			if (num != 0 || ProgressRegex.IsMatch(text))
				text = text.Replace("@", num.ToString());

			var atCounterMatch = atCounterRegex.Match(text);
			if (atCounterMatch.Success)
			{
				var replacement = num == 1 ? atCounterMatch.Groups[0].Value : atCounterMatch.Groups[1].Value;
				text = text.Substring(0, atCounterMatch.Index) + replacement + text.Substring(atCounterMatch.Index + atCounterMatch.Length);
			}

			var parts = text.Split('@');
			if (parts.Count() >= 2) {
				text = parts[0];
				text = text.Trim();
			}

			return text;

		}

		public string GetLocFlavorText(Locale lang) => Entity.GetLocString(FLAVORTEXT, lang);

		public bool IsWild => Helper.WildSets.Contains(Set);
	}
}
