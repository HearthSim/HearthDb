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
		public static Dictionary<string, Card> All { get; private set; } = new Dictionary<string, Card>();
		public static Dictionary<int, Card> AllByDbfId { get; private set; } = new Dictionary<int, Card>();

		public static Dictionary<string, Card> Collectible { get; private set; } = new Dictionary<string, Card>();
		public static Dictionary<int, Card> CollectibleByDbfId { get; private set; } = new Dictionary<int, Card>();

		public static Dictionary<string, Card> BaconPoolMinions { get; private set; } = new Dictionary<string, Card>();
		public static Dictionary<int, Card> BaconPoolMinionsByDbfId { get; private set; } = new Dictionary<int, Card>();

		public static Dictionary<string, string> NormalToTripleCardIds { get; private set; } = new Dictionary<string, string>();
		public static Dictionary<string, string> TripleToNormalCardIds { get; private set; } = new Dictionary<string, string>();

		public static Dictionary<int, int> NormalToTripleDbfIds { get; private set; } = new Dictionary<int, int>();
		public static Dictionary<int, int> TripleToNormalDbfIds { get; private set; } = new Dictionary<int, int>();
		
		public static Dictionary<string, int> CardIdToDbfId { get; private set; } = new Dictionary<string, int>();
		public static Dictionary<int, string> DbfIdToCardId { get; private set; } = new Dictionary<int, string>();

		private static readonly HashSet<string> IgnoreTripleIds = new HashSet<string>
		{
			CardIds.NonCollectible.Priest.GhastcoilerTROLL,
			CardIds.NonCollectible.Mage.GlyphGuardian,
			CardIds.NonCollectible.Neutral.SeabreakerGoliathGILNEAS
		};

		private static readonly XmlSerializer CardDefsSerializer = new XmlSerializer(typeof(CardDefs.CardDefs));

		public static string Build { get; private set; }

		private static (string ETag, string LastModified)? _bundledCardDefsETag;
		public static (string ETag, string LastModified) GetBundledCardDefsETag()
		{
			if(_bundledCardDefsETag == null)
			{
				using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HearthDb.CardDefs.base.etag")!;
				using var reader = new StreamReader(stream);
				var text = reader.ReadToEnd().Split('\n');
				_bundledCardDefsETag = (text[0], text[1]);
			}
			return _bundledCardDefsETag.Value;
		}

		public static CardDefs.CardDefs GetBundledBaseData()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HearthDb.CardDefs.base.xml")!;
			return ParseCardDefs(stream);
		}

		static Cards()
		{
			if(Config.AutoLoadCardDefs)
				LoadBaseData();
		}

		public static CardDefs.CardDefs ParseCardDefs(Stream cardDefsData)
		{
			return (CardDefs.CardDefs)CardDefsSerializer.Deserialize(cardDefsData);
		}

		/// <summary>
		/// Load base card data (non-localized card metadata, as well as
		/// localized strings in enUS and zhCN) bundled with HearthDb.
		/// </summary>
		public static void LoadBaseData() => LoadBaseData(GetBundledBaseData());

		/// <summary>
		/// Load base card data, this should usually be a
		/// <c>CardDefs.base.xml</c> file, that at least includes all
		/// non-localized metadata.
		///
		/// This will clear any previously loaded data.
		/// </summary>
		public static void LoadBaseData(Stream cardDefsData) => LoadBaseData(ParseCardDefs(cardDefsData));

		/// <summary>
		/// Load base card data, this should usually be a
		/// <c>CardDefs.base.xml</c> file, that at least includes all
		/// non-localized metadata.
		///
		/// This will clear any previously loaded data.
		/// </summary>
		public static void LoadBaseData(CardDefs.CardDefs cardDefs)
		{
			// Instantiate new dictionaries here and re-assign once complete to avoid modifying collections.
			// This may be called in a thread.

			var all  = new Dictionary<string, Card>();
			var allByDbfId  = new Dictionary<int, Card>();
			var collectible  = new Dictionary<string, Card>();
			var collectibleByDbfId  = new Dictionary<int, Card>();
			var baconPoolMinions  = new Dictionary<string, Card>();
			var baconPoolMinionsByDbfId  = new Dictionary<int, Card>();
			var cardIdToDbfId  = new Dictionary<string, int>();
			var dbfIdToCardId = new Dictionary<int, string>();

			Build = cardDefs.Build;

			var baconTriples = new List<(Card, int)>();
			var nonBaconTriples = new List<(Card, int)>();
			foreach (var entity in cardDefs.Entites)
			{
				var card = new Card(entity);
				all[entity.CardId] = card;
				allByDbfId[entity.DbfId] = card;
				cardIdToDbfId[entity.CardId] = entity.DbfId;
				dbfIdToCardId[entity.DbfId] = entity.CardId;
				if (card.Collectible && (card.Type != CardType.HERO || card.Set != CardSet.HERO_SKINS))
				{
					collectible[entity.CardId] = card;
					collectibleByDbfId[entity.DbfId] = card;
				}

				if (card.IsBaconPoolMinion)
				{
					baconPoolMinions[entity.CardId] = card;
					baconPoolMinionsByDbfId[entity.DbfId] = card;
				}

				if (!IgnoreTripleIds.Contains(entity.CardId))
				{
					var tripleDbfId = card.Entity.Tags.FirstOrDefault(x => x.EnumId == 1429);
					if (tripleDbfId != null)
					{
						if (card.IsBaconPoolMinion)
							baconTriples.Add((card, tripleDbfId.Value));
						else
							nonBaconTriples.Add((card, tripleDbfId.Value));
					}
				}
			}
			
			// For some reason these minions are missing divine shield
			foreach (var minionCardId in MinionsMissingDivineShield)
			{
				if (all.TryGetValue(minionCardId, out var minionCard))
					minionCard.Entity.Tags.Add(new Tag { EnumId = (int)GameTag.DIVINE_SHIELD, Value = 1 });
			}

			All = all;
			AllByDbfId = allByDbfId;
			Collectible = collectible;
			CollectibleByDbfId = collectibleByDbfId;
			BaconPoolMinions = baconPoolMinions;
			BaconPoolMinionsByDbfId = baconPoolMinionsByDbfId;
			CardIdToDbfId = cardIdToDbfId;
			DbfIdToCardId = dbfIdToCardId;

			var normalToTripleCardIds  = new Dictionary<string, string>();
			var tripleToNormalCardIds  = new Dictionary<string, string>();
			var normalToTripleDbfIds  = new Dictionary<int, int>();
			var tripleToNormalDbfIds  = new Dictionary<int, int>();

			// Triples have to be resolved after the first loop since we need to look up the triple card from the id
			// Loop over non-bacon first in case both contain a mapping to the same card.
			// We want to use the bacon one in that case.
			foreach (var (card, tripleDbfId) in nonBaconTriples.Concat(baconTriples))
			{
				if (!AllByDbfId.TryGetValue(tripleDbfId, out var triple))
					continue;
				normalToTripleCardIds[card.Id] = triple.Id;
				normalToTripleDbfIds[card.DbfId] = triple.DbfId;
				tripleToNormalCardIds[triple.Id] = card.Id;
				tripleToNormalDbfIds[triple.DbfId] = card.DbfId;
			}

			NormalToTripleCardIds = normalToTripleCardIds;
			TripleToNormalCardIds = tripleToNormalCardIds;
			NormalToTripleDbfIds = normalToTripleDbfIds;
			TripleToNormalDbfIds = tripleToNormalDbfIds;
		}

		/// <summary>
		/// Load additional locale specific card data (names, text, ...).
		/// <c>LoadBaseData()</c> must have been previously called if
		/// <c>Config.AutoLoadCardDefs</c> (enabled by default) was disabled.
		/// </summary>
		public static void LoadLocaleData(Stream cardDefsData, Locale locale) => LoadLocaleData(ParseCardDefs(cardDefsData), locale);

		/// <summary>
		/// Load additional locale specific card data (names, text, ...).
		/// <c>LoadBaseData()</c> must have been previously called if
		/// <c>Config.AutoLoadCardDefs</c> (enabled by default) was disabled.
		/// </summary>
		public static void LoadLocaleData(CardDefs.CardDefs cardDefs, Locale locale)
		{
			foreach (var entity in cardDefs.Entites)
			{
				if (!All.TryGetValue(entity.CardId, out var curr))
					continue;
				foreach (var tag in entity.Tags)
				{
					var currTag = curr.Entity.Tags.FirstOrDefault(x => x.EnumId == tag.EnumId);
					if (currTag == null)
						curr.Entity.Tags.Add(tag);
					else
					{
						switch (locale)
						{
							case Locale.deDE:
								currTag.LocStringDeDe = tag.LocStringDeDe;
								break;
							case Locale.enUS:
								currTag.LocStringEnUs = tag.LocStringEnUs;
								break;
							case Locale.esES:
								currTag.LocStringEsEs = tag.LocStringEsEs;
								break;
							case Locale.esMX:
								currTag.LocStringEsMx = tag.LocStringEsMx;
								break;
							case Locale.frFR:
								currTag.LocStringFrFr = tag.LocStringFrFr;
								break;
							case Locale.itIT:
								currTag.LocStringItIt = tag.LocStringItIt;
								break;
							case Locale.jaJP:
								currTag.LocStringJaJp = tag.LocStringJaJp;
								break;
							case Locale.koKR:
								currTag.LocStringKoKr = tag.LocStringKoKr;
								break;
							case Locale.plPL:
								currTag.LocStringPlPl = tag.LocStringPlPl;
								break;
							case Locale.ptBR:
								currTag.LocStringPtBr = tag.LocStringPtBr;
								break;
							case Locale.ruRU:
								currTag.LocStringRuRu = tag.LocStringRuRu;
								break;
							case Locale.zhCN:
								currTag.LocStringZhCn = tag.LocStringZhCn;
								break;
							case Locale.zhTW:
								currTag.LocStringZhTw = tag.LocStringZhTw;
								break;
							case Locale.thTH:
								currTag.LocStringThTh = tag.LocStringThTh;
								break;
						}
					}
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
		
		private static readonly List<string> MinionsMissingDivineShield = new List<string>
		{
			CardIds.NonCollectible.Neutral.DeflectOBot,
			CardIds.NonCollectible.Neutral.DeflectOBotTavernBrawl,
			CardIds.NonCollectible.Neutral.Gemsplitter,
			CardIds.NonCollectible.Neutral.Gemsplitter_Gemsplitter,
		};
	}
}