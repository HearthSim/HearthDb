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
			if(string.IsNullOrEmpty(text))
				return null;

			if (!text.Contains("@") && !text.Contains("|4"))
				return text;

			if (Helper.SpellstoneStrings.Contains(Entity.CardId))
				return text.Replace("@", "");

			var num = Entity.GetTag(TAG_SCRIPT_DATA_NUM_1);
			if (num == 0)
				num = Entity.GetTag(SCORE_VALUE_1);
			if (num != 0 || Helper.ProgressRegex.IsMatch(text))
				text = text.Replace("@", num.ToString());

			var atCounterMatch = Helper.AtCounterRegex.Match(text);
			if (atCounterMatch.Success)
			{
				var replacement = num == 1 ? atCounterMatch.Groups[1].Value : atCounterMatch.Groups[2].Value;
				text = text.Substring(0, atCounterMatch.Index) + replacement + text.Substring(atCounterMatch.Index + atCounterMatch.Length);
			}

			var parts = text.Split('@');
			if (parts.Count() >= 2)
				text = parts[0].Trim();

			return text;

		}

		public string GetLocFlavorText(Locale lang) => Entity.GetLocString(FLAVORTEXT, lang);

		public bool IsWild => Helper.WildSets.Contains(Set);
	}
}
