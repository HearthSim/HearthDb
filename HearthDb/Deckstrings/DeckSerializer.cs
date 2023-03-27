using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HearthDb.Enums;

namespace HearthDb.Deckstrings
{
	public class DeckSerializer
	{
		/// <summary>
		/// Serializes given deck to Hearthstone importable deck string.
		/// </summary>
		/// <param name="deck">Deck to be serialized</param>
		/// <param name="includeComments">Adds cards, etc, in human readable form</param>
		/// <exception cref="InvalidDeckException"></exception>
		/// <returns>Deck serialized as a Hearthstone deckstring</returns>
		public static string Serialize(Deck deck, bool includeComments)
		{
			var deckString = Serialize(deck);
			if(!includeComments)
				return deckString;

			var hero = TitleCase(deck.GetHero().Class.ToString());
			var sb = new StringBuilder();
			sb.AppendLine("### " + (string.IsNullOrEmpty(deck.Name) ? hero + " Deck" : deck.Name));
			sb.AppendLine("# Class: " + hero);
			sb.AppendLine("# Format: " + TitleCase(deck.Format.ToString().Substring(3)));
			if(deck.ZodiacYear > 0)
				sb.AppendLine("# Year of the " + TitleCase(deck.ZodiacYear.ToString()));
			sb.AppendLine("#");
			foreach (var card in deck.GetCards().OrderBy(x => x.Key.Cost).ThenBy(x => x.Key.Name))
			{
				sb.AppendLine($"# {card.Value}x ({card.Key.Cost}) {card.Key.Name}");
				if (deck.GetSideboards().TryGetValue(card.Key, out var sideboard))
					foreach (var sideboardCard in sideboard)
						sb.AppendLine($"#   {sideboardCard.Value}x ({sideboardCard.Key.Cost}) {sideboardCard.Key.Name}");
			}
			sb.AppendLine("#");
			sb.AppendLine(deckString);
			sb.AppendLine("#");
			sb.AppendLine("# To use this deck, copy it to your clipboard and create a new deck in Hearthstone");
			return sb.ToString();
		}

		private static string Serialize(Deck deck)
		{
			if(deck == null)
				throw  new InvalidDeckException("Deck can not be null");
			if(deck.HeroDbfId == 0)
				throw new InvalidDeckException("HeroDbfId can not be zero");
			if(deck.GetHero()?.Type != CardType.HERO)
				throw new InvalidDeckException("HeroDbfId does not represent a valid hero");
			if(deck.Format == FormatType.FT_UNKNOWN)
				throw new InvalidDeckException("Format can not be FT_UNKNOWN");

			using(var ms = new MemoryStream())
			{
				void Write(int value)
				{
					if(value == 0)
						ms.WriteByte(0);
					else
					{
						var bytes = VarInt.GetBytes((ulong)value);
						ms.Write(bytes, 0, bytes.Length);
					}
				}

				ms.WriteByte(0);
				Write(1);
				Write((int)deck.Format);
				Write(1);
				Write(deck.HeroDbfId);

				var cards = deck.CardDbfIds.OrderBy(x => x.Key).ToList();
				var singleCopy = cards.Where(x => x.Value == 1).ToList();
				var doubleCopy = cards.Where(x => x.Value == 2).ToList();
				var nCopy = cards.Where(x => x.Value > 2).ToList();

				Write(singleCopy.Count);
				foreach(var card in singleCopy)
					Write(card.Key);

				Write(doubleCopy.Count);
				foreach(var card in doubleCopy)
					Write(card.Key);

				Write(nCopy.Count);
				foreach(var card in nCopy)
				{
					Write(card.Key);
					Write(card.Value);
				}


				var hasSideboard = deck.Sideboards.Values.Any(s => s.Values.Sum() > 0);
				Write(hasSideboard ? 1 : 0);
				if (hasSideboard)
				{
					var sideboardsCards = deck.Sideboards.SelectMany(s =>
						s.Value.Select(x => new { Owner = s.Key, x.Key, x.Value })
					).OrderBy(x => x.Key).ToList();
					var sideboardsSingleCopy = sideboardsCards.Where(x => x.Value == 1).ToList();
					var sideboardsDoubleCopy = sideboardsCards.Where(x => x.Value == 2).ToList();
					var sideboardsNCopy = sideboardsCards.Where(x => x.Value > 2).ToList();

					void WriteSideboardCard(int dbfId, int ownerDbfId, int? qty = null)
					{
						Write(dbfId);
						Write(ownerDbfId);
						if (qty != null)
							Write((int)qty);
					}

					Write(sideboardsSingleCopy.Count);
					foreach (var card in sideboardsSingleCopy)
						WriteSideboardCard(card.Key, card.Owner);

					Write(sideboardsDoubleCopy.Count);
					foreach (var card in sideboardsDoubleCopy)
						WriteSideboardCard(card.Key, card.Owner);

					Write(sideboardsNCopy.Count);
					foreach (var card in sideboardsNCopy)
						WriteSideboardCard(card.Key, card.Owner, card.Value);
				}

				return Convert.ToBase64String(ms.ToArray());
			}
		}

