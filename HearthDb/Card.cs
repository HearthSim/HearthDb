#region

using System;
using System.Linq;
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
			if(text == null)
				return null;
			var index = text.IndexOf('@');
			if (index == 0)
				return text;
			if (Entity.GetTag(PLAYER_TAG_THRESHOLD_TAG_ID) > 0)
				return text.Substring(0, index);
			var scriptData = Entity.GetTag(TAG_SCRIPT_DATA_NUM_1);
			if (scriptData > 0)
				return text.Replace("@", scriptData.ToString());
			return text.Substring(index + 1);
		}

		public string GetLocFlavorText(Locale lang) => Entity.GetLocString(FLAVORTEXT, lang);

		public bool IsWild => Helper.WildSets.Contains(Set);
	}
}
