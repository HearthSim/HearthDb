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
		public Card GetHero() => Cards.GetFromDbfId(HeroDbfId, false);

		/// <summary>
		/// Converts (DbfId, Count) dictionary to (CardObject, Count).
		/// </summary>
		public Dictionary<Card, int> GetCards() => CardDbfIds
			.Select(x => new { Card = Cards.GetFromDbfId(x.Key), Count = x.Value })
			.Where(x => x.Card != null).ToDictionary(x => x.Card, x => x.Count);
	}
}
