#region

using System.Linq;
using HearthStone.Database.CardDefs;
using HearthStone.Database.Enums;

#endregion

namespace HearthStone.Database
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

		public CardClass Class => (CardClass)Entity.GetTag(GameTag.CLASS);

		public Rarity Rarity => (Rarity)Entity.GetTag(GameTag.RARITY);

		public CardType Type => (CardType)Entity.GetTag(GameTag.CARDTYPE);

		public Race Race => (Race)Entity.GetTag(GameTag.CARDRACE);

		public CardSet Set => (CardSet)Entity.GetTag(GameTag.CARD_SET);

		public Faction Faction => (Faction)Entity.GetTag(GameTag.FACTION);

		public int Cost => Entity.GetTag(GameTag.COST);

		public int Attack => Entity.GetTag(GameTag.ATK);

		public int Health => Entity.GetTag(GameTag.HEALTH);

		public int Durability => Entity.GetTag(GameTag.DURABILITY);

		public int Armor => Entity.GetTag(GameTag.ARMOR);

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

		public string ArtistName => Entity.GetInnerValue(GameTag.ARTISTNAME);

		public string[] EntourageCardIds => Entity.EntourageCards.Select(x => x.CardId).ToArray();

		public Locale DefaultLanguage { get; set; } = Locale.enUS;

		public bool Collectible => Entity.GetTag(GameTag.COLLECTIBLE) != 0;

		public string GetLocName(Locale lang) => Entity.GetLocString(GameTag.CARDNAME, lang);

		public string GetLocText(Locale lang)
		{
			var text = Entity.GetLocString(GameTag.CARDTEXT_INHAND, lang)?.Replace("_", "\u00A0").Trim();
			if(text == null)
				return null;
			var index = text.IndexOf('@');
			return index > 0 ? text.Substring(index + 1) : text;
		}

		public string GetLocFlavorText(Locale lang) => Entity.GetLocString(GameTag.FLAVORTEXT, lang);
	}
}
