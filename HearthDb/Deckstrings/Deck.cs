using System.Collections.Generic;
using System.Linq;
using HearthDb.Enums;

namespace HearthDb.Deckstrings
{
	public class Deck
	{
		/// <summary>
		/// DbfId of the hero. Required.
		/// This can be a specific hero for a class, i.e. Medivh over Jaina.
		/// </summary>
		public int HeroDbfId { get; set; }

		/// <summary>
		/// Dictionary of (DbfId, Count) for each card.
		/// Needs to be a total of 30 cards to be accepted by Hearthstone.
		/// </summary>
		public Dictionary<int, int> CardDbfIds { get; set; } = new Dictionary<int, int>();

		/// <summary>
		/// Dictionary of (owner DbfId, Dictionary of (DbfId, Count)) for each card in each sideboard.
		/// </summary>
		public Dictionary<int, Dictionary<int, int>> Sideboards { get; set; } = new Dictionary<int, Dictionary<int, int>>();

		/// <summary>
		/// Format of the deck. Required.
		/// </summary>
		public FormatType Format { get; set; }

		/// <summary>
		/// Year of the deck format. Optional.
		/// </summary>
		public ZodiacYear ZodiacYear { get; set; }

		/// <summary>
		/// Name of the deck. Optional.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Hearthstones internal ID of the deck. Optional.
		/// </summary>
		public long DeckId { get; set; }

		/// <summary>
		/// Gets the card object for the given HeroDbfId
		/// </summary>
		public Card GetHero() => Cards.GetFromDbfId(HeroDbfId);

		/// <summary>
		/// Converts (DbfId, Count) dictionary to (CardObject, Count).
		/// </summary>
		public Dictionary<Card, int> GetCards() => CardDbfIds
			.Select(x => new { Card = Cards.GetFromDbfId(x.Key), Count = x.Value })
			.Where(x => x.Card != null).ToDictionary(x => x.Card, x => x.Count);

		/// <summary>
		/// Converts (OwnerDbfId, (DbfId, Count)) dictionary to (OwnerCardObject, (CardObject, Count)).
		/// </summary>
		public Dictionary<Card, Dictionary<Card, int>> GetSideboards() => Sideboards
			.Select(x => new
			{
				Owner = Cards.GetFromDbfId(x.Key),
				Sideboard = x.Value.Select(s => new
				{
					Card = Cards.GetFromDbfId(s.Key),
					Count = s.Value
				}).Where(s => s.Card != null).ToDictionary(x => x.Card, x => x.Count)
			})
			.Where(x => x.Owner != null).ToDictionary(x => x.Owner, x => x.Sideboard);
	}
}
