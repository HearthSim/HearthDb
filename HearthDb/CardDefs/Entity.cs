#region

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using HearthDb.Enums;

#endregion

namespace HearthDb.CardDefs
{
    public class Entity
    {
        [XmlAttribute("CardID")]
        public string CardId { get; set; }

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

        public int GetTag(GameTag gameTag)
        {
            var tag = Tags.FirstOrDefault(x => x.EnumId == (int)gameTag);
            return tag?.Value ?? 0;
        }

        public int GetReferencedTag(GameTag gameTag)
        {
            var tag = ReferencedTags.FirstOrDefault(x => x.EnumId == (int)gameTag);
            return tag?.Value ?? 0;
        }

        public string GetInnerValue(GameTag gameTag)
        {
            var tag = Tags.FirstOrDefault(x => x.EnumId == (int)gameTag);
            return tag?.InnerValue;
        }

        public string GetLocString(GameTag gameTag, Language lang)
        {
            var tag = Tags.FirstOrDefault(x => x.EnumId == (int)gameTag);
            if(tag == null)
                return null;
            if(tag.TypeString != "LocString")
                return null;
            switch(lang)
            {
                case Language.deDE:
                    return tag.LocStringDeDe;
                case Language.enUS:
                    return tag.LocStringEnUs;
                case Language.esES:
                    return tag.LocStringEsEs;
                case Language.esMX:
                    return tag.LocStringEsMx;
                case Language.frFR:
                    return tag.LocStringFrFr;
                case Language.itIT:
                    return tag.LocStringItIt;
                case Language.jaJP:
                    return tag.LocStringJaJp;
                case Language.koKR:
                    return tag.LocStringKoKr;
                case Language.plPL:
                    return tag.LocStringPlPl;
                case Language.ptBR:
                    return tag.LocStringPtBr;
                case Language.ruRU:
                    return tag.LocStringRuRu;
                case Language.zhCN:
                    return tag.LocStringZhCn;
                case Language.zhTW:
                    return tag.LocStringZhTw;
                default:
                    return null;
            }
        }
    }
}