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

		public CardSet Set => (CardSet)Entity.GetTag(CARD_SET);

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
					Dictionaries.ReferencedMechanics.Keys.Where(mechanic => Entity.GetTag(mechanic) > 0)
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
			return index > 0 ? text.Substring(index + 1) : text;
		}

		public string GetLocFlavorText(Locale lang) => Entity.GetLocString(FLAVORTEXT, lang);
	}
}
