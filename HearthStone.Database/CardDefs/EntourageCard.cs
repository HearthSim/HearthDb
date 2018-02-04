#region

using System.Xml.Serialization;

#endregion

namespace HearthStone.Database.CardDefs
{
	public class EntourageCard
	{
		[XmlAttribute("cardID")]
		public string CardId { get; set; }
	}
}