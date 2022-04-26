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

		private CardClass? _class;
		public CardClass Class => _class ??= (CardClass)Entity.GetTag(CLASS);

		private Rarity? _rarity;
		public Rarity Rarity => _rarity ??= (Rarity)Entity.GetTag(RARITY);

		private CardType? _type;
		public CardType Type => _type ??= (CardType)Entity.GetTag(CARDTYPE);

		private Race? _race;
		public Race Race => _race ??= (Race)Entity.GetTag(CARDRACE);

		private CardSet? _set;
		public CardSet Set
		{
			get
			{
				if (_set == null)
				{
					// HACK to fix missing set value on Hall of Fame cards
					if (new[]
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
						CardIds.Collectible.Priest.ShadowformHOF,
						CardIds.Collectible.Priest.ProphetVelen,
						CardIds.Collectible.Priest.DivineSpirit,
						CardIds.Collectible.Priest.NorthshireCleric,
						CardIds.Collectible.Neutral.AcolyteOfPain,
						CardIds.Collectible.Neutral.Spellbreaker,
						CardIds.Collectible.Neutral.MindControlTech,
						CardIds.Collectible.Neutral.MountainGiant,
						CardIds.Collectible.Neutral.LeeroyJenkins,
					}.Contains(Id))
					{
						_set = CardSet.HOF;
					}
					else
					{
						_set = (CardSet)Entity.GetTag(CARD_SET);
					}
				}
				return _set.Value;
			}
		}

		public Faction Faction => (Faction)Entity.GetTag(FACTION);

		private int? _cost;
		public int Cost => _cost ??= Entity.GetTag(COST);

		private int? _attack;
		public int Attack => _attack ??= Entity.GetTag(ATK);

		private int? _health;
		public int Health => _health ??= Entity.GetTag(HEALTH);

		private int? _durability;
		public int Durability => _durability ??= Entity.GetTag(DURABILITY);

		private int? _armor;
		public int Armor => _armor ??= Entity.GetTag(ARMOR);

		private int? _spellSchool;
		public int SpellSchool => _spellSchool ??= Entity.GetTag(SPELL_SCHOOL);

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

		private bool? _collectible;
		public bool Collectible => _collectible ??= Convert.ToBoolean(Entity.GetTag(COLLECTIBLE));

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

		private bool? _isWild;
		public bool IsWild => _isWild ??= Helper.WildSets.Contains(Set);

		private bool? _isClassic;
		public bool IsClassic => _isClassic ??= Helper.ClassicSets.Contains(Set);

		private int? _techLevel;
		public int TechLevel => _techLevel ??= Entity.GetTag(TECH_LEVEL);

		private bool? _isBaconPoolMinion;
		public bool IsBaconPoolMinion => _isBaconPoolMinion ??= Entity.GetTag(IS_BACON_POOL_MINION) > 0;

		private bool? _taunt;
		public bool Taunt => _taunt ??= Entity.GetTag(TAUNT) > 0;

		private bool? _divineShield;
		public bool DivineShield => _divineShield ??= Entity.GetTag(DIVINE_SHIELD) > 0;

		private bool? _poisonous;
		public bool Poisonous => _poisonous ??= Entity.GetTag(POISONOUS) > 0;

		private bool? _windfury;
		public bool Windfury => _windfury ??= Entity.GetTag(WINDFURY) > 0;

		private bool? _megaWindfury;
		public bool MegaWindfury => _megaWindfury ??= Entity.GetTag(MEGA_WINDFURY) > 0;

		private bool? _cantAttack;
		public bool CantAttack => _cantAttack ??= Entity.GetTag(CANT_ATTACK) > 0;

		private bool? _reborn;
		public bool Reborn => _reborn ??= Entity.GetTag(REBORN) > 0;

		private bool? _deathrattle;
		public bool Deathrattle => _deathrattle ??= (Entity.GetTag(DEATHRATTLE) > 0 || (Entity.GetLocString(CARDTEXT_INHAND, Locale.enUS)?.Contains("Deathrattle:") ?? false));
	}
}
