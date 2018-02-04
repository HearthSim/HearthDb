#region

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using HearthStone.Database.Enums;

#endregion

namespace HearthStone.Database.CardDefs
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
		public Power Power { get; set; }

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
				case Locale.deDE:
					return tag.LocStringDeDe;
				case Locale.enUS:
					return tag.LocStringEnUs;
				case Locale.esES:
					return tag.LocStringEsEs;
				case Locale.esMX:
					return tag.LocStringEsMx;
				case Locale.frFR:
					return tag.LocStringFrFr;
				case Locale.itIT:
					return tag.LocStringItIt;
				case Locale.jaJP:
					return tag.LocStringJaJp;
				case Locale.koKR:
					return tag.LocStringKoKr;
				case Locale.plPL:
					return tag.LocStringPlPl;
				case Locale.ptBR:
					return tag.LocStringPtBr;
				case Locale.ruRU:
					return tag.LocStringRuRu;
				case Locale.zhCN:
					return tag.LocStringZhCn;
				case Locale.zhTW:
					return tag.LocStringZhTw;
				case Locale.thTH:
					return tag.LocStringThTh;
				default:
					return null;
			}
		}
	}
}