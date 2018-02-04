#region

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace HearthStone.Database.CardDefs
{
	public class Power
	{
		[XmlElement("PlayRequirement")]
		public List<PlayRequirement> PlayRequirements { get; set; }
	}
}