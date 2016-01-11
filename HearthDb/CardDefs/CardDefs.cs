#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace HearthDb.CardDefs
{
	[XmlRoot("CardDefs")]
	public class CardDefs
	{
		[XmlElement("Entity")]
		public List<Entity> Entites { get; set; }
	}
}