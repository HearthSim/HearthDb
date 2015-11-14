#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace HearthDb.CARD
{
    public class Dbf
    {
        [XmlElement("Record")]
        public List<Record> Records { get; set; } = new List<Record>();
    }
}