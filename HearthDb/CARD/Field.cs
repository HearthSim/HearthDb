#region

using System.Xml.Serialization;

#endregion

namespace HearthDb.CARD
{
    public class Field
    {
        [XmlAttribute("column")]
        public string Column { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}