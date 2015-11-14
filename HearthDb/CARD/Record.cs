#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace HearthDb.CARD
{
    public class Record
    {
        [XmlElement("Field")]
        public List<Field> Fields { get; set; } = new List<Field>();
    }
}