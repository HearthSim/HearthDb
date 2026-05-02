#region

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using HearthDb.Enums;
using static HearthDb.Enums.Locale;

#endregion

namespace HearthDb.CardDefs
{
	public class Entity
	{
		[XmlAttribute("CardID")]
		public string CardId { get; set; }

		[XmlAttribute("ID")]
		public int DbfId { get; set; }

		[XmlAttribute("version")]
		public int Version { get; set; }

		[XmlElement("MasterPower")]
		public string MasterPower { get; set; }

		[XmlElement("Tag")]
		public List<Tag> Tags { get; set; } = new List<Tag>();

		[XmlElement("ReferencedTag")]
		public List<Tag> ReferencedTags { get; set; } = new List<Tag>();

		[XmlElement("Power")]
		public List<Power> Powers { get; set; }

		[XmlElement("EntourageCard")]
		public List<EntourageCard> EntourageCards { get; set; } = new List<EntourageCard>();

		[XmlElement("TriggeredPowerHistoryInfo")]
		public TriggeredPowerHistoryInfo TriggeredPowerHistoryInfo { get; set; }

		public int GetTag(GameTag gameTag) => Tags.FirstOrDefault(x => x.EnumId == (int)gameTag)?.Value ?? 0;

		public int GetReferencedTag(GameTag gameTag) => ReferencedTags.FirstOrDefault(x => x.EnumId == (int)gameTag)?.Value ?? 0;

		public string GetInnerValue(GameTag gameTag) => Tags.FirstOrDefault(x => x.EnumId == (int)gameTag)?.InnerValue;

		public string GetLocString(GameTag gameTag, Locale lang)
		{
			var tag = Tags.FirstOrDefault(x => x.EnumId == (int)gameTag);
			if(tag == null)
				return null;
			if(tag.TypeString != "LocString")
				return null;
			switch(lang)
			{
				case deDE:
					return tag.LocStringDeDe;
				case enUS:
					return tag.LocStringEnUs;
				case esES:
					return tag.LocStringEsEs;
				case esMX:
					return tag.LocStringEsMx;
				case frFR:
					return tag.LocStringFrFr;
				case itIT:
					return tag.LocStringItIt;
				case jaJP:
					return tag.LocStringJaJp;
				case koKR:
					return tag.LocStringKoKr;
				case plPL:
					return tag.LocStringPlPl;
				case ptBR:
					return tag.LocStringPtBr;
				case ruRU:
					return tag.LocStringRuRu;
				case zhCN:
					return tag.LocStringZhCn;
				case zhTW:
					return tag.LocStringZhTw;
				case thTH:
					return tag.LocStringThTh;
				default:
					return null;
			}
		}
	}
}