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

		public static readonly Dictionary<string, Card> Collectible = new Dictionary<string, Card>();

		static Cards()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HearthDb.CardDefs.xml");
			if(stream == null)
				return;
			using(TextReader tr = new StreamReader(stream))
			{
				var xml = new XmlSerializer(typeof(CardDefs.CardDefs));
				var cardDefs = (CardDefs.CardDefs)xml.Deserialize(tr);
				foreach(var entity in cardDefs.Entites)
				{
					// For some reason Deflect-o-bot is missing divine shield
					if (IsDeflectOBot(entity) && !entity.Tags.Any(x => x.EnumId == (int)GameTag.DIVINE_SHIELD))
						entity.Tags.Add(new Tag { EnumId = (int)GameTag.DIVINE_SHIELD, Value = 1 });

					var card = new Card(entity);
					All.Add(entity.CardId, card);
					if(card.Collectible && (card.Type != CardType.HERO || card.Set != CardSet.CORE && card.Set != CardSet.HERO_SKINS))
						Collectible.Add(entity.CardId, card);
				}
			}
		}

		public static Card GetFromName(string name, Locale lang, bool collectible = true)
			=> (collectible ? Collectible : All).Values.FirstOrDefault(x => x.GetLocName(lang)?.Equals(name, StringComparison.InvariantCultureIgnoreCase) ?? false);

        public static List<Card> GetFromFuzzyName(string name, Locale lang, bool collectible = true)
        {
            var values = (collectible ? Collectible : All).Values;
            var result = values.Where(x =>
                x.GetLocName(lang)?.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) >= 0);
            return result.ToList();
        }

        public static Card GetFromDbfId(int dbfId, bool collectibe = true)
			=> (collectibe ? Collectible : All).Values.FirstOrDefault(x => x.DbfId == dbfId);

		private static bool IsDeflectOBot(Entity entity) => entity.CardId == CardIds.NonCollectible.Neutral.DeflectOBot || entity.CardId == CardIds.NonCollectible.Neutral.DeflectOBotTavernBrawl;
	}
}