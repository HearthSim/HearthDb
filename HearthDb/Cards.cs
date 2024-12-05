#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using HearthDb.CardDefs;
using HearthDb.Enums;

#endregion

namespace HearthDb
{
	public static class Cards
	{
		public static readonly Dictionary<string, Card> All = new Dictionary<string, Card>();
		public static readonly Dictionary<int, Card> AllByDbfId = new Dictionary<int, Card>();

		public static readonly Dictionary<string, Card> Collectible = new Dictionary<string, Card>();
		public static readonly Dictionary<int, Card> CollectibleByDbfId = new Dictionary<int, Card>();

		public static readonly Dictionary<string, Card> BaconPoolMinions = new Dictionary<string, Card>();
		public static readonly Dictionary<int, Card> BaconPoolMinionsByDbfId = new Dictionary<int, Card>();

		public static readonly Dictionary<string, string> NormalToTripleCardIds = new Dictionary<string, string>();
		public static readonly Dictionary<string, string> TripleToNormalCardIds = new Dictionary<string, string>();

		public static readonly Dictionary<int, int> NormalToTripleDbfIds = new Dictionary<int, int>();
		public static readonly Dictionary<int, int> TripleToNormalDbfIds = new Dictionary<int, int>();

		private static readonly HashSet<string> IgnoreTripleIds = new HashSet<string>
		{
			CardIds.NonCollectible.Priest.GhastcoilerTROLL,
			CardIds.NonCollectible.Mage.GlyphGuardianTROLL,
			CardIds.NonCollectible.Neutral.SeabreakerGoliathGILNEAS
		};

		static Cards()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HearthDb.CardDefs.xml");
			if(stream == null)
				return;
			using(TextReader tr = new StreamReader(stream))
			{
				var xml = new XmlSerializer(typeof(CardDefs.CardDefs));
				var cardDefs = (CardDefs.CardDefs)xml.Deserialize(tr);
				var baconTriples = new List<(Card, int)>();
				var nonBaconTriples = new List<(Card, int)>();
				foreach(var entity in cardDefs.Entites)
				{
					// For some reason Deflect-o-bot is missing divine shield
					if (IsDeflectOBot(entity) && !entity.Tags.Any(x => x.EnumId == (int)GameTag.DIVINE_SHIELD))
						entity.Tags.Add(new Tag { EnumId = (int)GameTag.DIVINE_SHIELD, Value = 1 });

					var card = new Card(entity);
					All.Add(entity.CardId, card);
					AllByDbfId.Add(entity.DbfId, card);
					if (card.Collectible && (card.Type != CardType.HERO || card.Set != CardSet.HERO_SKINS))
					{
						Collectible.Add(entity.CardId, card);
						CollectibleByDbfId.Add(entity.DbfId, card);
					}

					if (card.IsBaconPoolMinion)
					{
						BaconPoolMinions.Add(entity.CardId, card);
						BaconPoolMinionsByDbfId.Add(entity.DbfId, card);
					}

					if (!IgnoreTripleIds.Contains(entity.CardId))
					{
						var tripleDbfId = card.Entity.Tags.FirstOrDefault(x => x.EnumId == 1429);
						if (tripleDbfId != null)
						{
							if(card.IsBaconPoolMinion)
								baconTriples.Add((card, tripleDbfId.Value));
							else
								nonBaconTriples.Add((card, tripleDbfId.Value));
						}
					}
				}
				
				// Triples have to be resolved after the first loop since we need to look up the triple card from the id
				// Loop over non-bacon first in case both contain a mapping to the same card.
				// We want to use the bacon one in that case.
				foreach (var (card, tripleDbfId) in nonBaconTriples.Concat(baconTriples))
				{
					if (!AllByDbfId.TryGetValue(tripleDbfId, out var triple))
						continue;
					NormalToTripleCardIds[card.Id] = triple.Id;
					NormalToTripleDbfIds[card.DbfId] = triple.DbfId;
					TripleToNormalCardIds[triple.Id] = card.Id;
					TripleToNormalDbfIds[triple.DbfId] = card.DbfId;
				}
			}
		}

		/// <summary>
		/// Will try to return the card ID of the triple. Returns the normal card id if it cannot be found.
		/// </summary>
		public static string TryGetTripleId(string normalCardId)
		{
			return NormalToTripleCardIds.TryGetValue(normalCardId, out var triple) ? triple : normalCardId;
		}

		public static Card GetFromName(string name, Locale lang, bool collectible = true)
			=> (collectible ? Collectible : All).Values.FirstOrDefault(x => x.GetLocName(lang)?.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false);

		public static Card GetFromDbfId(int dbfId, bool collectible = false)
			=> (collectible ? CollectibleByDbfId : AllByDbfId).TryGetValue(dbfId, out var card) ? card : null;

		private static bool IsDeflectOBot(Entity entity) => entity.CardId == CardIds.NonCollectible.Neutral.DeflectOBot || entity.CardId == CardIds.NonCollectible.Neutral.DeflectOBotTavernBrawl;
	}
}