		/// <summary>
		/// Deserializes a given deckstring into a deck object
		/// </summary>
		/// <exception cref="ArgumentException"></exception>
		/// <param name="input">Deckstring to be deserialized</param>
		/// <returns>Deserialized deck object</returns>
		public static Deck Deserialize(string input)
		{
			Deck deck = null;
			var lines = input.Split('\n').Select(x => x.Trim());
			string deckName = null;
			string deckId = null;
			foreach(var line in lines)
			{
				if(string.IsNullOrEmpty(line))
					continue;
				if(line.StartsWith("#"))
				{
					if(line.StartsWith("###"))
						deckName = line.Substring(3).Trim();
					if(line.StartsWith("# Deck ID:"))
						deckId = line.Substring(10).Trim();
					continue;
				}
				if(deck == null)
					deck = DeserializeDeckString(line);
			}
			if(deck != null)
			{
				deck.Name = deckName;
				deck.DeckId = long.TryParse(deckId, out var id) ? id : 0;
			}
			return deck;
		}

		private static Deck DeserializeDeckString(string deckString)
		{
			var deck = new Deck();
			byte[] bytes;
			try
			{
				bytes = Convert.FromBase64String(deckString);
			}
			catch(Exception e)
			{
				throw new ArgumentException("Input is not a valid deck string.", e);
			}
			var offset = 0;
			ulong Read()
			{
				if(offset > bytes.Length)
					throw new ArgumentException("Input is not a valid deck string.");
				var value = VarInt.ReadNext(bytes.Skip(offset).ToArray(), out var length);
				offset += length;
				return value;
			}

			//Zero byte
			offset++;

			//Version - always 1
			Read();

			deck.Format = (FormatType)Read();

			//Num Heroes - always 1
			Read();

			deck.HeroDbfId = (int)Read();

			void AddCard(int? dbfId = null, int count = 1)
			{
				dbfId = dbfId ?? (int)Read();
				deck.CardDbfIds[dbfId.Value] = count;
			}

			var numSingleCards = (int)Read();
			for(var i = 0; i < numSingleCards; i++)
				AddCard();

			var numDoubleCards = (int)Read();
			for(var i = 0; i < numDoubleCards; i++)
				AddCard(count: 2);

			var numMultiCards = (int)Read();
			for(var i = 0; i < numMultiCards; i++)
			{
				var dbfId = (int)Read();
				var count = (int)Read();
				AddCard(dbfId, count);
			}

			var hasSideboards = (int)Read();
			if (hasSideboards == 1)
			{
				void AddSideboardCard(int? dbfId = null, int count = 1)
				{
					dbfId = dbfId ?? (int)Read();
					var ownerDbfId = (int)Read();
					if (!deck.Sideboards.ContainsKey(ownerDbfId))
						deck.Sideboards[ownerDbfId] = new Dictionary<int, int>();
					deck.Sideboards[ownerDbfId][dbfId.Value] = count;
				}

				var numSingleSideboardCards = (int)Read();
				for (var i = 0; i < numSingleSideboardCards; i++)
					AddSideboardCard();

				var numDoubleSideboardCards = (int)Read();
				for (var i = 0; i < numDoubleSideboardCards; i++)
					AddSideboardCard(count: 2);

				var numMultiSideboardCards = (int)Read();
				for (var i = 0; i < numMultiSideboardCards; i++)
				{
					var dbfId = (int)Read();
					var count = (int)Read();
					AddSideboardCard(dbfId, count);
				}
			}

			return deck;
		}

		private static string TitleCase(string str)
		{
			if(string.IsNullOrEmpty(str))
				return string.Empty;
			if(str.Length == 1)
				return str.ToUpperInvariant();
			return str.Substring(0, 1).ToUpperInvariant() + str.Substring(1, str.Length - 1).ToLowerInvariant();
		}
	}
}
